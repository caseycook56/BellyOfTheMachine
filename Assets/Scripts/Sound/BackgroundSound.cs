using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
    public AudioClip[] backgroundSound;
    public Transform player; // Few ways this can be done, Possibly unnecessary

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        source.PlayOneShot(backgroundSound[Random.Range(0, backgroundSound.Length)]);
    }

    private void Update()
    {
        this.transform.position = player.position;
    }
}
