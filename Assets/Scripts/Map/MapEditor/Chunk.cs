using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MapEditor
{
    public class Chunk : MonoBehaviour
    {
        public static Chunk instance;
        public GameObject mapGenBlock;

        public int[][] emptyTemplate = new int[20][]
        {
        new int[32] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1  },
        new int[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[32] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1  },
        };

        public Block[][] mapGenBlocks = new Block[20][];


        bool reversed = false;

        bool newChunk = true;
        public int tempId = 0;

        public TextMesh chunkIdText;

        public static int placedBlockType = 1;

        public void Start()
        {
            instance = this;
            ChunkTemplates.GetFromTxt();
            GenerateChunk();
            chunkIdText.text = "new";
        }

        public void Update()
        {
            
            /*
            if (Input.GetKeyDown(KeyCode.U))
            {
                newChunk = false;
                SetChunk(ChunkTemplates.templates[tempId]);
                chunkIdText.text = tempId.ToString();
            }
            */

            ///=============
            


        }

        public void ReverseMap()
        {
            reversed = !reversed;
            Camera.main.transform.position = (reversed) ? new Vector3(0, 10.5f, -10) : new Vector3(0, 0, -10);
        }

        /// <summary>
        /// Подготовка чанка, расстановка блоков итд
        /// </summary>
        void GenerateChunk()
        {

            for (int i = 0; i < emptyTemplate.Length; i++)
            {
                mapGenBlocks[i] = new Block[32];
                for (int k = 0; k < emptyTemplate[i].Length; k++)
                {
                    Block block = SpawnBlock(k, i + 1, emptyTemplate[i][k], true);
                    Block blockReversed = SpawnBlock(k, -(i + 1), emptyTemplate[i][k], false);

                    block.relatedBlock = blockReversed;
                    blockReversed.relatedBlock = block;
                }
            }
        }

        /// <summary>
        /// Применение темплейта на чанк
        /// </summary>
        /// <param name="template"></param>
        void SetChunk(int[][] template)
        {
            for (int i = 0; i < emptyTemplate.Length; i++)
            {

                for (int k = 0; k < emptyTemplate[i].Length; k++)
                {
                    mapGenBlocks[i][k].SetBothBlocksType(template[i][k]);
                }
            }
        }

        /// <summary>
        /// Сохранение в файл с темплейтами
        /// </summary>
        public void SaveTemplate()
        {
            if (newChunk)
                SaveTemplateAsNew();
            else
            {
                Debug.Log("Save existing template");
                ChunkTemplates.templates[tempId] = GetTemplateMatrix();
                ChunkTemplates.SaveToTxt();
            }
        }

        void SaveTemplateAsNew()
        {
            ChunkTemplates.templates.Add(GetTemplateMatrix());
            ChunkTemplates.SaveToTxt();
        }

        /// <summary>
        /// Чистый темплейт
        /// </summary>
        public void NewTemplate()
        {
            newChunk = true;
            SetChunk(emptyTemplate);
            chunkIdText.text = "new";
        }

        public void NextTemplate()
        {
            newChunk = false;
            if (tempId == ChunkTemplates.templates.Count - 1)
                tempId = 0;
            else
                tempId++;
            SetChunk(ChunkTemplates.templates[tempId]);
            chunkIdText.text = tempId.ToString();
        }

        public void PreviousTemplate()
        {
            newChunk = false;
            if (tempId == 0)
                tempId = ChunkTemplates.templates.Count - 1;
            else
                tempId--;
            SetChunk(ChunkTemplates.templates[tempId]);
            chunkIdText.text = tempId.ToString();
        }





        Block SpawnBlock(int x, int y, int type, bool toMatrix)
        {

            GameObject b = Instantiate(mapGenBlock, this.transform, false);
            b.transform.localPosition = new Vector3((float)x / 2, (float)-y / 2);
            b.GetComponent<Block>().SetBlockType(type);

            //Debug.Log(x + " " + y);
            if (toMatrix)
                mapGenBlocks[y - 1][x] = b.GetComponent<Block>();
            return b.GetComponent<Block>();

        }

        int[][] GetTemplateMatrix()
        {
            int[][] newTemplate = new int[20][];

            for (int i = 0; i < mapGenBlocks.Length; i++)
            {
                newTemplate[i] = new int[32];
                for (int k = 0; k < mapGenBlocks[i].Length; k++)
                {
                    newTemplate[i][k] = mapGenBlocks[i][k].blockType;
                    //SpawnBlock(k, -(i + 1), emptyTemplate[i][k]);
                }
            }

            return newTemplate;
        }

        string GetTemplateMatrixText()
        {
            string newTemplate = "new int[20][] \n        { ";

            for (int i = 0; i < mapGenBlocks.Length; i++)
            {
                //new int[32] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1  },
                newTemplate += "\n            new int[32] { ";
                for (int k = 0; k < mapGenBlocks[i].Length; k++)
                {
                    newTemplate += mapGenBlocks[i][k].blockType.ToString();
                    newTemplate += (k == mapGenBlocks[i].Length - 1) ? "" : ",";
                    //SpawnBlock(k, -(i + 1), emptyTemplate[i][k]);
                }
                newTemplate += "}";
                newTemplate += (i == mapGenBlocks.Length - 1) ? "" : ",";
            }
            newTemplate += "\n        },";

            return newTemplate;
        }


        /// <summary>
        /// Уже не нужно кажись
        /// </summary>
        public void CopyTemplate()
        {
            TextEditor te = new TextEditor();


            te.text = GetTemplateMatrixText();
            te.SelectAll();
            te.Copy();
        }





    }
}
