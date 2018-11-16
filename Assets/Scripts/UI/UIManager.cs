using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Text healthText;

    public void Awake()
    {
        instance = this;
    }


    public void DrawHealth(int health)
    {
        string hString = "";
        for (int i = 0; i < health; i++)
        {
            hString += "0 ";
        }
        healthText.text = hString;
    }
}
