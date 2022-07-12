using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quit : MonoBehaviour
{
    public void Start()
    {
        Button b = GetComponent<Button>();
        b.onClick.AddListener(QuitGame);

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
