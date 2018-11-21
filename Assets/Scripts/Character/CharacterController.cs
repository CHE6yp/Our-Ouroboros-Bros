using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : PhysicsObject
{
    public float maxSpeed = 15;
    public float jumpTakeOffSpeed = 15;
    public SpriteRenderer spriteRenderer;
    private Animator animator;

    bool lookLeft;

    public ParticleSystem particleHit;

    public GameObject meleeHit;
    public MeleeTrigger meleeTrigger;
    public MeleeTrigger meleeTriggerDown;
    public MeleeTrigger meleeTriggerUp;

    


    float lastY = 0;



    void Awake()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
    }

    protected override void ComputeVelocity()
    {
        //Debug.Log("y - "+transform.position.y+"; acceleration - " + Mathf.Abs(transform.position.y - lastY));
        lastY = transform.position.y;

        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");
        //Debug.Log(Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("Jump") && grounded)
            Jump();

        if (Input.GetButtonUp("Jump"))
            BreakJump();
            

        if (Input.GetButtonDown("Fire3"))
            Strike();

        if (Input.GetKeyDown(KeyCode.X))
            Time.timeScale = (Time.timeScale == 1) ? 0.02f : 1;

        if ((lookLeft && (move.x > 0.01f)) || !lookLeft && (move.x < -0.01f))
            SwitchDirection();


        //animator.SetBool("grounded", grounded);
        //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed /2;
    }

    public void Jump()
    {
        velocity.y = jumpTakeOffSpeed;
        //event Jump()
    }

    //позволяет прыгать ниже
    public void BreakJump()
    {
        if (velocity.y > 0)
        {
            velocity.y = velocity.y * 0.5f;
        }
    }

    
    public void KnockBack()
    {
        velocity.y = jumpTakeOffSpeed*0.5f;

        velocity.x = (lookLeft) ? 200 : -200;
    }


    public void Strike()
    {
        Debug.Log(Input.GetAxis("Vertical"));

        //if (Input.GetAxis("Vertical")>0.55f)
           // StartCoroutine(meleeTriggerUp.Strike());
        //else if (!grounded && Input.GetAxis("Vertical")<-0.55f)
         //   StartCoroutine(meleeTriggerDown.Strike());
        //else
//StartCoroutine(meleeTrigger.Strike());
//StartCoroutine(meleeTrigger.Strike());
    }




    void SwitchDirection()
    {
        lookLeft = !lookLeft;
        spriteRenderer.flipX = !spriteRenderer.flipX;
        meleeHit.transform.localScale = new Vector3(meleeHit.transform.localScale.x * -1, meleeHit.transform.localScale.y, meleeHit.transform.localScale.z);
        //meleeHit.transform.position = meleeHit.transform.position * -1;
    }




}
