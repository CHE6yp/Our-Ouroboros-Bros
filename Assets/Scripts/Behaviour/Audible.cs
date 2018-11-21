using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class Audible : MonoBehaviour
{

    AudioSource audioSource;
    public AudioClip hitAudio;

    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        //events
        Health health = GetComponent<Health>();
        if (health)
        {
            health.damaged += RecieveDamage;
        }
        Walking walking = GetComponent<Walking>();
        if (walking)
        {
            walking.jump += PlayPrimary;
        }

        //Weapon
        Weapon weapon = GetComponent<Weapon>();
        if (weapon)
        {
            weapon.strike += PlayPrimary;
        }
    }

    void PlayPrimary()
    {
        audioSource.Play();
    }

    void RecieveDamage()
    {
        audioSource.PlayOneShot(hitAudio);
    }

}
