using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTrigger : MonoBehaviour
{   

    public BoxCollider2D boxCollider;
    public SpriteRenderer spriteRenderer;
    public bool down;
    public CharacterController characterController;
    


    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator Strike(float cooldown, float duration)
    {
        

        boxCollider.enabled = true;
        spriteRenderer.enabled = true;
        yield return new WaitForSeconds(duration);

        boxCollider.enabled = false;
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(cooldown - duration);

        
    }
}
