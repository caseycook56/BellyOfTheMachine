using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsOptions : MonoBehaviour
{

    public void Low()
    {
        Debug.Log("Graphics Quality: Low!");
        QualitySettings.SetQualityLevel(1, true);
    }

    public void High()
    {
        Debug.Log("Graphics Quality: High!");
        QualitySettings.SetQualityLevel(0, true);
    }
}
