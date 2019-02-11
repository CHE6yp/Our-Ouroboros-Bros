using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapEditor
{
    public class Block : MonoBehaviour
    {
        public int blockType;
        public SpriteRenderer spriteRenderer;
        public Vector2Int coordinates;

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
            
            if (blockType == 13)
            {
                Debug.Log("Placing on obstacle block");
                return;
            }

            if (blockType == 12)
            {
                //убираем все блоки препятствия
            }

            if (bType.id == 12)
            {
                if (coordinates.x < ChunkTemplates.chunkWidth - 5 && coordinates.y < ChunkTemplates.chunkHeight - 3)
                {
                    //ставим блоки
                    for (int y = 0; y < 3; y++)
                    {
                        for (int x = 0; x < 5; x++)
                        {
                            if (x == 0 && y == 0)
                                continue;
                            Chunk.instance.mapGenBlocks[y+coordinates.y][x+coordinates.x].GetComponent<Block>().SetBlockType(13);
                        }
                    }
                }
                else
                    Debug.Log("Can't fit obstacle");
            }

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
                SetBlockType(BlockLibrary.instance.blocks[Chunk.placedBlockType]);
            else
                if (Input.GetMouseButton(1))
                SetBlockType(0);
        }

        
    }
}
