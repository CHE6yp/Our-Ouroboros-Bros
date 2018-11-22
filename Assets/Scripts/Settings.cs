using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{

    public AudioMixer audioMixer;

    public void MasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(volume)*20);
    }

    public void MusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
    }

    public void EffectsVolume(float volume)
    {
        audioMixer.SetFloat("effectsVolume", Mathf.Log10(volume) * 20);
    }

}
