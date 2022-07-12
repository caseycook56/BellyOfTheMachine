using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateDoors : MonoBehaviour
{
    private Animator animator;
    public bool isOpen;
    [Range(5f, 25f)]
    public float interval;
    private float last_time;

    void Start()
    {
        animator = GetComponent<Animator>();
        isOpen = false;
        last_time = Time.realtimeSinceStartup;
        animator.SetTrigger("Close");
    }
    void FixedUpdate()
    {
        if (Time.realtimeSinceStartup - last_time > interval)
        {
            last_time = Time.realtimeSinceStartup;
            if (isOpen)
            {
                animator.SetTrigger("Close");
                isOpen = false;
            }
            else
            {
                animator.SetTrigger("Open");
                isOpen = true;
            }
        }
    }
}
