using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrab : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            //GetComponent<Walking>().Jump();
            Debug.Log("HIT");
        }
        Debug.DrawRay(transform.position, -Vector2.up, Color.red);
    }
}
