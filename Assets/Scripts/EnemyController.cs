using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PhysicsObject
{
    public GameObject player;
    public float speed = 2;
    public float dist = 0;
    public float pos = 4;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");        
    }

    void Update()
    {
        dist = Vector2.Distance(player.transform.position, transform.position);

        if (dist <= pos)
        {
            if (dist <= 1)
            {
                transform.position = player.transform.position;
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }            
        }
    }
}
