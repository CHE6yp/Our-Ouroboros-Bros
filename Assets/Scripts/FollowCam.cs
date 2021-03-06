﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{

    public delegate void CamDelegate();
    public CamDelegate camPosition;
    public Transform cam;
    public GameObject playerCreature;
    public PlayerController playerController;
    public Walking walkingPlayer;
    public CamBound camBound;
    public float yValue = 0;

    public float smoothSpeed = 0.2f;
    public float shakeMin = 0.5f;
    public float shakeMax = 1;
    public float shakeInterval = 0.02f;
    float shakeX=0;
    float shakeY=0;


    private void Awake()
    {
        //Есть баг! если начинать с PlayerPosition2 пропадают партиклы на существах
        camPosition = PlayerPosition2;

    }

    void Start()
    {
        AssignCreature(playerCreature);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (camBound && camBound.bound)
            return;

        camPosition();
    }

    public void PlayerPosition()
    {
        this.transform.position = new Vector3(playerCreature.transform.position.x+shakeX, yValue+shakeY, -10);
    }

    public void PlayerPosition2()
    {
        float rx = Input.GetAxis("HorizontalRight");
        float ry = Input.GetAxis("VerticalRight");
        float multiplier = 2;


        Vector3 desiredPosition = new Vector3(playerCreature.transform.position.x+rx* multiplier, playerCreature.transform.position.y+ry* multiplier, -10);
        //desiredPosition += horSpace;
        Vector3 lerpedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        transform.position = new Vector3(lerpedPosition.x + shakeX, lerpedPosition.y + shakeY, -10);
    }

    public void ChangeCamPosition()
    {
        if (camPosition == PlayerPosition)
            camPosition = PlayerPosition2;
        else if (camPosition == PlayerPosition2 && cam.GetComponent<Camera>().orthographicSize == 10)
            cam.GetComponent<Camera>().orthographicSize = 5;
        else
        {
            cam.GetComponent<Camera>().orthographicSize = 10;
            camPosition = PlayerPosition;
        }
            
    }

    public void ScreenShake()
    {
        StartCoroutine(ScreenShakeIEnum());
    }

    public IEnumerator ScreenShakeIEnum()
    {
        for (int i = 0; i < 6; i++)
        {
            shakeX = Random.Range(shakeMin, shakeMax);
            shakeY = Random.Range(shakeMin, shakeMax);

            shakeX = (Random.value > 0.5f) ? shakeX : -shakeX;
            shakeY = (Random.value > 0.5f) ? shakeY : -shakeY;

            yield return new WaitForSeconds(shakeInterval);
        }
        shakeX = 0;
        shakeY = 0;

    }

    public void AssignCreature(GameObject creature)
    {
        playerCreature = creature;
        walkingPlayer = creature.GetComponent<Walking>();

        //events
        Health health = creature.GetComponent<Health>();
        if (health)
        {
            health.damaged += ScreenShake;
        }

    }
}
