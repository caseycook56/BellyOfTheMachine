using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBody : MonoBehaviour
{
    private CapsuleCollider Collider;
    private Rigidbody rb;

    #region Structs
    public struct CharacterState
    {
        public bool IsTouchingStep;
        public bool IsTouchingWall;
        public bool IsTouchingSlope;
        public bool IsGrounded;
        public bool IsJumping;

        public bool LockOnSlope;

        public float SurfaceAngle;

        public Vector3 GroundNormal;
        public Vector3 WallNormal;
    }

    public struct RelativeWorldDirections
    {
        public Vector3 RelativeDown;
        public Vector3 RelativeForward;
    }
    public struct PhysicsState
    {
        public Vector3 Velocity;
    }
    #endregion

    #region Exposed Values
    [HideInInspector]
    public PhysicsState CurrentPhysicsState;
    [HideInInspector]
    public PhysicsState PreviousPhysicsState;
    [HideInInspector]
    public RelativeWorldDirections CurrentRelativeWorldDirection;
    [HideInInspector]
    public RelativeWorldDirections GlobalRelativeWorldDirection;
    [HideInInspector]
    public CharacterState CurrentCharacterState;
    [HideInInspector]
    public CharacterState PreviousCharacterState;
    // lock onto slopes
    [HideInInspector]
    public bool Lock = true;
    // rotation angle
    [HideInInspector]
    public float TargetAngle;
    #endregion

    #region Settings
    public float GroundCheckRadius;
    public LayerMask GroundMask;

    [Range(0f, 1f)]
    [Tooltip("friction against floor")]
    public float FloorFriction = 0.3f;

    [Range(0.01f, 0.99f)]
    [Tooltip("friction against wall")]
    public float WallFriction = 0.839f;

    [Tooltip("distance from the player feet used to check if the player is touching a slope")]
    public float SlopeCheckThreshold = 0.51f;
    
    //[Tooltip("Distance from the player feet used to check if the player is touching the ground")]
    //public float GroundCheckThreshold = 0.2f;

    [Tooltip("Distance from the player head used to check if the player is touching a wall")]
    public float WallCheckThreshold = 0.8f;

    [Tooltip("Distance from the player center used to check if the player is touching a step")]
    public float StepCheckThreshold = 0.6f;

    [Range(1f, 89f)]
    [Tooltip("max walkable slope angle")]
    public float MaxSlopeAngle = 53.6f;

    [Tooltip("Max climbable step height")]
    public float MaxStepHeight = 0.74f;

    [Tooltip("speed multiplier based on slope angle")]
    public AnimationCurve SpeedMultiplierOnAngle = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Range(0.01f, 1f)]
    [Tooltip("Multipler factor on step")]
    public float ClimbingStairsMultiplierCurve = 0.637f;

    [Range(0.01f, 1f)]
    [Tooltip("Multipler factor on climbable slope")]
    public float CanSlideMultiplierCurve = 0.061f;

    [Tooltip("Multipler factor for gravity used on non climbable slope")]
    public float GravityMultiplierIfUnclimbableSlope = 30f;

    [Tooltip("Wall checker distance from the player center")]
    public float HightWallCheck = 0.5f;

    [Range(0.01f, 1f)]
    [Tooltip("Multipler factor on non climbable slope")]
    public float CantSlideMultiplierCurve = 0.039f;
    #endregion

    #region GRAPPLE TEMP LOCATION
    private void GrappleUpdate()
    {
        Vector3 AveragePosition = Vector3.zero;

        if (GrappleSelectionComplete)
        {
            AveragePosition = AveragePoint(points.ToArray());
        }

        if (AveragePosition != Vector3.zero)
        {
            DisableGravity = true;
            Vector3 DesiredVelocity = AveragePosition - transform.position;
            if (!float.IsNaN(DesiredVelocity.x) && !float.IsNaN(DesiredVelocity.y) && !float.IsNaN(DesiredVelocity.z))
            {
                move(DesiredVelocity);
            }
            GrappleSelectionBegun = false;
        }
    }
    private void move(Vector3 velocity)
    {
        float grapple_speed = 100f;
        float acceleration = 40f;
        if (velocity.sqrMagnitude > 1)
        {
            velocity = velocity.normalized;
        }
        velocity *= grapple_speed;
        var a = (velocity - rb.velocity) * acceleration;
        if (a.sqrMagnitude > acceleration * acceleration)
        {
            a = a.normalized * acceleration;
        }
        rb.AddForce(a);
    }

    private Vector3 AveragePoint(Vector3[] points)
    {
        Vector3 centre = Vector3.zero;
        foreach (Vector3 point in points)
        {
            centre += point;
        }
        centre /= points.Length;
        return centre;
    }
    [HideInInspector]
    public bool GrappleSelectionBegun = false;
    [HideInInspector]
    public bool GrappleSelectionComplete = false;
    [HideInInspector]
    public List<Vector3> points = new List<Vector3>();
    [HideInInspector]
    public bool DisableGravity = false;
    #endregion

    public void UpdateBody()
    {
        CheckGrounded();
        CurrentCharacterState.IsTouchingStep = CheckStep(CurrentRelativeWorldDirection.RelativeForward.normalized - new Vector3(0, BottomStepOffset, 0));
        UpdateState();
    }

    public void UpdateState()
    {
        RaycastHit FloorHit;
        CheckSurface(out FloorHit);

        //Debug.DrawRay(transform.position, AverageSurfaceNormal(), Color.white);

        if (FloorHit.collider)
        {
            float SlopeSpeedCurve = SpeedMultiplierOnAngle.Evaluate(CurrentCharacterState.SurfaceAngle / 90f);

            CurrentCharacterState.SurfaceAngle = Vector3.Angle(Vector3.up, FloorHit.normal);
            CurrentRelativeWorldDirection.RelativeDown = Vector3.Project(Vector3.down, FloorHit.normal);
            GlobalRelativeWorldDirection.RelativeDown = Vector3.down.normalized;

            SetFriction(FloorFriction, true);

            RaycastHit SlopeHit;
            bool IsFacingSlope = FacingSlope(GlobalRelativeWorldDirection.RelativeForward, out SlopeHit);

            if (RoundValue(FloorHit.normal.y) == 1 && !IsFacingSlope)
            {
                CurrentRelativeWorldDirection.RelativeForward = Quaternion.Euler(0f, TargetAngle, 0f) * Vector3.forward;
                GlobalRelativeWorldDirection.RelativeForward = CurrentRelativeWorldDirection.RelativeForward;
                CurrentCharacterState.SurfaceAngle = 0f;
                CurrentCharacterState.IsTouchingSlope = false;
            }
            else if ((IsFacingSlope || OnSlope()) && !CurrentCharacterState.IsTouchingStep)
            {
                Vector3 TempForward;

                if (IsFacingSlope)
                {
                    TempForward = new Vector3(
                        transform.forward.normalized.x,
                        Vector3.ProjectOnPlane(transform.forward.normalized, SlopeHit.normal).normalized.y,
                        transform.forward.z
                    );
                } 
                else
                {
                    TempForward = new Vector3(
                        transform.forward.normalized.x,
                        Vector3.ProjectOnPlane(transform.forward.normalized, FloorHit.normal).normalized.y,
                        transform.forward.normalized.z
                    );
                }

                CurrentRelativeWorldDirection.RelativeForward = TempForward * ((SlopeSpeedCurve * CanSlideMultiplierCurve) + 1f);
                GlobalRelativeWorldDirection.RelativeForward = transform.forward.normalized * ((SlopeSpeedCurve * CanSlideMultiplierCurve) + 1f);
            }
            else if (CurrentCharacterState.IsTouchingStep)
            {
                Vector3 TempForward = new Vector3(
                    transform.forward.normalized.x,
                    Vector3.ProjectOnPlane(transform.forward.normalized, FloorHit.normal).normalized.y,
                    transform.forward.normalized.z
                );

                floorPoint = FloorHit.point;
                
                // TODO:
                // note: moving the rigid body shouldn't be done here
                // move to the intersection point of the step
                rb.position = (FloorHit.point + new Vector3(0, Collider.height / 2, 0));
                CurrentRelativeWorldDirection.RelativeForward = TempForward * ((SlopeSpeedCurve * ClimbingStairsMultiplierCurve));
                GlobalRelativeWorldDirection.RelativeForward = transform.forward.normalized * ((SlopeSpeedCurve * ClimbingStairsMultiplierCurve));
            }
            else
            {
                Vector3 TempForward = new Vector3(
                    transform.forward.normalized.x,
                    Vector3.ProjectOnPlane(transform.forward.normalized, FloorHit.normal).normalized.y,
                    transform.forward.normalized.z
                );
                CurrentRelativeWorldDirection.RelativeForward = TempForward * ((SlopeSpeedCurve * CantSlideMultiplierCurve));
                GlobalRelativeWorldDirection.RelativeForward = transform.forward.normalized * ((SlopeSpeedCurve * CantSlideMultiplierCurve));
                SetFriction(0f, true);
            }
        }
        else
        {
            CurrentCharacterState.GroundNormal = Vector3.zero;
            CurrentRelativeWorldDirection.RelativeForward = Vector3.ProjectOnPlane(transform.forward, FloorHit.normal).normalized;
            GlobalRelativeWorldDirection.RelativeForward = CurrentRelativeWorldDirection.RelativeForward;
            CurrentRelativeWorldDirection.RelativeDown = Vector3.down.normalized;
            GlobalRelativeWorldDirection.RelativeDown = Vector3.down.normalized;
            CurrentCharacterState.LockOnSlope = Lock;
        }       
    }
    Vector3 floorPoint = Vector3.zero;
    public bool OnStep()
    {
        return CheckStep(GlobalRelativeWorldDirection.RelativeForward) || CheckStep(-GlobalRelativeWorldDirection.RelativeForward);
    }

    public bool FacingSlope (Vector3 direction, out RaycastHit hit)
    {
        float RayDistance = Collider.radius + 0.1f;
        if (Physics.Raycast(transform.position - new Vector3(0f, Collider.height / 2, 0f), direction, out hit, 1f, GroundMask))
        {
            float SurfaceAngle = Vector3.Angle(Vector3.up, hit.normal);
            Debug.DrawLine(transform.position - new Vector3(0f, Collider.height / 2, 0f), hit.point, Color.magenta);
            return SurfaceAngle <= MaxSlopeAngle;
        }
        return false;
    }

    public bool OnSlope()
    {
        RaycastHit hit;
        float RayDistance = (Collider.height / 2f) + 0.1f;
        Physics.Raycast(transform.position, Vector3.down, out hit, RayDistance, GroundMask);
        return hit.normal.Round(1) != Vector3.up;
    }

    #region UnityCallbacks
    private void FixedUpdate()
    {
        GrappleUpdate();
    }

    private void Awake()
    {
        Collider = gameObject.GetOrAddComponent<CapsuleCollider>();

        CurrentCharacterState = new CharacterState();
        PreviousCharacterState = new CharacterState();

        CurrentPhysicsState = new PhysicsState();
        PreviousPhysicsState = new PhysicsState();

        CurrentRelativeWorldDirection = new RelativeWorldDirections();
        GlobalRelativeWorldDirection = new RelativeWorldDirections();

        rb = gameObject.GetOrAddComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.drag = 0.5f;
        SetFriction(FloorFriction, true);
    }
    #endregion

    #region Get / Set
    public void SetGravity(bool value)
    {
        rb.useGravity = value;
    }

    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }

    public Vector3 GetPosition()
    {
        return rb.position;
    }
    #endregion

    #region World Checks
    public void CheckGrounded()
    {
        PreviousCharacterState.IsGrounded = CurrentCharacterState.IsGrounded;
        CurrentCharacterState.IsGrounded = Physics.CheckSphere(
            transform.position - new Vector3(0, Collider.height / 2f, 0),
            GroundCheckRadius,
            GroundMask
        );
    }

    public void CheckSurface(out RaycastHit hit)
    {
        Vector3 right = Vector3.Cross(Vector3.up, CurrentRelativeWorldDirection.RelativeForward);
        Vector3 TempDown = Vector3.Cross(right, CurrentRelativeWorldDirection.RelativeForward);
        Debug.DrawRay(transform.position, right, Color.red);

        //Physics.SphereCast(transform.position + new Vector3(0f, Collider.height / 2, 0f), SlopeCheckThreshold, Vector3.down, out hit, 1f, GroundMask);
        
        Debug.DrawRay(transform.position, TempDown, Color.blue);
        Physics.Raycast(transform.position + new Vector3(0f, Collider.height / 2, 0f), TempDown, out hit, 1f, GroundMask);
    }

    private Vector3 AverageSurfaceNormal()
    {
        Vector3 center = transform.position;
        Vector3 infront = center + CurrentRelativeWorldDirection.RelativeForward;
        Vector3 behind = center - CurrentRelativeWorldDirection.RelativeForward;
        Vector3 left = center - Vector3.Cross(CurrentRelativeWorldDirection.RelativeForward, Vector3.down);
        Vector3 right = center + Vector3.Cross(CurrentRelativeWorldDirection.RelativeForward, Vector3.down);

        RaycastHit[] hits = new RaycastHit[5];

        Physics.SphereCast(center + new Vector3(0f, Collider.height / 2, 0f), SlopeCheckThreshold, Vector3.down, out hits[0], 1f, GroundMask);
        Physics.SphereCast(center + new Vector3(0f, Collider.height / 2, 0f), SlopeCheckThreshold, Vector3.down, out hits[1], 1f, GroundMask);
        Physics.SphereCast(center + new Vector3(0f, Collider.height / 2, 0f), SlopeCheckThreshold, Vector3.down, out hits[2], 1f, GroundMask);
        Physics.SphereCast(center + new Vector3(0f, Collider.height / 2, 0f), SlopeCheckThreshold, Vector3.down, out hits[3], 1f, GroundMask);
        Physics.SphereCast(center + new Vector3(0f, Collider.height / 2, 0f), SlopeCheckThreshold, Vector3.down, out hits[4], 1f, GroundMask);

        Vector3 average = Vector3.zero;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider == null) return Vector3.zero;

            average += hits[i].normal;
        }

        return average / hits.Length;
    }

    [Range(0f, 1f)]
    public float BottomStepOffset = 0;
    private bool CheckStep(Vector3 direction)
    {
        bool tmpStep = false;
        Vector3 bottomStepPos = transform.position - new Vector3(0f, BottomStepOffset, 0f);

        RaycastHit stepLowerHit;
        if (Physics.Raycast(bottomStepPos, direction, out stepLowerHit, StepCheckThreshold, GroundMask))
        {
            RaycastHit stepUpperHit;
            if (RoundValue(stepLowerHit.normal.y) == 0 && 
                !Physics.Raycast(
                    bottomStepPos + new Vector3(0f, MaxStepHeight, 0f), 
                    GlobalRelativeWorldDirection.RelativeForward, 
                    out stepUpperHit, 
                    StepCheckThreshold, 
                    GroundMask))
            {
                tmpStep = true;
            }
        }

        RaycastHit stepLowerHit45;
        if (Physics.Raycast(
            bottomStepPos, 
            Quaternion.AngleAxis(45, transform.up) * direction, 
            out stepLowerHit45, 
            StepCheckThreshold, 
            GroundMask))
        {
            RaycastHit stepUpperHit45;
            if (RoundValue(stepLowerHit45.normal.y) == 0 && 
                !Physics.Raycast(
                    bottomStepPos + new Vector3(0f, MaxStepHeight, 0f), 
                    Quaternion.AngleAxis(45, Vector3.up) * direction, 
                    out stepUpperHit45, 
                    StepCheckThreshold, 
                    GroundMask))
            {
                tmpStep = true;
            }
        }

        RaycastHit stepLowerHitMinus45;
        if (Physics.Raycast(
            bottomStepPos, 
            Quaternion.AngleAxis(-45, transform.up) * direction, 
            out stepLowerHitMinus45, 
            StepCheckThreshold, GroundMask))
        {
            RaycastHit stepUpperHitMinus45;
            if (RoundValue(stepLowerHitMinus45.normal.y) == 0 && 
                !Physics.Raycast(
                    bottomStepPos + new Vector3(0f, MaxStepHeight, 0f), 
                    Quaternion.AngleAxis(-45, Vector3.up) * direction, 
                    out stepUpperHitMinus45, 
                    StepCheckThreshold, 
                    GroundMask))
            {
                tmpStep = true;
            }
        }

        return tmpStep;
    }
    public void CheckWall()
    {
        bool tmpWall = false;
        Vector3 tmpWallNormal = Vector3.zero;
        Vector3 topWallPos = new Vector3(transform.position.x, transform.position.y + HightWallCheck, transform.position.z);

        RaycastHit wallHit;
        if (Physics.Raycast(
            topWallPos, 
            GlobalRelativeWorldDirection.RelativeForward, 
            out wallHit, 
            WallCheckThreshold, 
            GroundMask))
        {
            tmpWallNormal = wallHit.normal;
            tmpWall = true;
        }
        else if (Physics.Raycast(
            topWallPos, 
            Quaternion.AngleAxis(45, transform.up) * GlobalRelativeWorldDirection.RelativeForward, 
            out wallHit, 
            WallCheckThreshold, 
            GroundMask))
        {
            tmpWallNormal = wallHit.normal;
            tmpWall = true;
        }
        else if (Physics.Raycast(
            topWallPos, 
            Quaternion.AngleAxis(90, transform.up) * GlobalRelativeWorldDirection.RelativeForward, 
            out wallHit, 
            WallCheckThreshold, 
            GroundMask))
        {
            tmpWallNormal = wallHit.normal;
            tmpWall = true;
        }
        else if (Physics.Raycast(
            topWallPos, 
            Quaternion.AngleAxis(135, transform.up) * GlobalRelativeWorldDirection.RelativeForward, 
            out wallHit, 
            WallCheckThreshold, 
            GroundMask))
        {
            tmpWallNormal = wallHit.normal;
            tmpWall = true;
        }
        else if (Physics.Raycast(
            topWallPos, 
            Quaternion.AngleAxis(180, transform.up) * GlobalRelativeWorldDirection.RelativeForward, 
            out wallHit, 
            WallCheckThreshold, 
            GroundMask))
        {
            tmpWallNormal = wallHit.normal;
            tmpWall = true;
        }
        else if (Physics.Raycast(
            topWallPos, 
            Quaternion.AngleAxis(225, transform.up) * GlobalRelativeWorldDirection.RelativeForward, 
            out wallHit, 
            WallCheckThreshold, 
            GroundMask))
        {
            tmpWallNormal = wallHit.normal;
            tmpWall = true;
        }
        else if (Physics.Raycast(
            topWallPos, 
            Quaternion.AngleAxis(270, transform.up) * GlobalRelativeWorldDirection.RelativeForward, 
            out wallHit, 
            WallCheckThreshold, 
            GroundMask))
        {
            tmpWallNormal = wallHit.normal;
            tmpWall = true;
        }
        else if (Physics.Raycast(
            topWallPos, 
            Quaternion.AngleAxis(315, transform.up) * GlobalRelativeWorldDirection.RelativeForward, 
            out wallHit, 
            WallCheckThreshold, 
            GroundMask))
        {
            tmpWallNormal = wallHit.normal;
            tmpWall = true;
        }

        CurrentCharacterState.IsTouchingWall = tmpWall;
        CurrentCharacterState.WallNormal = tmpWallNormal;
    }
    public bool IsFalling()
    {
        return rb.velocity.y < 0;
    }
    #endregion

    #region Body Interaction
    public void UpdateVelocity(Vector3 DesiredVelocity, float SmoothTime)
    {
        rb.velocity = Vector3.SmoothDamp(rb.velocity, DesiredVelocity, ref CurrentPhysicsState.Velocity, SmoothTime);
    }

    public void AddForce(Vector3 force)
    {
        rb.AddForce(force);
    }
    public void SetFriction(float _frictionWall, bool _isMinimum)
    {
        Collider.material.dynamicFriction = 0.6f * _frictionWall;
        Collider.material.staticFriction = 0.6f * _frictionWall;

        if (_isMinimum) Collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
        else Collider.material.frictionCombine = PhysicMaterialCombine.Maximum;
    }

    public void MovePosition(Vector3 destination)
    {
        rb.MovePosition(destination);
    }



    [Tooltip("factor for gravity")]
    public float GroundedGravityMultiplier = 3f;
    public float FallingGravityMultiplier = .5f;
    public void ApplyGravity()
    {
        Vector3 gravity;

        RaycastHit SlopeHit;
        bool IsFacingSlope = FacingSlope(GlobalRelativeWorldDirection.RelativeForward, out SlopeHit);
        if (CurrentCharacterState.IsGrounded && !OnStep() && (OnSlope() || IsFacingSlope))
        {
            Debug.DrawLine(transform.position, transform.position + CurrentRelativeWorldDirection.RelativeDown, Color.red);
            gravity = CurrentRelativeWorldDirection.RelativeDown * GroundedGravityMultiplier * -Physics.gravity.y;
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + GlobalRelativeWorldDirection.RelativeDown, Color.green);
            //gravity = GlobalRelativeWorldDirection.RelativeDown * GravityMultiplier * -Physics.gravity.y;
            gravity = GlobalRelativeWorldDirection.RelativeDown * FallingGravityMultiplier * -Physics.gravity.y;
        }

        if (CurrentCharacterState.GroundNormal.y != 1 &&
            CurrentCharacterState.GroundNormal.y != 0 &&
            (CurrentCharacterState.SurfaceAngle > MaxSlopeAngle &&
            !CurrentCharacterState.IsTouchingStep))
        {
            if (CurrentCharacterState.SurfaceAngle > 0f && CurrentCharacterState.SurfaceAngle <= 30f)
            {
                gravity = GlobalRelativeWorldDirection.RelativeDown * GravityMultiplierIfUnclimbableSlope * -Physics.gravity.y;
            }
            else if (CurrentCharacterState.SurfaceAngle > 30f && CurrentCharacterState.SurfaceAngle <= 89f)
            {
                gravity = GlobalRelativeWorldDirection.RelativeDown * GravityMultiplierIfUnclimbableSlope / 2f * -Physics.gravity.y;
            }
        }

        //if (CurrentCharacterState.IsTouchingWall && IsFalling())
        //{
        //    gravity *= WallFriction;
        //}
        AddForce(gravity);
    }
    #endregion

    #region Debugging
    private void OnDrawGizmos()
    {
        if (Collider != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(floorPoint, .02f);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position - new Vector3(0, Collider.height / 2f, 0), GroundCheckRadius);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position - new Vector3(0, Collider.height / 2f, 0), SlopeCheckThreshold);

            //direction
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position +( Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + CurrentRelativeWorldDirection.RelativeForward);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + GlobalRelativeWorldDirection.RelativeForward);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position + (Vector3.up * 0.2f), transform.position + (Vector3.up * 0.2f) + transform.forward);

            ////step check
            Vector3 bottomStepPos = transform.position - new Vector3(0f, BottomStepOffset, 0f);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(bottomStepPos, bottomStepPos + GlobalRelativeWorldDirection.RelativeForward.normalized * StepCheckThreshold);

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(
                bottomStepPos + new Vector3(0f, MaxStepHeight, 0f), 
                bottomStepPos + new Vector3(0f, MaxStepHeight, 0f) + GlobalRelativeWorldDirection.RelativeForward.normalized * StepCheckThreshold
            );

            Gizmos.DrawLine(
                bottomStepPos, 
                bottomStepPos + Quaternion.AngleAxis(45, transform.up) * (GlobalRelativeWorldDirection.RelativeForward * StepCheckThreshold)
            );
            Gizmos.DrawLine(bottomStepPos + new Vector3(0f, MaxStepHeight, 0f), 
                bottomStepPos + Quaternion.AngleAxis(45, Vector3.up) * (GlobalRelativeWorldDirection.RelativeForward.normalized * StepCheckThreshold) + new Vector3(0f, MaxStepHeight, 0f)
            );
            Gizmos.DrawLine(
                bottomStepPos, 
                bottomStepPos + Quaternion.AngleAxis(-45, transform.up) * (GlobalRelativeWorldDirection.RelativeForward * StepCheckThreshold)
            );
            Gizmos.DrawLine(bottomStepPos + new Vector3(0f, MaxStepHeight, 0f), 
                bottomStepPos + Quaternion.AngleAxis(-45, Vector3.up) * (GlobalRelativeWorldDirection.RelativeForward.normalized * StepCheckThreshold) + new Vector3(0f, MaxStepHeight, 0f)
            );

            //wall check
            Vector3 topWallPos = new Vector3(transform.position.x, transform.position.y + HightWallCheck, transform.position.z);
            Gizmos.color = Color.black;
            Gizmos.DrawLine(topWallPos, topWallPos + GlobalRelativeWorldDirection.RelativeForward * WallCheckThreshold);

            Gizmos.color = Color.black;
            Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(45, transform.up) * (GlobalRelativeWorldDirection.RelativeForward * WallCheckThreshold));

            Gizmos.color = Color.black;
            Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(90, transform.up) * (GlobalRelativeWorldDirection.RelativeForward * WallCheckThreshold));

            Gizmos.color = Color.black;
            Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(135, transform.up) * (GlobalRelativeWorldDirection.RelativeForward * WallCheckThreshold));

            Gizmos.color = Color.black;
            Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(180, transform.up) * (GlobalRelativeWorldDirection.RelativeForward * WallCheckThreshold));

            Gizmos.color = Color.black;
            Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(225, transform.up) * (GlobalRelativeWorldDirection.RelativeForward * WallCheckThreshold));

            Gizmos.color = Color.black;
            Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(270, transform.up) * (GlobalRelativeWorldDirection.RelativeForward * WallCheckThreshold));

            Gizmos.color = Color.black;
            Gizmos.DrawLine(topWallPos, topWallPos + Quaternion.AngleAxis(315, transform.up) * (GlobalRelativeWorldDirection.RelativeForward * WallCheckThreshold));

            foreach (Vector3 p in points)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(p, .15f);
            }
        }
    }
    #endregion

    #region Helpers
    private float RoundValue(float _value)
    {
        float unit = (float)Mathf.Round(_value);

        if (_value - unit < 0.000001f && _value - unit > -0.000001f) return unit;
        else return _value;
    }
    #endregion
}
