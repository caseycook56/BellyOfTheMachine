using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloadScene : MonoBehaviour
{
    public int Scene;
    private bool Unloaded;
    private void OnTriggerEnter(Collider other)
    {
        if(!Unloaded)
        {
            Unloaded = true;
            SceneController.instance.UnloadScene(Scene);
        }
    }
}
