using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBound : MonoBehaviour
{

    public bool bound = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CamBoundary")
            bound = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "CamBoundary")
            bound = false;
    }
}
