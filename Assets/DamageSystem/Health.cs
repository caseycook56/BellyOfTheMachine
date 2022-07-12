using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Health Events
    public static event Action GameOver;
    
    public float MaxHealth;
    private float CurrentHealth;

    private void Awake()
    {
        SceneController.ResetGame += ResetHealth;
        CurrentHealth = MaxHealth;
        Debug.Log("Health Awake: " + CurrentHealth);
    }

    private void ResetHealth()
    {
        Debug.Log("Reset Health");
        CurrentHealth = MaxHealth;
    }

    public float GetHealth()
    {
        return CurrentHealth;
    }

    public void TakeDamage (GameObject inflictor, int dps, int maxseconds)
    {
        if (!Dead())
        {
            print("Damage taken:" + dps + " from: " + inflictor + ", new health: " + CurrentHealth);
            StartCoroutine(DamgePerSecond(dps, maxseconds));
        }
    }

    private bool Dead()
    {
        return CurrentHealth <= 0;
    }

    private void isDead()
    {
        // assume this code will only be on the player
        if (CurrentHealth <= 0)
        {
            GameOver?.Invoke();
        }        
    }
    private void FixedUpdate()
    {
        if (CurrentHealth < MaxHealth)
        {
            CurrentHealth += .1f;
        }
    }
    private IEnumerator DamgePerSecond(int dps, int maxseconds)
    {
       for(int currentSeconds = 0; currentSeconds < maxseconds; currentSeconds++)
        {
            CurrentHealth -= dps;
            isDead();
            yield return new WaitForSeconds(1.0f);
        }
    }
}
