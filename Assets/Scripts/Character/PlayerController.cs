using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public delegate void PlayerDelegate(Walking player);
    //public event PlayerDelegate switchPlayers;

    public GameObject playerGreen;
    public GameObject playerRed;

    public Creature currentPlayer;
    public FollowCam cam;

    public bool red = false;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        

        //надо привязать чуть получше. Ккто тупо.
        //currentPlayer.health.died += SwitchPlayers;
        //currentPlayer.GetComponent<Health>().died += SwitchPlayers;
    }

    // Update is called once per frame
    void Update()
    {

        currentPlayer.walking.GetMoveX(Input.GetAxis("Horizontal"));

        if (Input.GetButtonDown("Jump"))
            currentPlayer.walking.Jump();

        if (Input.GetButtonUp("Jump"))
            currentPlayer.walking.BreakJump();

        if (Input.GetButtonDown("Fire3"))
            currentPlayer.weapon.Strike( Input.GetAxis("Vertical"));


        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            SwitchPlayers();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            //SceneManager.LoadScene(2);
            if (PauseMenu.instance.shown)
                PauseMenu.instance.HidePauseMenu();
            else
                PauseMenu.instance.ShowPauseMenu();
        }


    }

    //MAKE IT STATIC
    //может и не статик, но тут еще есть говнокод который надо разобрать
    public void SwitchPlayers()
    {
        return;// отказ от фичи


        red = !red;
        playerGreen.SetActive(!red);
        playerRed.SetActive(red);

        currentPlayer = red ? playerRed.GetComponent<Creature>() : playerGreen.GetComponent<Creature>();

        cam.AssignCreature((red) ? playerRed : playerGreen);
        cam.yValue = (red) ? 10.5f : -10.5f;
        //cam.GetComponent<Camera>().backgroundColor = (red) ? new Color(41, 25, 0) : new Color(41, 25, 0);
        cam.PlayerPosition();


        
        //switchPlayers?.Invoke(currentPlayer);
    }

    

}
