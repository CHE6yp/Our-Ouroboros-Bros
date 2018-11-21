using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOnContact : MonoBehaviour
{
    public delegate void HitOnContactDelegate();
    public event HitOnContactDelegate hit;
    public bool active;
    public List<string> hitTags = new List<string> { "Enemy" };

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (active)
        {
            foreach (string tag in hitTags)
            {
                if (collision.tag == tag)
                {
                    collision.GetComponent<Health>().RecieveDamage(1);
                    hit?.Invoke();
                    break;
                }
            }
        }
    }
}
