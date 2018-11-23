using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public GameObject projectile; 
    public float interval = 3;
    public Transform gunpoint;
    public Vector2 direction = new Vector2(0, 0);

    void Start()
    {
        InvokeRepeating("LaunchProjectile", 2, interval);
    }

    void LaunchProjectile()
    {
        GameObject instance = Instantiate(projectile, gunpoint.position, Quaternion.identity);
        instance.GetComponent<Projectile>().GetDirection(direction);


        //instance.velocity = Random.insideUnitSphere * 5;
    }

}
