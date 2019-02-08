using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapEditor
{
    public class Block : MonoBehaviour
    {
        public int blockType;
        public SpriteRenderer spriteRenderer;

        public Sprite[] sprites;

        public void SetBlockType(int type)
        {
            blockType = type;
            if (type == 0)
                spriteRenderer.sprite = null;
            else
                //spriteRenderer.sprite = sprites[type - 1];
                spriteRenderer.sprite = BlockLibrary.instance.blocks[type-1].sprite;
        }

        //Надо переделать метод, чтобы в него передавался не инт а BlockType
        public void SetBlockType(BlockLibrary.BlockType bType)
        {
            blockType = bType.id;
            if (blockType == 0)
                spriteRenderer.sprite = null;
            else
                spriteRenderer.sprite = bType.sprite;
        }


        void OnMouseOver()
        {
            Debug.Log("?");
            if (Input.GetMouseButton(0))
                //SetBlockType(Chunk.placedBlockType);
                SetBlockType(BlockLibrary.instance.blocks[Chunk.placedBlockType]);
            else
                if (Input.GetMouseButton(1))
                SetBlockType(0);
        }

        
    }
}
