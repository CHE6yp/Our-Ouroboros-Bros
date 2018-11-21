﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public delegate void HealthDelegate();
    public event HealthDelegate damaged;
    public event HealthDelegate died;
    public int health = 5;
    public bool invincible;
    public float invincibleTime = 0.1f; //1 for player


    /*
    public void RecieveDamage(int damage)
    {
        if (invincible)
            return;


        audioSource.PlayOneShot(hitAudio);
        particleHit.Play();
        health = health - damage;
        UIManager.instance.DrawHealth(health);

        StartCoroutine(cam.ScreenShake());
        KnockBack();
        StartCoroutine(Invincibility());
        if (health == 0)
            PlayerController.instance.SwitchPlayers();
    }
    */

    public void RecieveDamage(int damage)
    {
        

        if (invincible)
            return;

        health = health - damage;
        //UIManager.instance.DrawHealth(health);

        //StartCoroutine(cam.ScreenShake());
        //KnockBack();

        
        StartCoroutine(Invincibility());
        damaged?.Invoke();
        if (health == 0)
            StartCoroutine(Die());
    }

    /*
    public IEnumerator Invincibility()
    {
        invincible = true;
        Color c = spriteRenderer.color;
        spriteRenderer.color = new Color(c.r, c.g, c.b, 0.5f);
        yield return new WaitForSeconds(invincibleTime - 0.02f);
        spriteRenderer.color = c;
        yield return new WaitForSeconds(0.02f);
        invincible = false;

    }
    */

    public IEnumerator Invincibility()
    {
        invincible = true;
        yield return new WaitForSeconds(invincibleTime);
        invincible = false;

    }

    IEnumerator Die()
    {
        Debug.Log("DED");
        this.GetComponent<BoxCollider2D>().enabled = false;
        //audioSource.PlayOneShot(hitAudio);
        //particleHit.Play();
        //spriteRenderer.enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(1);
        died?.Invoke();
        Destroy(this.gameObject);

    }
}
