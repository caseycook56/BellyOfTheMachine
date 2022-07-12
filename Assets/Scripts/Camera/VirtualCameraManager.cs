using UnityEngine;
using Cinemachine;
[RequireComponent(typeof(CinemachineBrain))]
public class VirtualCameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera DollyA;
    public CinemachineVirtualCamera DollyB;
    private Transform Target;
    public enum CameraState
    {   DollyA,
        DollyB
    }
    private void Awake()
    {
        UpdateCameraState(CameraState.DollyA);
    }

    private void Start()
    {
        Target = GameObject.Find("Player").transform;
    }

    public void UpdateCameraState(CameraState state)
    {
        Debug.Log("Update Camera State");
        if (state == CameraState.DollyA)
        {
            DollyA.Follow = Target;
            DollyA.Priority = 1;
            DollyB.Priority = 0;
        }
        else if (state == CameraState.DollyB)
        {
            DollyB.Follow = Target;
            DollyB.Priority = 1;
            DollyA.Priority = 0;
        }
    }
}
