using System;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    private AbilityHolder ability;
    private RawImage AbilityIcon;
    [SerializeField] private Texture GrappleReadyIcon;
    [SerializeField] private Texture GrappleActiveIcon;
    [SerializeField] private Texture GrappleCooldownIcon;
    private void Awake()
    {
        //DontDestroyOnLoad(this);
    }
    private void Start()
    {
        AbilityIcon = gameObject.GetComponentInChildren<RawImage>();
    }
    public void Bind(AbilityHolder _AbilityHolder)
    {
        Debug.Log("Bind!");
        ability = _AbilityHolder;

        ability.Active += HandleActive;
        ability.Ready += HandleReady;
        ability.Cooldown += HandleCooldown;
    }
    public void OnDestroy()
    {
        if (ability != null)
        {
            ability.Active -= HandleActive;
            ability.Ready -= HandleReady;
            ability.Cooldown -= HandleCooldown;
        }
    }
    public void HandleReady()
    {
        //Debug.Log("Ability Ready");
        AbilityIcon.texture = GrappleReadyIcon;
    }
    public void HandleActive()
    {
        //Debug.Log("Ability Active");
        AbilityIcon.texture = GrappleActiveIcon;
    }
    public void HandleCooldown()
    {
        //Debug.Log("Ability Cooldown");
        AbilityIcon.texture = GrappleCooldownIcon;
    }
}
