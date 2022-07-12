using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    #region Structs
    public struct ControllerState
    {
        public bool Sprint;

        public bool Jump;
        public bool HasJumped;
        public bool SkippedFrame;

        public Vector2 Axis;
    }
    #endregion

    private PlayerInput ControllerInput;
    private RaycastHit CachedMouseHit;
    public ControllerState State;
    public LayerMask MouseGrappleMask;

    #region UnityCallbacks
    private void Awake()
    {
        State.Sprint = false;
        State.Jump = false;
        State.Axis = new Vector2(0f, 0f);

        ControllerInput = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        UpdateMouseWorldPosition();
        State.Axis = ControllerInput.actions["Movement"].ReadValue<Vector2>();

        if (State.HasJumped && State.SkippedFrame)
        {
            State.Jump = false;
            State.HasJumped = false;
        }
        if (!State.SkippedFrame) State.SkippedFrame = true;
    }
    #endregion

    #region Get / Set
    public Vector3 GetMouseWorldPosition()
    {
        return CachedMouseHit.point;
    }

    public RaycastHit GetMouseHit()
    {
        return CachedMouseHit;
    }

    public Vector2 GetInputAxis()
    {
        return State.Axis;
    }

    private void UpdateMouseWorldPosition()
    {
        if (Camera.main != null)
        {
            Vector2 mouse_position = ControllerInput.actions["MousePosition"].ReadValue<Vector2>();
            Vector3 projected_mouse_position = Camera.main.ScreenToWorldPoint(
                new Vector3(mouse_position.x, mouse_position.y, Mathf.Abs(transform.position.z))
            );
            Vector3 direction = (projected_mouse_position - Camera.main.transform.position);
            RaycastHit MouseHit;
            Physics.Raycast(Camera.main.transform.position, direction, out MouseHit, Mathf.Infinity, MouseGrappleMask);
            if (MouseHit.collider)
            {
                CachedMouseHit = MouseHit;
            }
        }
        else
        {
            Debug.Log("main camera is null: ", this);
        }
    }
    #endregion

    #region Debugging
    private void OnDrawGizmos()
    {
        // Draw mouse world position
        if (ControllerInput != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(CachedMouseHit.point, .15f);
        }
    }
    #endregion

    #region InputSystemCallbacks
    public void OnSprint(InputAction.CallbackContext c)
    {
        if (c.phase == InputActionPhase.Performed)
        {
            State.Sprint = true;
        }
        else if (c.phase == InputActionPhase.Canceled)
        {
            State.Sprint = false;
        }
    }

    public void OnJump(InputAction.CallbackContext c)
    {
        if (c.phase == InputActionPhase.Performed)
        {
            State.Jump = true;
            State.HasJumped = true;
            State.SkippedFrame = false;
        }
        else if (c.phase == InputActionPhase.Canceled)
        {
            State.Jump = false;
        }
    }
    #endregion
}
