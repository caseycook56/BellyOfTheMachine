using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityHolder : MonoBehaviour
{
    // Other classes can register to events for ability stages
    public event Action Ready;
    public event Action Active;
    public event Action Cooldown;

    private enum AbilityState
    {
        Ready,
        Active,
        Cooldown
    }

    public Ability ability;
    private AbilityState state = AbilityState.Ready;
    
    float CooldownTime;
    float ActiveTime;

    private void Update()
    {
        if (ability != null)
        {
            if (state == AbilityState.Active)
            {
                if (ActiveTime > 0)
                {
                    ActiveTime -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.Cooldown;
                    CooldownTime = ability.CooldownTime;
                    ability.Deactivate(gameObject);
                    Cooldown?.Invoke();
                }
            }
            else if (state == AbilityState.Cooldown)
            {
                if (CooldownTime > 0)
                {
                    CooldownTime -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.Ready;
                    Ready?.Invoke();
                }
            }
        }
    }
    public void Activate(InputAction.CallbackContext context)
    {
        if (ability != null)
        {
            if (context.phase == InputActionPhase.Started)
            {
                if (state == AbilityState.Ready)
                {
                    ability.Activate(gameObject);
                    ActiveTime = ability.ActiveTime;
                    state = AbilityState.Active;
                    Active?.Invoke();
                }
            }
        }
    }
}
