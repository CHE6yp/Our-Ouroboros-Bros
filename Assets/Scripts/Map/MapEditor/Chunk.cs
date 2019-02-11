using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MapEditor
{
    public class Chunk : MonoBehaviour
    {
        public static Chunk instance;

        public delegate void EditorChunkEvent();
        public event EditorChunkEvent switchTemplate;
        public event EditorChunkEvent switchBlock;

        public static bool newChunk = true;
        public static int currentTemplateId = 0;
        public static int placedBlockType = 1;

        public GameObject mapGenBlock;
        public Block[][] mapGenBlocks = new Block[ChunkTemplates.chunkHeight][];

        public static bool playTesting;
        public static ChunkTemplates.Template playTestTemplate;
        public static ChunkTemplates.Template currentTemplate;


        public static int currentBlockType = 0;

        public void Awake()
        {
            instance = this;
            Debug.Log("Chunk Awake");
        }

        public void Start()
        {
            Debug.Log("Chunk Start");
            Debug.Log("playtesting " + playTesting);
            GenerateChunk();
            if (playTesting)
                SetChunk(playTestTemplate);
            else
            {
                ChunkTemplates.GetFromJson();
                ChunkTemplates.GetObstaclesFromJson();
            }

            ChangeBlockType(0);
        }


        /// <summary>
        /// Подготовка чанка, расстановка блоков итд
        /// </summary>
        void GenerateChunk()
        {
            Debug.Log("Chunk GenerateChunk");
            Debug.Log("Generating chunk (" + ChunkTemplates.chunkWidth + "x" + ChunkTemplates.chunkHeight + ")");
            for (int y = 0; y < ChunkTemplates.chunkHeight; y++)
            {
                mapGenBlocks[y] = new Block[ChunkTemplates.chunkWidth];
                for (int x = 0; x < ChunkTemplates.chunkWidth; x++)
                {
                    Block block = SpawnBlock(x, y, true);
                }
            }

            NewTemplate();
        }

        /// <summary>
        /// Применение темплейта на чанк
        /// </summary>
        /// <param name="template"></param>
        void SetChunk(ChunkTemplates.Template template)
        {
            Debug.Log("Chunk SetChunk");
            currentTemplate = template;
            for (int y = 0; y < ChunkTemplates.chunkHeight; y++)
            {
                for (int x = 0; x < ChunkTemplates.chunkWidth; x++)
                {
                    //Debug.Log(x + " " + y);
                    mapGenBlocks[y][x].SetBlockType(template.elements[y*ChunkTemplates.chunkWidth+x].ttype);
                }
            }

            
        }

        /// <summary>
        /// Сохранение в файл с темплейтами
        /// </summary>
        public void SaveTemplate()
        {
            Debug.Log("Chunk SaveTemplate");
            if (newChunk)
                SaveTemplateAsNew();
            else
            {
                Debug.Log("Save existing template");
                ChunkTemplates.templatesContainer.templates[currentTemplateId] = GetTemplateMatrix();
                ChunkTemplates.SaveToTxt();
            }
        }

        void SaveTemplateAsNew()
        {
            Debug.Log("Chunk SaveTemplateAsNew");
            ChunkTemplates.templatesContainer.templates.Add(GetTemplateMatrix());
            ChunkTemplates.SaveToTxt();
        }

        /// <summary>
        /// Чистый темплейт
        /// </summary>
        public void NewTemplate()
        {
            Debug.Log("Chunk NewTemplate");
            newChunk = true;
            SetChunk(new ChunkTemplates.Template());
            switchTemplate();
        }

        /// <summary>
        /// Удалить существующий темплейт
        /// </summary>
        public void DeleteTemplate()
        {
            Debug.Log("Chunk DeleteTemplate");
            if (newChunk)
            {
                Debug.LogWarning("Can't delete new template");
                return;
            }

            ChunkTemplates.templatesContainer.templates.RemoveAt(currentTemplateId);
            Debug.Log("DELETED CHUNK TEMPLATE!!!!");
            PreviousTemplate();
        }



        public void NextTemplate()
        {
            Debug.Log("Chunk NextTemplate");
            newChunk = false;
            if (currentTemplateId == ChunkTemplates.templatesContainer.templates.Count - 1)
                currentTemplateId = 0;
            else
                currentTemplateId++;
            SetChunk(ChunkTemplates.templatesContainer.templates[currentTemplateId]);
            switchTemplate();
        }

        public void PreviousTemplate()
        {
            Debug.Log("Chunk PreviousTemplate");
            newChunk = false;
            if (currentTemplateId == 0)
                currentTemplateId = ChunkTemplates.templatesContainer.templates.Count - 1;
            else
                currentTemplateId--;
            SetChunk(ChunkTemplates.templatesContainer.templates[currentTemplateId]);
            switchTemplate();
        }



        public void NextBlockType()
        {
            Debug.Log("Chunk NextBlockType");
            //Debug.Log(currentBlockType + "   " + BlockLibrary.instance.blocks.Count);
            if (currentBlockType == BlockLibrary.instance.blocks.Count - 1)
                currentBlockType = 0;
            else
                currentBlockType++;
            ChangeBlockType(currentBlockType);
        }

        public void PreviousBlockType()
        {
            Debug.Log("Chunk PreviousBlockType");
            if (currentBlockType == 0)
                currentBlockType = BlockLibrary.instance.blocks.Count - 1;
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


        Block SpawnBlock(int x, int y, bool toMatrix)
        {
            Debug.Log("Chunk SpawnBlock");
            GameObject b = Instantiate(mapGenBlock, this.transform, false);
            b.transform.localPosition = new Vector3((float)x / 2, (float)-(y+1) / 2);
            b.GetComponent<Block>().coordinates = new Vector2Int(x, y);

            Debug.Log("MapGenBlock collider issue brute solving is here");
            //b.GetComponent<BoxCollider2D>().enabled = false;
            //b.GetComponent<BoxCollider2D>().enabled = true;

            if (toMatrix)
                mapGenBlocks[y][x] = b.GetComponent<Block>();
            return b.GetComponent<Block>();

        }

        /// <summary>
        /// Возвращает матрицу текущего шаблона
        /// </summary>
        /// <returns></returns>
        public ChunkTemplates.Template GetTemplateMatrix()
        {
            Debug.Log("Chunk GetTemplateMatrix");
            ChunkTemplates.Template template = new ChunkTemplates.Template();
            for (int i = 0; i < mapGenBlocks.Length; i++)
            {
                for (int k = 0; k < mapGenBlocks[i].Length; k++)
                {
                    ChunkTemplates.Block block = new ChunkTemplates.Block();
                    block.ttype = mapGenBlocks[i][k].blockType;
                    block.coordinates = new Vector2Int(k, i);

                    template.elements[i * ChunkTemplates.chunkWidth + k] = block;
                }
            }
            template.ttype = currentTemplate.ttype;

            template.topExit = currentTemplate.topExit;
            template.bottomExit = currentTemplate.bottomExit;
            template.leftExit = currentTemplate.leftExit;
            template.rightExit = currentTemplate.rightExit;


            return template;
        }

        public ChunkTemplates.Template GetTemplate()
        {
            Debug.Log("Chunk GetTemplate");
            ChunkTemplates.Template template = new ChunkTemplates.Template();

            for (int i = 0; i < mapGenBlocks.Length; i++)
            {
                for (int k = 0; k < mapGenBlocks[i].Length; k++)
                {

                    ChunkTemplates.Block block = new ChunkTemplates.Block();
                    block.ttype = mapGenBlocks[i][k].blockType;
                    block.coordinates = new Vector2Int(k, i);

                    template.elements[i * ChunkTemplates.chunkWidth + k] = block;

                }
            }

            return template;
        }

        public void ClearEvents()
        {
            
        }



    }
}
