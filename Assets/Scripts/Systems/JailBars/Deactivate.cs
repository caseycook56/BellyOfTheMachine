using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Deactivate : IState
{
    private readonly JailBarStateMachine _jailbarStateMachine;
    private readonly Animator _animator;
    private readonly AudioSource _audioSource;
    
    public Deactivate(JailBarStateMachine jailBarStateMachine, Animator animator, AudioSource audioSource)
    {
        _jailbarStateMachine = jailBarStateMachine;
        _animator = animator;
        _audioSource = audioSource;
    }

    public void OnEnter()
    {
        _animator.SetBool("Active", false);
        _audioSource.Stop();
        
        Debug.Log("Disable Cell!");
        // disable Spark particle systems
        GameObject[] sparks = GameObject.FindGameObjectsWithTag("JailCellParticlesOn");
        foreach (GameObject go in sparks)
        {
            go.GetComponent<VisualEffect>().enabled = false;
        }
    }

    public void Tick()
    {

    }

    public void OnExit()
    {

    }
}
