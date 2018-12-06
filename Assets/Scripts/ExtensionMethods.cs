using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ExtensionMethods
{
    public static Vector2 toVector2 (this Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.y);
    }

    public static void TiltPitchFromOne(this AudioSource audioSource, float tilt)
    {
        audioSource.pitch = 1 + (Random.Range(-tilt, tilt));
    }

    public static bool In<T>(this T obj, params T[] args)
    {
        return args.Contains(obj);
    }

}
