using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject player;
    public float speed = 2;
    public float dist = 0;
    public float pos = 4;
    public float direction = -1f;
    public Rigidbody2D rBody;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rBody = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        dist = Vector2.Distance(player.transform.position, transform.position);
        rBody.velocity = new Vector2(speed * direction, rBody.velocity.y);
        transform.localScale = new Vector3(direction, 1, 1);
        
        /*
        if (dist <= pos)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);         
        }
        */
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
            direction *= -1f;
    }
}
