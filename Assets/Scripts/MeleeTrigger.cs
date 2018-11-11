using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTrigger : MonoBehaviour
{
    public bool active;
    public float duration = 0.1f;

    public AudioSource audioSource;

    //эти штуки со спрайтами для дебага
    public SpriteRenderer spriteRenderer;
    public Color passiveColor;
    public Color activeColor;

    // Start is called before the first frame update
    void Awake()
    {
        passiveColor = new Color(1, 1, 1, 0.5f);
        activeColor = new Color(0.5f, 0, 0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (active && collision.tag == "Enemy")
            Destroy(collision.gameObject);
    }

    public IEnumerator Strike()
    {
        active = true;
        spriteRenderer.color = activeColor;
        audioSource.Play();
        yield return new WaitForSeconds(duration);
        active = false;
        spriteRenderer.color = passiveColor;
    }
}
