using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIController : MonoBehaviour
{
    // Events
    public static event Action<AbilityHolder> AddAbility;
    
    // Player UI Elements
    [SerializeField] private DrawLine line;
    [SerializeField] private Cursor cursor;

    // Ability System UI
    [SerializeField] private AbilityUI _AbilityUI;

    private void Awake()
    {
        RegisterCallbacks();
    }

    private void RegisterCallbacks()
    {
        Debug.Log("Register AddAbility");
        AddAbility += HandleAddAbility;
    }

    public void UnregisterCallbacks()
    {
        AddAbility -= HandleAddAbility;
    }

    private void OnDestroy()
    {
        AddAbility -= HandleAddAbility;
    }

    public void AddAbilities(GameObject Player)
    {
        AbilityHolder[] abilities = Player.GetComponents<AbilityHolder>();
        foreach (AbilityHolder ability in abilities)
        {
            Debug.Log("Register: " + ability);
            AddAbility?.Invoke(ability);
        }
    }

    private void HandleAddAbility(AbilityHolder ability)
    {
        _AbilityUI.Bind(ability);
    }
}
