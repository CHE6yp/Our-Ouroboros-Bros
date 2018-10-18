using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] surfaceSprites;
    public Sprite[] deepSprites;

    // Start is called before the first frame update
    void Start()
    {
        AssignSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AssignSprite()
    {
        spriteRenderer.sprite = surfaceSprites[Random.Range(0, surfaceSprites.Length)];
    }
}
