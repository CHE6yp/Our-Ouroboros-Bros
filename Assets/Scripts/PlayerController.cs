using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject playerGreen;
    public GameObject playerRed;
    public FollowCam cam;

    public bool red = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            SwitchPlayers();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            SceneManager.LoadScene(1);
        }


        // проверка геймпада
        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKeyDown("joystick 1 button " + i))
            {
                Debug.Log("joystick 1 button " + i);
            }
        }
    }

    public void SwitchPlayers()
    {
        red = !red;
        playerGreen.SetActive(!red);
        playerRed.SetActive(red);
        cam.player = (red) ? playerRed.transform : playerGreen.transform;
        cam.camBound = (red) ? playerRed.GetComponent<CamBound>() : playerGreen.GetComponent<CamBound>();
        cam.yValue = (red) ? 10.5f : 0;
        cam.PlayerPosition();
    }


}
