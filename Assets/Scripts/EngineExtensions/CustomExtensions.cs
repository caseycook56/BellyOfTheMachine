using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomExtensions
{
    public static T GetOrAddComponent<T>(this GameObject targetGameObject, bool includeChildren = false) where T : Component
    {
        T existingComponent = includeChildren ? targetGameObject.GetComponentInChildren<T>() : targetGameObject.GetComponent<T>();
        if (existingComponent != null)
        {
            return existingComponent;
        }

        T component = targetGameObject.AddComponent<T>();

        return component;
    }

    public static Vector3 Round(this Vector3 v, int decimalPlaces = 2)
    {
        float multiplier = 1;
        for (int i = 0; i < decimalPlaces; i++)
        {
            multiplier *= 10f;
        }
        return new Vector3(
            Mathf.Round(v.x * multiplier) / multiplier,
            Mathf.Round(v.y * multiplier) / multiplier,
            Mathf.Round(v.z * multiplier) / multiplier);
    }
}
