using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SecurityStateMachine : MonoBehaviour
{
    private StateMachine _stateMachine;
    private AudioSource audioSource;

    #region SecuritySettings
    
    [Range(5f, 10f)]
    public float Interval = 5f;
    private Vector3 hatch_offset;

    Animator animator;
    public VolumeManager vm;

    public AudioClip CloseSound;
    public AudioClip OpenSound;
    #endregion

    private void Awake()
    {
        hatch_offset = new Vector3(0, -.5f, 0);
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        _stateMachine = new StateMachine();
        
        IState Awake = new SecurityAwake(this, animator, audioSource, OpenSound, gameObject, vm);
        IState Asleep = new SecurityAsleep(this, animator, audioSource, CloseSound);

        At(Awake, Asleep, () => !IsActive());
        At(Asleep, Awake, () => IsActive());

        void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
        _stateMachine.SetState(Asleep);
    }

    private bool active = false;
    private float LastTime;
    private bool IsActive()
    {
        if (Time.realtimeSinceStartup - LastTime > Interval)
        {
            LastTime = Time.realtimeSinceStartup;
            active = !active;
        }
        return active;
    }

    void Update()
    {
        _stateMachine.Tick();
    }
}
