using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MapEditor
{
    public class Chunk : MonoBehaviour
    {
        public static Chunk instance;

        public delegate void EditorChunkEvent();
        public static event EditorChunkEvent switchTemplate;
        public static event EditorChunkEvent switchBlock;

        public static bool newChunk = true;
        public static int currentTemplateId = 0;
        public static int placedBlockType = 1;

        public GameObject mapGenBlock;
        public Block[][] mapGenBlocks = new Block[20][];

        public static bool playTesting;
        public static ChunkTemplates.Template playTestTemplate;
        public static ChunkTemplates.Template currentTemplate;
       
         

        public void Start()
        {
            instance = this;
            GenerateChunk();
            if (playTesting)
                SetChunk(playTestTemplate);
            else
                ChunkTemplates.GetFromJson();
            
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

        }


        /// <summary>
        /// Подготовка чанка, расстановка блоков итд
        /// </summary>
        void GenerateChunk()
        {
            for (int i = 0; i < ChunkTemplates.emptyTemplate.Length; i++)
            {
                mapGenBlocks[i] = new Block[32];
                for (int k = 0; k < ChunkTemplates.emptyTemplate[i].Length; k++)
                {
                    Block block = SpawnBlock(k, i + 1, ChunkTemplates.emptyTemplate[i][k], true);
                    Block blockReversed = SpawnBlock(k, -(i + 1), ChunkTemplates.emptyTemplate[i][k], false);

                    block.relatedBlock = blockReversed;
                    blockReversed.relatedBlock = block;
                }
            }
        }

        /// <summary>
        /// Применение темплейта на чанк
        /// </summary>
        /// <param name="template"></param>
        void SetChunk(ChunkTemplates.Template template)
        {
            for (int i = 0; i < ChunkTemplates.emptyTemplate.Length; i++)
            {

                for (int k = 0; k < ChunkTemplates.emptyTemplate[i].Length; k++)
                {
                    mapGenBlocks[i][k].SetBothBlocksType(template.elements[i*32+k].ttype);
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
                ChunkTemplates.templates.templates[currentTemplateId] = GetTemplateMatrix();
                ChunkTemplates.SaveToTxt();
            }
        }

        void SaveTemplateAsNew()
        {
            ChunkTemplates.templates.templates.Add(GetTemplateMatrix());
            ChunkTemplates.SaveToTxt();
        }

        /// <summary>
        /// Чистый темплейт
        /// </summary>
        public void NewTemplate()
        {
            newChunk = true;
            SetChunk(new ChunkTemplates.Template());
            switchTemplate();
        }

        public void NextTemplate()
        {
            newChunk = false;
            if (currentTemplateId == ChunkTemplates.templates.templates.Count - 1)
                currentTemplateId = 0;
            else
                currentTemplateId++;
            SetChunk(ChunkTemplates.templates.templates[currentTemplateId]);
            switchTemplate();
        }

        public void PreviousTemplate()
        {
            newChunk = false;
            if (currentTemplateId == 0)
                currentTemplateId = ChunkTemplates.templates.templates.Count - 1;
            else
                currentTemplateId--;
            SetChunk(ChunkTemplates.templates.templates[currentTemplateId]);
            switchTemplate();
        }

        public static void ChangeBlockType(int id)
        {
            placedBlockType = id;
            switchBlock();
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

        /// <summary>
        /// Возвращает матрицу текущего шаблона
        /// </summary>
        /// <returns></returns>
        public ChunkTemplates.Template GetTemplateMatrix()
        {
            int[][] newTemplate = new int[20][];

            for (int i = 0; i < mapGenBlocks.Length; i++)
            {
                newTemplate[i] = new int[32];
                for (int k = 0; k < mapGenBlocks[i].Length; k++)
                {
                    newTemplate[i][k] = mapGenBlocks[i][k].blockType;
                }
            }

            ChunkTemplates.Template template = new ChunkTemplates.Template();
            for (int i = 0; i < mapGenBlocks.Length; i++)
            {
                for (int k = 0; k < mapGenBlocks[i].Length; k++)
                {
                    ChunkTemplates.Block block = new ChunkTemplates.Block();
                    block.ttype = mapGenBlocks[i][k].blockType;
                    block.coordinates = new Vector2(k, i);

                    template.elements[i * 32 + k] = block;
                }
            }


            return template;
        }

        public ChunkTemplates.Template GetTemplate()
        {
            ChunkTemplates.Template newTemplate = new ChunkTemplates.Template();

            for (int i = 0; i < mapGenBlocks.Length; i++)
            {
                for (int k = 0; k < mapGenBlocks[i].Length; k++)
                {

                    newTemplate.elements[i*32+k].ttype = mapGenBlocks[i][k].blockType;
                    newTemplate.elements[i * 32 + k].coordinates = new Vector2(k, i);

                }
            }

            return newTemplate;
        }




    }
}
