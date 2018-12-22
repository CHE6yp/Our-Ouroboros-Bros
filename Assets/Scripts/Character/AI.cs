using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AI : MonoBehaviour
{
    Creature creature;

    public bool goRight = false;
    public float timeSpan = 10;
    public float time = 0;

    //3 трансформа потому что это УЕБИЩЕ не хочет блядь нормально позицию менять я не знаю почему. Так то должен 1 быть.
    public Transform detectionPointCurrent;
    public Transform detectionPoint1;
    public Transform detectionPoint2;

    public float groundDetectionDistance = 1;
    public float obstacleDetectionDistance = 0.3f;

    // Start is called before the first frame update
    void Awake()
    {
        creature = GetComponent<Creature>();
        detectionPointCurrent = detectionPoint1;
    }

    // Update is called once per frame
    void Update()
    {
        if (creature.walking.grounded)
            creature.walking.GetMoveX(DetectGround());
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

        RaycastHit2D groundInfo = Physics2D.Raycast(detectionPointCurrent.position, Vector2.down, groundDetectionDistance);
        Debug.DrawRay(detectionPointCurrent.position, Vector2.down, Color.blue);

        Vector2 obstacleCheckDirection = (goRight) ? Vector2.right : Vector2.left;
        RaycastHit2D obstacleInfo = Physics2D.Raycast(detectionPointCurrent.position, obstacleCheckDirection, obstacleDetectionDistance);
        Debug.DrawRay(detectionPointCurrent.position, obstacleCheckDirection, Color.blue);


        //Хммм, не могу понять, говнокод ли это?
        //Выглядит как говнокод...
        if ((!groundInfo.collider || 
            groundInfo.collider.tag == "Spike" || 
            //groundInfo.collider.tag == "Player" ||
            (obstacleInfo.collider != null && 
            (obstacleInfo.collider.tag != "Player" && 
            obstacleInfo.collider.tag != "MeleeRange"))) &&
            creature.walking.grounded )
        {
            goRight = !goRight;
            time = 0;
            detectionPointCurrent = (detectionPointCurrent == detectionPoint1) ? detectionPoint2 : detectionPoint1;
        }
        return (goRight) ? 1 : -1;

    }
}
