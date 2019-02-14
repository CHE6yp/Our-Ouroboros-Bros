using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlacer : MonoBehaviour
{
    public static BlockPlacer instance;

    public delegate void BlockPlacerEvent();
    public event BlockPlacerEvent switchBlock;//b

    public static int placedBlockType = 1;//b
    public static int currentBlockType = 0;//b

    public List<BlockType> blocks = new List<BlockType>();


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //ChangeBlockType(0);
    }

    public void NextBlockType()
    {
        Debug.Log("Chunk NextBlockType");
        //Debug.Log(currentBlockType + "   " + BlockLibrary.instance.blocks.Count);
        if (currentBlockType == blocks.Count - 1)
            currentBlockType = 0;
        else
            currentBlockType++;
        ChangeBlockType(currentBlockType);
    }

    public void PreviousBlockType()
    {
        Debug.Log("Chunk PreviousBlockType");
        if (currentBlockType == 0)
            currentBlockType = blocks.Count - 1;
        else
            currentBlockType--;
        ChangeBlockType(currentBlockType);
    }

    public void ChangeBlockType(int id)
    {
        Debug.Log("Chunk ChangeBlockType");
        placedBlockType = id;
        switchBlock();
    }

    [System.Serializable]
    public struct BlockType
    {
        public int id;
        public string name;
        public Sprite sprite;
        public List<BlockChance> prefabs;


    }

    [System.Serializable]
    public struct BlockChance
    {
        public int divider; //the chance of spawning the block is 1/divider
        public int blockLibraryId;
    }




}
