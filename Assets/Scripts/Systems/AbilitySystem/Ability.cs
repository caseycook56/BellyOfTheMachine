using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public new string name;
    public float CooldownTime;
    public float ActiveTime;

    // Events
    public virtual void Activate(GameObject parent) { }
    public virtual void Deactivate(GameObject parent) { }
}
