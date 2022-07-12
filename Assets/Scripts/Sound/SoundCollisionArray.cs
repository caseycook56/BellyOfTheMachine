using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCollisionArray : MonoBehaviour
{
    public AntiOverlapAudioClip audioClip;

    void Start()
    {
        audioClip.Initialise();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            audioClip.Play(collision.transform);
        }
    }
}

