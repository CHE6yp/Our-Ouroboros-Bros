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
        public Image blockSprite;
        public GameObject helpPanel;

        public Dropdown chunkType;
        public Toggle topExit;
        public Toggle bottomExit;
        public Toggle leftExit;
        public Toggle rightExit;


        void Awake()
        {
            instance = this;
            Chunk.instance.switchTemplate += ChangeTemplate;
            Chunk.instance.switchBlock += ChangeBlock;

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
            GetChunkExits();

        }

        public void GetChunkType()
        {
            chunkType.value = Chunk.currentTemplate.ttype - 1;
        }

        public void ChangeChunkType(int ttype)
        {
            Debug.Log("Template ttype changed to " + ttype);
            Chunk.currentTemplate.ttype = ttype + 1;
        }

        public void GetChunkExits()
        {
            topExit.isOn = Chunk.currentTemplate.topExit;
            bottomExit.isOn = Chunk.currentTemplate.bottomExit;
            leftExit.isOn = Chunk.currentTemplate.leftExit;
            rightExit.isOn = Chunk.currentTemplate.rightExit;
        }

        public void ChangeChunkTopExit(bool on)
        {
            Chunk.currentTemplate.topExit = on;
        }

        public void ChangeChunkBottomExit(bool on)
        {
            Chunk.currentTemplate.bottomExit = on;
        }

        public void ChangeChunkLeftExit(bool on)
        {
            Chunk.currentTemplate.leftExit = on;
        }

        public void ChangeChunkRightExit(bool on)
        { 
            Chunk.currentTemplate.rightExit = on;
        }


        public void ChangeBlock()
        {
            string blockText = BlockLibrary.instance.blocks[Chunk.placedBlockType].name;

            blockNameText.text = blockText;
            blockSprite.sprite = BlockLibrary.instance.blocks[Chunk.placedBlockType].sprite;
        }
    }
}
