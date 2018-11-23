using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{

    public AudioMixer audioMixer;

    private void Awake()
    {
        audioMixer.SetFloat("musicVolume", PlayerPrefs.GetFloat("musicVolume"));
        audioMixer.SetFloat("effectsVolume", PlayerPrefs.GetFloat("effectsVolume"));
    }

    public void MasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(volume)*20);
    }

    public void MusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
    }

    public void EffectsVolume(float volume)
    {
        audioMixer.SetFloat("effectsVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("effectsVolume", Mathf.Log10(volume) * 20);
    }

}
