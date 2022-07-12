using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomisedSound : MonoBehaviour
{
    public AntiOverlapAudioClip audioClipArray;

    void Start()
    {
        audioClipArray.Initialise();

    }

    void Update()
    {
        audioClipArray.Play(this.transform);
    }
}

