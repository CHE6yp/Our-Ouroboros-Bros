using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{

    public static DeathMenu instance;
    public bool shown;
    public GameObject deathMenuPanel;


    public void Start()
    {
        instance = this;
        PlayerController.instance.playerCharacter.health.died += ShowDeathMenu;
    }


    public void ShowDeathMenu()
    {
        deathMenuPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }


}
