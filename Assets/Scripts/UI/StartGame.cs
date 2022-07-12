using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public static event Action StartButton;

    // Start is called before the first frame update
    void Start()
    {

        Button b = GetComponent<Button>();
        b.onClick.AddListener(Game);
        

    }
    public void Game()
    {
        StartButton?.Invoke();

    }

}
