using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityAsleep : IState 
{
    private readonly SecurityStateMachine _SecurityDoorStateMachine;
    private readonly Animator _Animator;
    private readonly AudioSource _AudioSource;
    private AudioClip CloseSound;

    public SecurityAsleep(SecurityStateMachine FSM, Animator animator, AudioSource audio, AudioClip clip)
    {
        CloseSound = clip;
        _SecurityDoorStateMachine = FSM;
        _Animator = animator;
        _AudioSource = audio;
    }

    public void OnEnter()
    {
        LastTime = Time.realtimeSinceStartup;
        _Animator.SetTrigger("Close");
    }

    public void OnExit()
    {
        AnimationBegun = false;
    }

    private float LastTime;
    private float Interval = .42f;
    private bool AnimationBegun = false;
    void IState.Tick()
    {
        if (Time.realtimeSinceStartup - LastTime > Interval && !AnimationBegun)
        {
            //Debug.Log("ASLEEP!");
            _AudioSource.PlayOneShot(CloseSound);
            AnimationBegun = true;
        }
    }
}
