using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenBlock : MonoBehaviour
{
    public int blockType;
    public SpriteRenderer spriteRenderer;

    public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBlockType(int type)
    {
        blockType = type;
        if (type == 0)
            spriteRenderer.sprite = null;
        else
            spriteRenderer.sprite = sprites[type - 1];
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
            SetBlockType(1);
        else
            if (Input.GetMouseButton(1))
            SetBlockType(0);
    }

}
