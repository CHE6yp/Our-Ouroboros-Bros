using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public delegate void WeaponDelegate();
    public event WeaponDelegate strike;

    public bool canAttack = true;
    public float duration = 0.05f;
    public float cooldown = 0.6f;

    public MeleeTrigger meleeTrigger;
    public MeleeTrigger meleeTriggerDown;
    public MeleeTrigger meleeTriggerUp;

    //связь с передвижением мне не нравится, но пока что не вижу другого способа
    //запретить персонажу бить вниз, если он стоит. разве что кучей евентов, но тоже тупо
    Walking walking;


    void Awake()
    {
        walking = GetComponentInParent<Walking>();
        if (walking)
            walking.switchDirection += Turn;
    }

    public void Strike(float verticalAxes)
    {
        StartCoroutine(StrikeIEnum( verticalAxes));
    }



    public IEnumerator StrikeIEnum(float verticalAxes)
    {
        if (!canAttack)
            yield break;
        canAttack = false;


        ///неправильно вправо влево удары переписать
        ///

        if (verticalAxes > 0.55f)
            StartCoroutine(meleeTriggerUp.Strike(cooldown,duration));
        //else if (!grounded && Input.GetAxis("Vertical") < -0.55f) 
        else if (verticalAxes < -0.55f)
            StartCoroutine(meleeTriggerDown.Strike(cooldown, duration));
        else 
            StartCoroutine(meleeTrigger.Strike(cooldown, duration));


        strike?.Invoke();

        canAttack = true;
    }

    void Turn()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
