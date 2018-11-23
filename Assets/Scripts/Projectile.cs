using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public Vector2 direction = new Vector2(0,0);
    public float speed = 2;

    public bool stopWhenDead = true;
    bool stop;

    // Start is called before the first frame update
    void Awake()
    {

        rigidbody2d = GetComponent<Rigidbody2D>();

        //взято из Walking
        Health health = GetComponent<Health>();
        if (health)
        {
            health.died += Stop;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody2d.velocity = direction*speed;
    }

    public void GetDirection(Vector2 dir)
    {
        direction = dir;
    }


    /// <summary>
    /// Такой же метод есть в Walking, нужно чето придумать.
    /// </summary>
    void Stop()
    {
        direction = Vector2.zero;
        stop = true;
    }
}
