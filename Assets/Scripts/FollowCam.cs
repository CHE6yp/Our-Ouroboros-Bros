using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform cam;
    public GameObject playerCreature;
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
        AssignCreature(playerCreature);
    }

    // Update is called once per frame
    void Update()
    {
        if (camBound && camBound.bound)
            return;

        PlayerPosition();
    }

    public void PlayerPosition()
    {
        this.transform.position = new Vector3(playerCreature.transform.position.x+shakeX, yValue+shakeY, -10);
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

        //events
        Health health = creature.GetComponent<Health>();
        if (health)
        {
            health.damaged += ScreenShake;
        }

    }
}
