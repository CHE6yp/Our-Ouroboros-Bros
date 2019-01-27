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
        public Block[][] mapGenBlocks = new Block[ChunkTemplates.chunkHeight][];

        public static bool playTesting;
        public static ChunkTemplates.Template playTestTemplate;
        public static ChunkTemplates.Template currentTemplate;


        public static int currentBlockType = 0;


        public void Start()
        {
            instance = this;
            GenerateChunk();
            if (playTesting)
                SetChunk(playTestTemplate);
            else
                ChunkTemplates.GetFromJson();


            ChangeBlockType(0);
        }


        /// <summary>
        /// Подготовка чанка, расстановка блоков итд
        /// </summary>
        void GenerateChunk()
        {

            Debug.Log("Generating chunk (" + ChunkTemplates.chunkWidth + "x" + ChunkTemplates.chunkHeight + ")");
            for (int i = 0; i < ChunkTemplates.chunkHeight; i++)
            {
                mapGenBlocks[i] = new Block[ChunkTemplates.chunkWidth];
                for (int k = 0; k < ChunkTemplates.chunkWidth; k++)
                {
                    Block block = SpawnBlock(k, i + 1, true);
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
            currentTemplate = template;
            for (int i = 0; i < ChunkTemplates.chunkHeight; i++)
            {
                for (int k = 0; k < ChunkTemplates.chunkWidth; k++)
                {
                    mapGenBlocks[i][k].SetBlockType(template.elements[i*ChunkTemplates.chunkWidth+k].ttype);
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
                ChunkTemplates.templatesContainer.templates[currentTemplateId] = GetTemplateMatrix();
                ChunkTemplates.SaveToTxt();
            }
        }

        void SaveTemplateAsNew()
        {
            ChunkTemplates.templatesContainer.templates.Add(GetTemplateMatrix());
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

        /// <summary>
        /// Удалить существующий темплейт
        /// </summary>
        public void DeleteTemplate()
        {
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
            Debug.Log(currentBlockType + "   " + BlockLibrary.instance.blocks.Count);
            if (currentBlockType == BlockLibrary.instance.blocks.Count - 1)
                currentBlockType = 0;
            else
                currentBlockType++;
            ChangeBlockType(currentBlockType);
        }

        public void PreviousBlockType()
        {
            if (currentBlockType == 0)
                currentBlockType = BlockLibrary.instance.blocks.Count - 1;
            else
                currentBlockType--;
            ChangeBlockType(currentBlockType);
        }

        public void ChangeBlockType(int id)
        {
            placedBlockType = id;
            switchBlock();
        }


        Block SpawnBlock(int x, int y, bool toMatrix)
        {

            GameObject b = Instantiate(mapGenBlock, this.transform, false);
            b.transform.localPosition = new Vector3((float)x / 2, (float)-y / 2);
            //b.GetComponent<Block>().SetBlockType(type);

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
            
            ChunkTemplates.Template template = new ChunkTemplates.Template();
            for (int i = 0; i < mapGenBlocks.Length; i++)
            {
                for (int k = 0; k < mapGenBlocks[i].Length; k++)
                {
                    ChunkTemplates.Block block = new ChunkTemplates.Block();
                    block.ttype = mapGenBlocks[i][k].blockType;
                    block.coordinates = new Vector2(k, i);

                    template.elements[i * ChunkTemplates.chunkWidth + k] = block;
                }
            }
            template.ttype = currentTemplate.ttype;

            return template;
        }

        public ChunkTemplates.Template GetTemplate()
        {
            ChunkTemplates.Template template = new ChunkTemplates.Template();

            for (int i = 0; i < mapGenBlocks.Length; i++)
            {
                for (int k = 0; k < mapGenBlocks[i].Length; k++)
                {

                    ChunkTemplates.Block block = new ChunkTemplates.Block();
                    block.ttype = mapGenBlocks[i][k].blockType;
                    block.coordinates = new Vector2(k, i);

                    template.elements[i * ChunkTemplates.chunkWidth + k] = block;

                }
            }

            return template;
        }




    }
}
