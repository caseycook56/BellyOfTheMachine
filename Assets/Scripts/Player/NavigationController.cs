using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationController : MonoBehaviour
{
    PhysicsBody Body;
    InputController ControllerInput;

    #region MovementSettings
    [Tooltip("base movement speed")]
    public float MovementSpeed = 14f;
    public float SprintSpeed = 20f;

    [Range(0.01f, 0.99f)]
    [Tooltip("minimum input value to trigger movement")]
    public float InputThreshold = 0.01f;

    [Tooltip("speed up")]
    public float DampSpeedUp = 0.2f;

    [Tooltip("slow down")]
    public float DampSlowDown = 0.1f;

    [Tooltip("jump velocity")]
    public float JumpVelocity = 20f;
    #endregion

    #region UnityCallbacks
    private void Awake()
    {
        Body = gameObject.GetOrAddComponent<PhysicsBody>();
        ControllerInput = gameObject.GetOrAddComponent<InputController>();
    }

    private void FixedUpdate()
    {
        Body.UpdateBody();
        Body.CheckWall();

        Move();
        Jump();
        Rotate();

        if (!Body.DisableGravity)
        {
            Body.ApplyGravity();
        }
    }
    #endregion

    #region Movement

    private void Move()
    {
        Vector2 InputAxis = ControllerInput.GetInputAxis();
        Vector3 DesiredVelocity = Body.CurrentRelativeWorldDirection.RelativeForward;


        if (InputAxis.magnitude > InputThreshold)
        {
            Body.TargetAngle = Mathf.Atan2(InputAxis.x, InputAxis.y) * Mathf.Rad2Deg;
            if (ControllerInput.State.Sprint)
            {
                Body.UpdateVelocity(DesiredVelocity * SprintSpeed, DampSpeedUp);
            }
            else
            {
                Body.UpdateVelocity(DesiredVelocity * MovementSpeed, DampSpeedUp);
            }
        }
        else
        {
            Body.UpdateVelocity(Vector3.zero, DampSlowDown);
        }
    }

    private void Rotate()
    {
        //float step = 20f;
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, Body.TargetAngle, 0f), step);
        transform.rotation = Quaternion.Euler(0f, Body.TargetAngle, 0f);
    }

    private void Jump()
    {
        PhysicsBody.CharacterState CurrentState = Body.CurrentCharacterState;
        PhysicsBody.CharacterState PreviousState = Body.PreviousCharacterState;
        PhysicsBody.RelativeWorldDirections CurrentRelativeWorldDirection = Body.CurrentRelativeWorldDirection;
        PhysicsBody.RelativeWorldDirections GlobalRelativeWorldDirection = Body.GlobalRelativeWorldDirection;

        if (ControllerInput.State.Jump && CurrentState.IsGrounded)
        {
            CurrentState.IsJumping = true;
            Body.AddForce(Vector3.up * JumpVelocity);
        }
        else CurrentState.IsJumping = false;
    }

    #endregion
}
