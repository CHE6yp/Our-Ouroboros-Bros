using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Appearance : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public ParticleSystem hitParticle;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //events
        Walking walking = GetComponent<Walking>();
        if (walking)
            walking.switchDirection += SwitchDirection;

        Health health = GetComponent<Health>();
        if (health)
            health.damaged += Damaged;
    }

    void SwitchDirection()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    void Damaged()
    {
        hitParticle.Play();
    }
}
