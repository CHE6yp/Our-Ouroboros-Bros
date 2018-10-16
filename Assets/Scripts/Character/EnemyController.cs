using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PhysicsObject
{

    public float maxSpeed = 15;
    public float jumpTakeOffSpeed = 15;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public int health = 10;

    public float timeSpan = 10;
    public float time = 0;
    public bool goRight = false;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }



    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = DirectionTimed();



        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed / 2;
    }

    public float DirectionTimed()
    {
        time += Time.deltaTime;
        if (time >= timeSpan)
        {
            goRight = !goRight;
            time = 0;
        }
        return (goRight) ? 1 : -1;
    }
}
