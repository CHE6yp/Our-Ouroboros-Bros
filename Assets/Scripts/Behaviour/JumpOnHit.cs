using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HitOnContact))]
public class JumpOnHit : MonoBehaviour
{

    void Awake()
    {
        HitOnContact hitOnContact = GetComponent<HitOnContact>();
        Walking walking = GetComponentInParent<Walking>();
        if (walking)
        {
            hitOnContact.hit += walking.AirJump;
        }
    }
}
