﻿using System.Collections;
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

    public LayerMask enemyMask;
    Transform mTrans;
    float mWidth, mHeight;

    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        rBody = this.GetComponent<Rigidbody2D>();
        mTrans = this.transform;
        SpriteRenderer Sprite = this.GetComponent<SpriteRenderer>();
        mWidth = Sprite.bounds.extents.x;
        mHeight = Sprite.bounds.extents.y;
    }

    void FixedUpdate()
    {
        Vector2 lineCastPos = mTrans.position.toVector2() - mTrans.right.toVector2() * mWidth + Vector2.up * mHeight;
        Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
        bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);
        Debug.DrawLine(lineCastPos, lineCastPos - mTrans.right.toVector2() * .1f);
        bool isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - mTrans.right.toVector2() * .1f, enemyMask);

        //Разворот, если нет пола или впереди блок
        if (!isGrounded || isBlocked)
        {
            //speed *= -1;
            Vector2 curRot = mTrans.eulerAngles;
            curRot.y += 180 * Time.deltaTime;
            mTrans.eulerAngles = curRot;
            /*
            Vector2 curScale = mTrans.localScale;
            curScale.x -= 1 * Time.deltaTime;
            mTrans.localScale = curScale;
            */
        }

        //движение справа налево
        Vector2 mVelocity = rBody.velocity;
        mVelocity.x = -speed;
        rBody.velocity = mVelocity;

        /*
        dist = Vector2.Distance(player.transform.position, transform.position);
        rBody.velocity = new Vector2(speed * direction, rBody.velocity.y);
        transform.localScale = new Vector3(direction, 1, 1);
                
        if (dist <= pos)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);         
        }
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
            direction *= -1f;*/
    }
}
