using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
//public class Walking : PhysicsObject
public class Walking : CreaturePhysics
{
    public delegate void WalkingEvent();
    public event WalkingEvent switchDirection;
    public event WalkingEvent jump;

    public float maxSpeed = 15;
    public float jumpTakeOffSpeed = 15;
    public bool stopWhenDead = true;
    bool stop;

    bool lookLeft;
    public Vector2 move;
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        //
        Health health = GetComponent<Health>();
        if (health)
        {
            health.damagedSource += KnockBack;
            health.died += Stop;
        }
    }

    protected override void ComputeVelocity()
    {
        if (stop)
            return;
        //Эта строчка тут была когда писал Виталя. Когда я переписывал она стала мешать, 
        //но я не уверен до конца что она не нужна совсем.
        //move = Vector2.zero;

        if ((lookLeft && (move.x > 0.01f)) || !lookLeft && (move.x < -0.01f))
            SwitchDirection();

        targetVelocity = move * maxSpeed / 2;

        animator.SetFloat("velocityY", velocity.y);
        animator.SetBool("grounded", grounded);

        if (move.x != 0 && grounded)
            animator.SetBool("running", true);
        else
            animator.SetBool("running", false);
    }

    public void GetMoveX(float x)
    {
        move.x = x;
    }

    public void Jump()
    {
        if (grounded)
        {
            jumping = true;
            velocity.y =  speedY*Time.fixedDeltaTime;
            StartCoroutine(JumpTime());
            jump?.Invoke();
            
        }
    }

    public IEnumerator JumpTime()
    {
        for (int x = 0; x < 15; x++)
        {
            yield return new WaitForFixedUpdate();
        }
        jumping = false;
    }


    //позволяет прыгать ниже
    public void BreakJump()
    {
        //if (velocity.y > 0)
        //{
        //    velocity.y = velocity.y * 0.5f;
        //    jumping = false;
        //}
        jumping = false;
    }

    /// <summary>
    /// Пока что не нашел решения лучше, чем сделать второй метод.
    /// Для прыжков в воздухе. Будет идея лучше - перепишу.
    /// </summary>
    public void AirJump()
    {
        jumping = true;
        velocity.y = speedY * Time.fixedDeltaTime;
        StartCoroutine(JumpTime());
        jump?.Invoke();
    }



    public void KnockBack(int damage, Transform source)
    {
        //velocity.y = jumpTakeOffSpeed * 0.5f;
        //GetMoveX( (lookLeft) ? 10 : -10);
        Vector2 dir = transform.position - source.position;
        dir.Normalize();
        Debug.Log(dir*3);
        //rb2d.MovePosition(dir);
        StartCoroutine(KnockBackIEnum(source));
    }

    //хуйня, но пока что должно работать
    public IEnumerator KnockBackIEnum(Transform source)
    {
        Vector2 dir = transform.position - source.position;
        dir.Normalize();
        velocity.y = jumpTakeOffSpeed * 0.2f;
        jumping = true;
        for (int i = 0; i < 3; i++)
        {
            //Debug.Log(rb2d.position + dir * Time.deltaTime);
            GetMoveX(dir.x*4);
            //velocity.y = 0.1f + dir.y*jumpTakeOffSpeed;
            yield return new WaitForSeconds(0.01f);
        }
        jumping = false;
        //Debug.Break();
    }

    void SwitchDirection()
    {
        lookLeft = !lookLeft;
        switchDirection?.Invoke();

        //meleeHit.transform.localScale = new Vector3(meleeHit.transform.localScale.x * -1, meleeHit.transform.localScale.y, meleeHit.transform.localScale.z);
    }

    void Stop()
    {
        targetVelocity = Vector2.zero;
        stop = true;
    }
}
