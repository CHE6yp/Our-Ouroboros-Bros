using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectProjectilesOnContact : MonoBehaviour
{
    public Vector2 direction;
    public float acceleration = 2;
    public bool horizontal;


    private void Awake()
    {
        if (horizontal)
        {
            Walking walking = GetComponentInParent<Walking>();
            if (walking)
                walking.switchDirection += InvertDirection;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    { 
        if (collision.tag == "Projectile")
        {
            collision.GetComponent<Projectile>().GetDirection(direction*acceleration);
        }
    }

    void InvertDirection()
    {
        direction *= -1;
    }
}
