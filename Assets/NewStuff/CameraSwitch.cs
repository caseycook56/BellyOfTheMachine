using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject Vcam1;
    public GameObject Vcam2;
    public float holdTime;
    private bool Timer = false;


    public void Update()
    {
        if (Timer)
        {
            holdTime -= Time.deltaTime;
        }

        if (holdTime < 0){

            Vcam1.SetActive(true);
            Vcam2.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "main_player")
        {
            Timer = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            Vcam1.SetActive(false);
            Vcam2.SetActive(true);
        }
    }
}

