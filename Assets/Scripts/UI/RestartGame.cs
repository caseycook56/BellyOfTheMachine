using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public static event Action Restart;
    public void Start()
    {
        Button b = GetComponent<Button>();
        b.onClick.AddListener(GoToStartScreen);
    }
    public void GoToStartScreen()
    {
        Restart?.Invoke();
    }
}
