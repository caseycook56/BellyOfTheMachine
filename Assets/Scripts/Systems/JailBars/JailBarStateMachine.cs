using System;
using UnityEngine;

public class JailBarStateMachine : MonoBehaviour
{
    private AudioSource audioSource;
    private bool isActive = true;

    private StateMachine _stateMachine;
    private int DPS = 100;
    private int max_seconds_damage = 25;

    private void Awake()
    {
        var animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        _stateMachine = new StateMachine();

        IState active = new Active(this, animator, audioSource);
        IState deactivate = new Deactivate(this, animator, audioSource);

        At(active, deactivate, () => !ActiveState());

        _stateMachine.SetState(active);
        
        void At(IState from, IState to, Func<bool> condition) => _stateMachine.AddTransition(from, to, condition);
    }

    private bool ActiveState()
    {
        return isActive;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isActive)
        {
            if (collision.gameObject.tag == "ShortCircuit")
            {
                GetComponent<BoxCollider>().enabled = false;

                isActive = false;
            }
            else
            {
                if (collision.gameObject.TryGetComponent(out Health h))
                {
                    if (h != null)
                    {
                        h.TakeDamage(gameObject, DPS, max_seconds_damage);
                    }
                }
            }
        }
    }

    void Update()
    {
        _stateMachine.Tick();
    }
}
