using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Walking : PhysicsObject
{
    public delegate void WalkingEvent();
    public event WalkingEvent switchDirection;
    public event WalkingEvent jump;

    public float maxSpeed = 15;
    public float jumpTakeOffSpeed = 15;

    bool lookLeft;
    public Vector2 move;

    void Awake()
    {
        //
        Health health = GetComponent<Health>();
        if (health)
        {
            health.damaged += KnockBack;
        }
    }

    protected override void ComputeVelocity()
    {
        //Эта строчка тут была когда писал Виталя. Когда я переписывал она стала мешать, 
        //но я не уверен до конца что она не нужна совсем.
        //move = Vector2.zero;

        if ((lookLeft && (move.x > 0.01f)) || !lookLeft && (move.x < -0.01f))
            SwitchDirection();

        targetVelocity = move * maxSpeed / 2;
    }

    public void GetMoveX(float x)
    {
        move.x = x;
    }

    public void Jump()
    {
        if (grounded)
        {
            velocity.y = jumpTakeOffSpeed;
            jump?.Invoke();
        }
    }

    //пока что не нашел решения лучше, чем сделать второй метод
    //для прыжков в воздухе. Будет идея лучше - перепишу.
    public void AirJump()
    { 
        velocity.y = jumpTakeOffSpeed;
        jump?.Invoke();
    }

    //позволяет прыгать ниже
    public void BreakJump()
    {
        if (velocity.y > 0)
            velocity.y = velocity.y * 0.5f;
    }


    public void KnockBack()
    {
        velocity.y = jumpTakeOffSpeed * 0.5f;
        velocity.x = (lookLeft) ? 200 : -200;

    }

    void SwitchDirection()
    {
        lookLeft = !lookLeft;
        switchDirection?.Invoke();

        //meleeHit.transform.localScale = new Vector3(meleeHit.transform.localScale.x * -1, meleeHit.transform.localScale.y, meleeHit.transform.localScale.z);
    }
}
