using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PhysicsObject
{

    public float maxSpeed = 15;
    public float jumpTakeOffSpeed = 15;

    public SpriteRenderer spriteRenderer;
    private Animator animator;

    public int health = 10;

    public float timeSpan = 10;
    public float time = 0;
    public bool goRight = false;

    //3 трансформа потому что это УЕБИЩЕ не хочет блядь нормально позицию менять я не знаю почему. Так то должен 1 быть.
    public Transform groundDetectionCurrent;
    public Transform groundDetection1;
    public Transform groundDetection2;
    public float groundDetectionDistance = 1;
    //Та же ъхуйня
    public Transform obstacleDetectionCurrent;
    public Transform obstacleDetection1;
    public Transform obstacleDetection2;
    public float obstacleDetectionDistance = 0.3f;



    void Start()
    {
        //spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        groundDetectionCurrent = groundDetection1;
    }



    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = DetectGround();



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

    public float DetectGround()
    {

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetectionCurrent.position, Vector2.down, groundDetectionDistance);
        Debug.DrawRay(groundDetectionCurrent.position, Vector2.down, Color.blue);

        Vector2 obstacleCheckDirection = (goRight) ? Vector2.right : Vector2.left;
        RaycastHit2D obstacleInfo = Physics2D.Raycast(groundDetectionCurrent.position, obstacleCheckDirection, obstacleDetectionDistance);
        Debug.DrawRay(groundDetectionCurrent.position, obstacleCheckDirection, Color.blue);
        //Debug.Log(obstacleInfo.collider);

        //какой же говнокод
        //if (!groundInfo.collider || groundInfo.collider.tag == "Spike" || groundInfo.collider.isTrigger || (obstacleInfo.collider != null && obstacleInfo.collider.tag != "Player"))

        if (obstacleInfo.collider != null )
            Debug.Log(obstacleInfo.collider.tag);

        if (!groundInfo.collider || groundInfo.collider.tag == "Spike"  || (obstacleInfo.collider != null && (obstacleInfo.collider.tag != "Player" && obstacleInfo.collider.tag != "MeleeRange")))
        {
            Debug.Log("Turn");
            goRight = !goRight; 
            time = 0;
            groundDetectionCurrent = (groundDetectionCurrent == groundDetection1) ? groundDetection2 : groundDetection1;
        }
        return (goRight) ? 1 : -1;

    }
}
