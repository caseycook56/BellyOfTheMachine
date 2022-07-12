using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class JumpAbility : Ability
{
    private IEnumerator activeJumpCoroutine;
    public float JumpProgress { get; private set; }

    public override void Activate(GameObject parent)
    {
        PhysicsBody body = parent.GetComponent<PhysicsBody>();
        InputController controller = parent.GetComponent<InputController>();
        if (body != null && controller != null)
        {
            //Jump(body, controller.GetMouseWorldPosition());
        }
    }
    public override void Deactivate(GameObject parent)
    {

    }
    //private void Jump(PhysicsBody body, Vector3 target)
    //{
    //    if (activeJumpCoroutine != null)
    //    {
    //        StopCoroutine(activeJumpCoroutine);
    //        activeJumpCoroutine = null;
    //        JumpProgress = 0.0f;
    //    }

    //    float maxHeight = 1f;
    //    float time = .8f;
    //    activeJumpCoroutine = JumpCoroutine(body, target, maxHeight, time);
    //    StartCoroutine(activeJumpCoroutine);
    //}
    //private IEnumerator JumpCoroutine(PhysicsBody body, Vector3 target, float maxHeight, float time)
    //{
    //    var StartPosition = body.transform.position;
    //    while (JumpProgress <= 1.0)
    //    {
    //        JumpProgress += Time.deltaTime / time;
    //        var height = Mathf.Sin(Mathf.PI * JumpProgress) * maxHeight;
    //        if (height < 0f)
    //        {
    //            height = 0f;
    //        }

    //        //TODO ADDFORCE!
    //        //body.DesiredVelocity -= Vector3.Lerp(StartPosition, target, JumpProgress) + Vector3.up * height;
    //        //body.transform.position = Vector3.Lerp(StartPosition, target, JumpProgress) + Vector3.up * height;

    //        if (body.CheckCollisions())
    //        {
    //            break;
    //        }

    //        yield return null;
    //    }
    //}
}
