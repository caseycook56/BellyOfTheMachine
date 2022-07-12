using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AntiOverlapAudioClip", menuName = "Anti-Overlap AudioClip")]


public class AntiOverlapAudioClip : ScriptableObject
{
    public AudioClip[] audioClipArray;

    public bool enableRandomSpacing = false; // By Default disabled.
    public float minRandomRange = 0;
    public float maxRandomRange = 0;
    public bool enableStartDelay = false;
    public float startDelayTimer = 0;

    public GameObject audioClipSettings;


    private bool firstPlay;
    private float lastPlayedTime;
    private float randomTimeBuffer;
    private int lastPlayedSample = 0;
    private bool startDelay;


    public void Initialise()
    {
        lastPlayedTime = 0;
        startDelay = enableStartDelay;
        firstPlay = false;
    }

    public void Play(Transform transform)
    {
        if (!IsPlaying())
        {
            PlayPrefabClipAtPoint(transform.position);
            lastPlayedTime = Time.realtimeSinceStartup;
            if (enableRandomSpacing)
            {
                randomTimeBuffer = Random.Range(minRandomRange, maxRandomRange);
            }
        }
    }

    public bool IsPlaying()
    {
        if (!firstPlay)
        {
            if(startDelay)
            {
                if(Time.realtimeSinceStartup > startDelayTimer)
                {
                    firstPlay = true;
                    return false;
                }
                return true;
            }
            if (!startDelay)
            {
                firstPlay = true;
                return false;
            }
        }
        if (Time.realtimeSinceStartup - lastPlayedTime < audioClipArray[lastPlayedSample].length + randomTimeBuffer && firstPlay)
        {

            return true;
        }

        return false;
    }

    // Unused
    public void PlayClipAtPoint(AudioClip clip, Vector3 position)
    {
        GameObject go = new GameObject("PlayandForget");
        go.transform.position = position;
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.clip = clip;
        Destroy(go, clip.length);
        audioSource.Play();
    }

    public void PlayPrefabClipAtPoint(Vector3 position)
    {
        if (!IsPlaying())
        {
            lastPlayedSample = Random.Range(0, audioClipArray.Length);
            GameObject go = Instantiate(audioClipSettings, position, Quaternion.identity);
            AudioSource audio = go.GetComponent<AudioSource>();
            audio.name = audio.name + " " + audio.clip;
            audio.clip = audioClipArray[lastPlayedSample];
            Destroy(go, audio.clip.length);
            audio.Play();
        }
    }
}