using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitOnContact : MonoBehaviour
{
    public delegate void HitOnContactDelegate();
    public bool active = true;
    public event HitOnContactDelegate hit;
    public List<string> hitTags = new List<string> { "Enemy" };
    public bool fromCreature;

    private void Awake()
    {
        Health health = GetComponentInParent<Health>();
        if (health)
            health.died += MakeInactive;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!active)
            return;

        foreach (string tag in hitTags)
        {
            if (collision.tag == tag)
            {
                if (fromCreature)
                    collision.GetComponent<Health>().RecieveDamage(1, GetComponentInParent<Creature>().transform);
                else
                    collision.GetComponent<Health>().RecieveDamage(1, transform);
                hit?.Invoke();
                break;
            }
        }
    }

    void MakeInactive()
    {
        active = false;
    }
}
