﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : PhysicsObject
{
    public Character character;
    public float maxSpeed = 15;
    public float jumpTakeOffSpeed = 15;

    public SpriteRenderer spriteRenderer;
    private Animator animator;

    AudioSource audioSource;
    public TextMesh debugText;
    bool lookLeft;

    public int health = 1;

    // Use this for initialization
    void Awake()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpTakeOffSpeed;
            audioSource.Play();
        }
        //позволяет прыгать ниже
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        }

        if ((lookLeft && (move.x > 0.01f)) || !lookLeft && (move.x < -0.01f))
            SwitchDirection();

        string flipDebugText = "lookLeft: " + lookLeft.ToString() + System.Environment.NewLine + "spriteRenderer.flipX: "+ spriteRenderer.flipX.ToString();
        DebugText(flipDebugText);

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed /2;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //PlayerController pc = new PlayerController(); //GetComponent<PlayerController>().SwitchPlayers();
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("You Died");
            PlayerController.instance.SwitchPlayers();
        }
    }

    void DebugText(string text)
    {
        debugText.text = text;
    }

    void SwitchDirection()
    {
        lookLeft = !lookLeft;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

}
