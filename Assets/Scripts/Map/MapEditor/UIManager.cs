using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MapEditor
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;


        public Text templateNameText;
        public Text blockNameText;
        public GameObject helpPanel;

        public Dropdown chunkType;

        
        void Awake()
        {
            instance = this;
            Chunk.switchTemplate += ChangeTemplate;
            Chunk.switchBlock += ChangeBlock;

        }

        public void ToggleHelp()
        {
            helpPanel.SetActive(!helpPanel.activeSelf);
        }

        public void ChangeTemplate()
        {
            if (Chunk.newChunk)
                templateNameText.text = "New Template";
            else
                templateNameText.text = "Template #" + Chunk.currentTemplateId;
            GetChunkType();

        }

        public void GetChunkType()
        {
            Debug.Log("Template ttype - "+Chunk.currentTemplate.ttype);
            if (Chunk.currentTemplate.ttype == 1)
                chunkType.value = 0;
            if (Chunk.currentTemplate.ttype == 2)
                chunkType.value = 1;
            if (Chunk.currentTemplate.ttype == 3)
                chunkType.value = 2;
        }

        public void ChangeChunkType(int ttype)
        {
            Debug.Log("Template ttype changed to " + ttype);
            Chunk.currentTemplate.ttype = ttype + 1;
        }

        public void ChangeBlock()
        {
            string blockText = "Block: ";
            switch (Chunk.placedBlockType)
            {
                case 1:
                    blockText += "Solid";
                    break;
                case 2:
                    blockText += "Enemy";
                    break;
                case 3:
                    blockText += "Spike";
                    break;
                case 4:
                    blockText += "SpikeReverse";
                    break;
                case 5:
                    blockText += "Pickup";
                    break;
                case 6:
                    blockText += "Proj up";
                    break;
                case 7:
                    blockText += "Proj down";
                    break;
                case 8:
                    blockText += "Proj left";
                    break;
                case 9:
                    blockText += "Proj right";
                    break;

            }

            blockNameText.text = blockText;
        }
    }
}
