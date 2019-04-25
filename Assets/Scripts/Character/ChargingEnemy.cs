using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChargingEnemy : AI
{
    //Creature creature;

    
    public float walkingDuration = 5;
    public float standingDuration = 2;
    public bool standing = false;
    public float playerDetectionDistance = 3;
    public float timer = 0;

    public override float DetectGround()
    {

        RaycastHit2D groundInfo = Physics2D.Raycast(detectionPointCurrent.position, Vector2.down, groundDetectionDistance);
        Debug.DrawRay(detectionPointCurrent.position, Vector2.down, Color.blue);

        Vector2 obstacleCheckDirection = (goRight) ? Vector2.right : Vector2.left;
        RaycastHit2D obstacleInfo = Physics2D.Raycast(detectionPointCurrent.position, obstacleCheckDirection, obstacleDetectionDistance);
        Debug.DrawRay(detectionPointCurrent.position, obstacleCheckDirection, Color.white);

        Vector2 playerDetection = (goRight) ? Vector2.right : Vector2.left;
        RaycastHit2D playerCheckInfo = Physics2D.Raycast(detectionPointCurrent.position, playerDetection, playerDetectionDistance);
        Debug.DrawRay(detectionPointCurrent.position+new Vector3(0,0.05f,0), playerDetection*playerDetectionDistance, Color.red);


        if (playerCheckInfo.collider != null && playerCheckInfo.collider.tag == "Player")
            Debug.Log("Player detected");



        timer += Time.deltaTime;
        if (!standing)
        {
            if (timer >= walkingDuration)
            {
                standing = true;
                timer = 0;
                return 0;
            }
            else
            {
                //Хммм, не могу понять, говнокод ли это?
                //Выглядит как говнокод...
                if ((!groundInfo.collider ||
                    groundInfo.collider.tag == "Spike" ||
                    //groundInfo.collider.tag == "Player" ||
                    (obstacleInfo.collider != null &&
                    (obstacleInfo.collider.tag != "Player" &&
                    obstacleInfo.collider.tag != "MeleeRange"))) &&
                    creature.walking.grounded)
                {
                    goRight = !goRight;
                    time = 0;
                    detectionPointCurrent = (detectionPointCurrent == detectionPoint1) ? detectionPoint2 : detectionPoint1;
                }
                return (goRight) ? 1 : -1;
            }

        }
        else
        {
            if (timer >= standingDuration)
            {
                standing = false;
                timer = 0;
                return (goRight) ? 1 : -1;
            }
            else
            {
                return 0;
            }

        }




        


        

    }
}
