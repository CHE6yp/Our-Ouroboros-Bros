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

    public static string ToDebugLogString(this int[][] matrix, string message)
    {
        //Выдаем лэйаут в консоль
        string layoutString = message+"\n";
        for (int y = 0; y < matrix.Length; y++)
        {
            layoutString += "|";
            for (int x = 0; x < matrix[y].Length; x++)
            {
                string element = matrix[y][x].ToString();
                element = (element.Length == 1) ? " " + element + "|" :  element + "|";
                layoutString += element;
            }
            layoutString += "\n";
        }
        return layoutString;
    }
}
