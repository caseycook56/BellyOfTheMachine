using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Active : IState
{
    private readonly JailBarStateMachine _jailbarStateMachine;
    private readonly Animator _animator;
    private readonly AudioSource _audioSource;

    public Active(JailBarStateMachine jailBarStateMachine, Animator animator, AudioSource audioSource)
    {
        _jailbarStateMachine = jailBarStateMachine;
        _animator = animator;
        _audioSource = audioSource;
    }

    public void OnEnter()
    {
        _animator.SetBool("Active", true);
        _audioSource.Play();
    }

    public void Tick()
    {

    }

    public void OnExit()
    {

    }
}
