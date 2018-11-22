using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOnContact : MonoBehaviour
{
    public delegate void HitOnContactDelegate();
    public event HitOnContactDelegate hit;
    public List<string> hitTags = new List<string> { "Enemy" };

    private void OnTriggerStay2D(Collider2D collision)
    {
        foreach (string tag in hitTags)
        {
            if (collision.tag == tag)
            {
                collision.GetComponent<Health>().RecieveDamage(1, transform);
                hit?.Invoke();
                break;
            }
        }
    }
}
