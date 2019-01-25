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
                spriteRenderer.sprite = Chunk.instance.blockTypes[type-1].sprite;
        }

        //Надо переделать метод, чтобы в него передавался не инт а BlockType
        public void SetBlockType(BlockType bType)
        {
            blockType = bType.id;
            if (blockType == 0)
                spriteRenderer.sprite = null;
            else
                spriteRenderer.sprite = bType.sprite;
        }


        void OnMouseOver()
        {
            if (Input.GetMouseButton(0))
                //SetBlockType(Chunk.placedBlockType);
                SetBlockType(Chunk.instance.blockTypes[Chunk.placedBlockType]);
            else
                if (Input.GetMouseButton(1))
                SetBlockType(0);
        }

        [System.Serializable]
        public struct BlockType
        {
            public int id;
            public string name;
            public Sprite sprite;
        }
    }
}
