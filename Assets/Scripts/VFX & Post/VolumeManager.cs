using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class VolumeManager : MonoBehaviour
{
    private Vignette vignette;
    private ChromaticAberration ca;

    public Volume SceneLocalVolume;
    private void Awake()
    {
        Debug.Log(SceneLocalVolume);

        for (int i = 0; i < SceneLocalVolume.profile.components.Count; i++)
        {
            if (SceneLocalVolume.profile.components[i].name == "ChromaticAberration(Clone)")
            {
                Debug.Log("CA");
                ca = (ChromaticAberration)SceneLocalVolume.profile.components[i];
            }

            if (SceneLocalVolume.profile.components[i].name == "Vignette(Clone)")
            {
                Debug.Log("Vignette");
                vignette = (Vignette)SceneLocalVolume.profile.components[i];
            }
        }
    }
    private void Start()
    {

    }
    public void SetVignette(float value)
    {
        vignette.intensity.Override(value);
    }
    public void SetChromaticAberration(float value)
    {
        ca.intensity.Override(value);
    }
}
