using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform cam;
    public Transform player;
    public CamBound camBound;
    public float yValue = 0;
    public float shakeMin = 0.5f;
    public float shakeMax = 1;
    public float shakeInterval = 0.02f;
    float shakeX=0;
    float shakeY=0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (camBound.bound)
            return;

        PlayerPosition();
    }

    public void PlayerPosition()
    {
        this.transform.position = new Vector3(player.position.x+shakeX, yValue+shakeY, -10);
    }

    public IEnumerator ScreenShake()
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
}
