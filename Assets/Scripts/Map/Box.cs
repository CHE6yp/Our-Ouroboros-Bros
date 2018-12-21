using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] surfaceSprites;
    public Sprite[] deepSprites;
    public Vector2 chunkCoordinates;

    // Start is called before the first frame update
    void Start()
    {
        //AssignSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AssignSprite()
    {
        spriteRenderer.sprite = surfaceSprites[Random.Range(0, surfaceSprites.Length)];
    }

    public void AssignSprite(int x, int y, Chunk chunk)
    {
        //Debug.Log(chunk.chunkTemplate[y - 1][x]);
        chunkCoordinates = new Vector2(x, y);
        if (y!=1 && !chunk.chunkTemplate[y-2][x].In(1,6,7,8,9))
        {
            //Debug.Log(chunk.chunkTemplate[y - 1][x]);
            spriteRenderer.sprite = surfaceSprites[Random.Range(0, surfaceSprites.Length)];
        }
        else
            spriteRenderer.sprite = deepSprites[Random.Range(0, deepSprites.Length)];
    }
}
