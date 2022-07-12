using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    public AudioSource UIAudioSource;
    public AudioClip clickSound;
    public AudioClip hoverSound;

    public void clickButtonSound()
    {
        UIAudioSource.PlayOneShot(clickSound);
    }
    public void hoverButtonSound()
    {
        UIAudioSource.PlayOneShot(hoverSound, 0.5f);
    }
}
