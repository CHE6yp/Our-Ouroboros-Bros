using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MapEditor
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        public GameObject chunkCanvas;
        public GameObject obstacleCanvas;

        public Text templateNameText;
        public Text blockNameText;
        public Image blockSprite;
        public GameObject helpPanel;

        public Dropdown chunkType;
        public Toggle topExit;
        public Toggle bottomExit;
        public Toggle leftExit;
        public Toggle rightExit;

        //
        public Text obstacleNameText;


        void Awake()
        {
            instance = this;
            Debug.Log("UI manager Awake");
            Chunk.instance.switchTemplate += ChangeTemplate;
            Obstacle.instance.switchTemplate += ChangeObstacle;
            BlockPlacer.instance.switchBlock += ChangeBlock;

        }

        public void ToggleHelp()
        {
            helpPanel.SetActive(!helpPanel.activeSelf);
        }

        public void ChangeTemplate()
        {
            if (Chunk.newTemplate)
                templateNameText.text = "New Template";
            else
                templateNameText.text = "Template #" + Chunk.currentTemplateId;
            GetChunkType();
            GetChunkExits();

        }

        public void ChangeObstacle()
        {
            if (Obstacle.newTemplate)
                obstacleNameText.text = "New Obstacle";
            else
                obstacleNameText.text = "Obstacle #" + Obstacle.currentTemplateId;
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
            string blockText = BlockPlacer.instance.blocks[BlockPlacer.placedBlockType].name;

            blockNameText.text = blockText;
            blockSprite.sprite = BlockPlacer.instance.blocks[BlockPlacer.placedBlockType].sprite;
        }

        public void SwichToObstacles(bool obstacles)
        {
            chunkCanvas.SetActive(!obstacles);
            obstacleCanvas.SetActive(obstacles);
    }
    }
}
