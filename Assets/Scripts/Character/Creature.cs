using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Walking))]
[RequireComponent(typeof(Appearance))]
/// <summary>
/// Class contains references to other components
/// </summary>
public class Creature : MonoBehaviour
{
    public Health health;
    public Walking walking;
    public Appearance appearance;
    public Weapon weapon;

    void Awake()
    {
        health = GetComponent<Health>();
        walking = GetComponent<Walking>();
        appearance = GetComponent<Appearance>();
        weapon = GetComponentInChildren<Weapon>();
    }
}
