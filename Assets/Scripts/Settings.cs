using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class Settings : MonoBehaviour
{
    public static Settings instance;
    public AudioMixer audioMixer;

    public static Resolution[] resolutions;
 

    private void Awake()
    {

        /*
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        */

        GetResolutions();
        SetupGraphics();
        SetupAudio();
    }

    public void SetupGraphics()
    {
        bool fulScr = PlayerPrefs.GetInt("fullScreen") == 1 ? true : false;
        Screen.SetResolution(PlayerPrefs.GetInt("resW"), PlayerPrefs.GetInt("resH"), fulScr);
    }

    public void SetupAudio()
    {
        float musVol = PlayerPrefs.GetFloat("musicVolume");
        float effVol = PlayerPrefs.GetFloat("effectsVolume");
        audioMixer.SetFloat("musicVolume", Mathf.Log10(musVol) * 20);
        audioMixer.SetFloat("effectsVolume", Mathf.Log10(effVol) * 20);
    }

    public void MasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(volume)*20);
    }

    public void MusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume",  volume);
    }

    public void EffectsVolume(float volume)
    {
        audioMixer.SetFloat("effectsVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("effectsVolume", volume);
    }


    //graphics

    public void GetResolutions()
    {
        resolutions = Screen.resolutions;
    }

    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resW", res.width);
        PlayerPrefs.SetInt("resH", res.height);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        int saveFull = isFullscreen ? 1 : 0;
        PlayerPrefs.SetInt("fullScreen", saveFull);
    }

}
