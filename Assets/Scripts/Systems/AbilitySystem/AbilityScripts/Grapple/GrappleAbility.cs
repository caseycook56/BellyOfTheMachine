using System;
using UnityEngine;
[CreateAssetMenu]
public class GrappleAbility : Ability
{
    private TimeManager _TimeManager;
    PhysicsBody body;
    public override void Activate(GameObject parent)
    {
        // TODO
        _TimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        body = parent.GetComponent<PhysicsBody>();
        body.GrappleSelectionBegun = true;
        _TimeManager.BeginScaleTime(.7f, .5f);
    }

    public override void Deactivate(GameObject parent)
    {
        if (body.points.Count > 0)
        {
            body.GrappleSelectionComplete = true;
        }
    }
}