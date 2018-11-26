using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    public bool shown;
    public GameObject pauseMenuPanel;

    public void Awake()
    {
        instance = this;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ShowPauseMenu()
    {
        Time.timeScale = 0;
        pauseMenuPanel.SetActive(true);
        shown = true;
    }

    public void HidePauseMenu()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1;
        shown = false;
    }
}
