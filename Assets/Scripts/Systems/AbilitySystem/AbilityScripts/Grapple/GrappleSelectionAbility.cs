using System;
using UnityEngine;

[CreateAssetMenu]
public class GrappleSelectionAbility : Ability
{
    public int MaxPoints;
    [HideInInspector]
    public int count = 0;
    PhysicsBody body;
    InputController controller;

    public override void Activate(GameObject parent)
    {
        body = parent.GetComponent<PhysicsBody>();
        controller = parent.GetComponent<InputController>();

        if (body.GrappleSelectionBegun || body.GrappleSelectionComplete)
        {
            AddGrapple();
        }
    }

    public override void Deactivate(GameObject parent)
    {
        
    }

    private void AddGrapple()
    {
        RaycastHit hit = controller.GetMouseHit();
        if (hit.collider != null)
        {
            body.points.Add(hit.point);
            if (body.points.Count > MaxPoints || body.GrappleSelectionComplete)
            {
                body.points.RemoveAt(0);
            }
            count = body.points.Count;
        }
    }
}
