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
                spriteRenderer.sprite = sprites[type - 1];
        }

        void OnMouseOver()
        {
            if (Input.GetMouseButton(0))
                SetBlockType(Chunk.placedBlockType);
            else
                if (Input.GetMouseButton(1))
                SetBlockType(0);
        }
    }
}
