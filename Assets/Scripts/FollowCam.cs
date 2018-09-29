using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform cam;
    public Transform player;
    public CamBound camBound;
    public float yValue = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (camBound.bound)
            return;

        this.transform.position = new Vector3(player.position.x, yValue, -10);
    }
}
