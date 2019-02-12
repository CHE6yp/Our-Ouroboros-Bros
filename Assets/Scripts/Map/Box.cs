using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public void AssignSprite()
    {
        spriteRenderer.sprite = surfaceSprites[Random.Range(0, surfaceSprites.Length)];
    }

    public void AssignSprite(int x, int y)
    {
        if (y != 0)
        {
            int upperBlockType = Map.instance.mapTemplate[y-1][x];
            if (!upperBlockType.In(1, 6, 7, 8, 9))
            {
                //Debug.Log(chunk.chunkTemplate[y - 1][x]);
                spriteRenderer.sprite = surfaceSprites[Random.Range(0, surfaceSprites.Length)];
            }
            else
                spriteRenderer.sprite = deepSprites[Random.Range(0, deepSprites.Length)];
        }
        else
            spriteRenderer.sprite = deepSprites[Random.Range(0, deepSprites.Length)];



    }
}
