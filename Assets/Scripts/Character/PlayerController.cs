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


    public Creature playerCharacter;
    public FollowCam cam;

    

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
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
            float x = Input.GetAxis("Horizontal");

            
            if (x > 0.5f)
                x = 1;
            else if (x < -0.5f)
                x = -1;
            else
                x = 0;
            playerCharacter.walking.GetMoveX(x);




            if (Input.GetButtonDown("Jump"))
                playerCharacter.walking.Jump();

            if (Input.GetButtonUp("Jump"))
                playerCharacter.walking.BreakJump();


            if (Input.GetButtonDown("Fire3"))
                playerCharacter.weapon.Strike(Input.GetAxis("Vertical"));

            //if (Input.GetButtonDown("Fire2"))
                //cam.ChangeCamPosition();


            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton6))
            {
                //SceneManager.LoadScene(2);
                if (!PauseMenu.instance.shown)
                {
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
