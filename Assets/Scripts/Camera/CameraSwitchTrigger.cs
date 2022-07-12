using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitchTrigger : MonoBehaviour
{
    private float last_time;
    private bool b_has_triggered;
    void Start()
    {
        last_time = Time.realtimeSinceStartup;
        b_has_triggered = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        float cooldown = 1f;
        if (other.tag == "main_player")
        {
            Debug.Log("Camera Switch Trigger");
            if (Time.realtimeSinceStartup - last_time > cooldown)
            {
                if (!b_has_triggered)
                {
                    FindObjectOfType<VirtualCameraManager>().UpdateCameraState(VirtualCameraManager.CameraState.DollyB);
                }
                else
                {
                    FindObjectOfType<VirtualCameraManager>().UpdateCameraState(VirtualCameraManager.CameraState.DollyA);
                }
                b_has_triggered = !b_has_triggered;
                last_time = Time.realtimeSinceStartup;
            }
        }
    }
}
