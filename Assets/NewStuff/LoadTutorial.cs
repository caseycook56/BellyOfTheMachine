using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LoadTutorial : MonoBehaviour
{

    public Canvas Tutorial;
    public Texture button;
    public VideoClip visual;
    public RenderTexture visualtexture;
    public string text; 
    // Start is called before the first frame update
    public void Load()
    {
        Canvas tut = Instantiate(Tutorial);
        tut.GetComponent<TutorialPointers>().button.texture = button;
        tut.GetComponent<TutorialPointers>().visual.clip = visual;
        tut.GetComponent<TutorialPointers>().visualtexture.texture = visualtexture;
        tut.GetComponent<TutorialPointers>().text.text = text;
        //could set time scale to 0.
    }
}
