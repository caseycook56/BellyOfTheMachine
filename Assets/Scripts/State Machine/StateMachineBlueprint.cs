using System;
using UnityEngine;

public class StateMachineBlueprint : MonoBehaviour
{
    private AudioSource audioSource;


    private StateMachine _stateMachine;

    private void Awake()
    {
        var animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        _stateMachine = new StateMachine();
        

        void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);

    }

    void Update()
    {
        _stateMachine.Tick();

    }
}
