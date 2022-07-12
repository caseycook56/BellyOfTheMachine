using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityAwake : IState
{
    private readonly SecurityStateMachine _SecurityDoorStateMachine;
    private readonly Animator _Animator;
    private readonly AudioSource _AudioSource;
    private AudioClip OpenSound;
    private TimeManager _TimeManager;

    private Vector3 _EyePosition;
    Vector3 HatchOffset;
    private GameObject Target;
    private GameObject Parent;
    private VolumeManager _VolumeManager;
    public SecurityAwake(SecurityStateMachine FSM, Animator animator, AudioSource audio, AudioClip clip, GameObject parent, VolumeManager vm)
    {
        _VolumeManager = vm;
        _TimeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        OpenSound = clip;
        _SecurityDoorStateMachine = FSM;
        _Animator = animator;
        _AudioSource = audio;
        
        Parent = parent;
        HatchOffset = new Vector3(0, -.5f, 0);
        _EyePosition = parent.transform.position + HatchOffset;
    }

    void IState.OnEnter()
    {
        _Animator.SetTrigger("Open");
        _AudioSource.PlayOneShot(OpenSound);
        

        // TODO
        //GameManager gm = GameObject.Find("MasterUI").GetComponent<GameManager>();
        //Target = gm.GetPlayer();
        Target = GameObject.Find("Player");

        LastTime = Time.realtimeSinceStartup;
    }

    void IState.OnExit()
    {
        TimeOutOver = false;
    }

    private float LastTime;
    private float Interval = .5f;
    private bool TimeOutOver = false;
    void IState.Tick()
    {
        if (Time.realtimeSinceStartup - LastTime > Interval || TimeOutOver)
        {
            //Debug.Log("AWAKE");
            TimeOutOver = true;
            int dps = 1;
            float VisionRadius = 5f;
            if (Target != null)
            {
                Vector3 direction = Target.transform.position - _EyePosition;
                RaycastHit hit;
                Physics.Raycast(_EyePosition, direction, out hit, VisionRadius);
                if (hit.point != Vector3.zero)
                {
                    if (hit.rigidbody == Target.GetComponent<Rigidbody>())
                    {
                        _TimeManager.BeginScaleTime(.5f, 1f);
                        if (hit.collider.gameObject.TryGetComponent(out Health h))
                        {
                            if (h != null)
                            {
                                h.TakeDamage(Parent, dps, 1);
                                // assuming the door will only do damage to the player
                                // change the volume chromatic aberration for visual queue of damage
                                float value = 1f - ((float)h.GetHealth() / 100f);
                                _VolumeManager.SetChromaticAberration(value);
                                _VolumeManager.SetVignette(.6f);
                            }
                        }
                    }
                    else
                    {
                        // assuming the door will only do damage to the player
                        // change the volume chromatic aberration for visual queue of damage
                        Target.TryGetComponent(out Health h);
                        if (h != null)
                        {
                            float value = 1f - ((float)h.GetHealth() / 100f);
                            _VolumeManager.SetChromaticAberration(value);
                        }
                        _VolumeManager.SetVignette(.52f);
                    }
                }
            }
        }
    }
}
