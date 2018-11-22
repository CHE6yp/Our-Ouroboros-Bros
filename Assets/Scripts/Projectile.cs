using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public Vector2 direction = new Vector2(0,0);
    public float speed = 2;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody2d.velocity = direction*speed;
    }
}
