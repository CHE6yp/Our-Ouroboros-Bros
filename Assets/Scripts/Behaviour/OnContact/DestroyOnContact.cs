using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    public bool dieHealth;
    public List<string> hitTags = new List<string> { "Player" };

    private void OnTriggerStay2D(Collider2D collision)
    {
        foreach (string tag in hitTags)
        {
            if (collision.tag == tag)
            {
                StartCoroutine(GetComponent<Health>().Die());
                break;
            }
        }
    }


}
