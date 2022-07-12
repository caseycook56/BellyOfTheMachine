using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    private float CurrentTimeScale;

    public void BeginScaleTime(float scale, float period)
    {
        if (CurrentTimeScale == 1)
        {
            StartCoroutine(ScaleTime(scale, period));
        }
    }
    private IEnumerator ScaleTime(float scale, float period)
    {
        CurrentTimeScale = CurrentTimeScale < scale ? CurrentTimeScale : scale;
        Time.timeScale = CurrentTimeScale;

        yield return new WaitForSeconds(period);

        ResetTimeScale();
    }

    public void ResetTimeScale()
    {
        CurrentTimeScale = 1;
        Time.timeScale = 1;
    }
}
