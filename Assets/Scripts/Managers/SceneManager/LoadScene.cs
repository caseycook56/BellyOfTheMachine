using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string CurrentScene;
    private bool Loaded;

    private void OnTriggerEnter(Collider other)
    {
        if (!Loaded)
        {
            SceneController.instance.LoadNextScene(CurrentScene);
            Loaded = true;
        }
    }
}
