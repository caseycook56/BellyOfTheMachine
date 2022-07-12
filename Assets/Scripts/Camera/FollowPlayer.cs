using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowPlayer : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    private void Awake()
    {
        SceneController.SceneChange += OnSceneChange;
        vcam = GetComponent<CinemachineVirtualCamera>();
    }
    void Start()
    {
        if (vcam == null) vcam = GetComponent<CinemachineVirtualCamera>();
        vcam.Follow = GameObject.Find("Player").transform;
        //vcam.enabled = false;
    }
    public void OnSceneChange()
    {
        Debug.Log("NEWSCENE!");
        vcam.enabled = true;
    }
}
