using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public delegate void PlayerDelegate();
    public static event PlayerDelegate addCoin;
    public bool controllingCharacter = true;
    public static int coins = 0;

    public GameObject playerGreen;
    public GameObject playerRed;

    public Creature currentPlayer;
    public FollowCam cam;

    

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        controllingCharacter = true;
        coins = 0;
        Time.timeScale = 1;


        //надо привязать чуть получше. Ккто тупо.
        //currentPlayer.health.died += SwitchPlayers;
        //currentPlayer.GetComponent<Health>().died += SwitchPlayers;
    }

    // Update is called once per frame
    void Update()
    {

        if (controllingCharacter)
        {
            currentPlayer.walking.GetMoveX(Input.GetAxis("Horizontal"));

            if (Input.GetButtonDown("Jump"))
                currentPlayer.walking.Jump();

            if (Input.GetButtonUp("Jump"))
                currentPlayer.walking.BreakJump();

            if (Input.GetButtonDown("Fire3"))
                currentPlayer.weapon.Strike(Input.GetAxis("Vertical"));


            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton6))
            {
                //SceneManager.LoadScene(2);
                if (!PauseMenu.instance.shown)
                {
                    controllingCharacter = false;
                    PauseMenu.instance.ShowPauseMenu();
                }
            }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton6))
            {
                //SceneManager.LoadScene(2);
                if (PauseMenu.instance.shown)
                {
                    PauseMenu.instance.HidePauseMenu();
                    controllingCharacter = true;
                }
            }
        }
    }


    public static void AddCoin()
    {
        coins++;
        addCoin?.Invoke();
    }
    

}
