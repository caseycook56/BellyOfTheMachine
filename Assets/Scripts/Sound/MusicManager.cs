using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public float waitTime;
    public GameObject startBGMPrefab;
    public GameObject fluidBGMPrefab;
    public GameObject ventBGMPrefab;
    public GameObject jailBGMPrefab;
    public GameObject deathBGMPrefab;
    private GameObject currentClip;


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Start")
        {
            NewFirstClip(startBGMPrefab);
        }
        if (scene.name == "Fluid")
        {
            // ChangeClip(fluidBGM);
            NewChangeClip(fluidBGMPrefab);
        }
        if (scene.name == "Vents")
        {
            // ChangeClip(ventBGM);
            NewChangeClip(ventBGMPrefab);
        }
        if (scene.name == "Jail")
        {
            // ChangeClip(jailBGM);
            NewChangeClip(jailBGMPrefab);
        }
        if (scene.name == "Death")
        {
            // ChangeClip(deathBGM);
            NewChangeClip(deathBGMPrefab);
        }
    }

    /*
    public void StopClip()
    {
        StartCoroutine(FadeOut());
    }
    
    public void ChangeClip(AudioClip bgm)
    {
        if(musicSource.clip != null)
        {
            StartCoroutine(FadeOut());
            StartCoroutine(FadeIn(bgm));
        } else
        {
            FirstFadeIn(bgm);
        }
    }

    IEnumerator FadeOut()
    {
        musicAnim.ResetTrigger("fadeIn");
        musicAnim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(waitTime);

    }

    IEnumerator FadeIn(AudioClip bgm)
    {
        yield return new WaitForSeconds(waitTime);
        musicAnim.ResetTrigger("fadeOut");
        musicAnim.SetTrigger("fadeIn");
        musicSource.Stop();
        musicSource.clip = bgm;
        musicSource.Play();
    }

    void FirstFadeIn(AudioClip bgm)
    {
        musicAnim.ResetTrigger("fadeOut");
        musicAnim.SetTrigger("fadeIn");
        musicSource.Stop();
        musicSource.clip = bgm;
        musicSource.Play();
    }

    */

    public void NewFadeOut()
    {
        currentClip.GetComponent<Animator>().SetTrigger("fadeOut");
    }


    public void NewChangeClip(GameObject startClip)
    {
        NewFadeOut();
        Destroy(currentClip, waitTime);
        currentClip = Instantiate(startClip);
    }
    
    public void NewFirstClip(GameObject firstClip)
    {
        currentClip = Instantiate(firstClip);
        
        Debug.Log(currentClip.GetComponent<AudioSource>().clip.name);
    }
}
