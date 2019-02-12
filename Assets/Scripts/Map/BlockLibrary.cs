using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockLibrary : MonoBehaviour
{
    public static BlockLibrary instance;
    //all block types
    public List<BlockLibrary.BlockType> blocks = new List<BlockLibrary.BlockType>();

    private void Awake()
    {
        instance = this;
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
        public GameObject prefab;
    }
}
