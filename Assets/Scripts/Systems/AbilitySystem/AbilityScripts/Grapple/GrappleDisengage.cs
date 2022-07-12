using System;
using UnityEngine;
[CreateAssetMenu]
public class GrappleDisengage : Ability
{
    PhysicsBody body;

    //public event Action<float> DisengageGrapple;
    public override void Activate(GameObject parent)
    {
        body = parent.GetComponent<PhysicsBody>();
        
        // reset ability state
        body.GrappleSelectionComplete = false;
        body.DisableGravity = false;
        body.points.Clear();
    }

    public override void Deactivate(GameObject parent)
    {
        //cooldown for grapple
        //DisengageGrapple?.Invoke(CooldownTime);
    }
}

