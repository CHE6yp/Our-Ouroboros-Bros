using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTrigger : MonoBehaviour
{

    public CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D col)
    {
        //PlayerController pc = new PlayerController(); //GetComponent<PlayerController>().SwitchPlayers();
        if (col.gameObject.tag == "Enemy")
        {
            //Debug.Log("You Died");
            //PlayerController.instance.SwitchPlayers();
            characterController.RecieveDamage(1);
        }
    }
}
