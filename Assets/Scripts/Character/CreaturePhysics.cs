using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaturePhysics : MonoBehaviour
{

    public float minGroundNormalY = 0f;
    public float gravityModifier = 5f;

    public float speedY = 100f;

    protected Vector2 targetVelocity;
    public bool grounded;

    public Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    public float maxVelocityY = 3;
    public Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    public bool jumping;


    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        //Debug.Log(gameObject.layer);
        contactFilter.useLayerMask = true;
    }

    void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    void FixedUpdate()
    {
        if (!jumping)
        {
            velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
            if (velocity.y < -speedY * Time.deltaTime)
                velocity = new Vector2(0, -speedY * Time.deltaTime);
            //velocity = new Vector2(0, -speedY * Time.deltaTime);
        }

        velocity.x = targetVelocity.x;
        //if (Mathf.Abs(velocity.y) > maxVelocityY )

        grounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                // TODO блядь какое же это сранное говно, это условие, нахуй понять как правильно и сделать правильно
                // а вообще это дерьмо тут чтобы враг мог нормально проходить через игрока. Без условия он останавливался, хотя в слоях я прописал им чтобы они не сталкивались
                // разобраться со слоями короче надо
                //if (hitBuffer[i].collider.tag != "Player")
                    hitBufferList.Add(hitBuffer[i]);
                //Debug.Log(hitBuffer[i].collider.name);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }


        }
        //if (gameObject.tag == "Player")
            //Debug.Log(velocity);
        //if (Mathf.Abs(velocity.y) > maxVelocityY)
            //velocity = new Vector2(velocity.x, maxVelocityY * velocity.normalized.y);
        //if (gameObject.tag == "Player")
            //Debug.Log("After "+velocity);

        rb2d.position = rb2d.position + move.normalized * distance;
    }

}