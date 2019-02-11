using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MapEditor
{
    public class TemplateEditor : MonoBehaviour
    {
        public static TemplateEditor instance;

        public delegate void EditorChunkEvent();
        public event EditorChunkEvent switchTemplate;

        public static bool newTemplate = true;
        public static int currentTemplateId = 0;


        public GameObject mapGenBlock;
        public Block[][] mapGenBlocks = new Block[ChunkTemplates.chunkHeight][];

        public static ChunkTemplates.Template currentTemplate;


        public void Awake()
        {
            instance = this;
        }

        public void Start()
        {
            GenerateTemplate();
            ChunkTemplates.GetFromJson();
        }

        /// <summary>
        /// Подготовка чанка, расстановка блоков итд
        /// </summary>
        void GenerateTemplate()
        {
            Debug.Log("TemplateEditor GenerateChunk");
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
        /// Применение темплейта 
        /// </summary>
        /// <param name="template"></param>
        void SetTemplate(ChunkTemplates.Template template)
        {
            Debug.Log("Chunk SetChunk");
            currentTemplate = template;
            for (int y = 0; y < ChunkTemplates.chunkHeight; y++)
            {
                for (int x = 0; x < ChunkTemplates.chunkWidth; x++)
                {
                    //Debug.Log(x + " " + y);
                    mapGenBlocks[y][x].SetBlockType(template.elements[y * ChunkTemplates.chunkWidth + x].ttype);
                }
            }


        }

        /// <summary>
        /// Сохранение в файл с темплейтами
        /// </summary>
        public void SaveTemplate()
        {
            if (newTemplate)
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
            newTemplate = true;
            SetTemplate(new ChunkTemplates.Template());
            switchTemplate();
        }

        /// <summary>
        /// Удалить существующий темплейт
        /// </summary>
        public void DeleteTemplate()
        {
            Debug.Log("Chunk DeleteTemplate");
            if (newTemplate)
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
            newTemplate = false;
            if (currentTemplateId == ChunkTemplates.templatesContainer.templates.Count - 1)
                currentTemplateId = 0;
            else
                currentTemplateId++;
            SetTemplate(ChunkTemplates.templatesContainer.templates[currentTemplateId]);
            switchTemplate();
        }

        public void PreviousTemplate()
        {
            Debug.Log("Chunk PreviousTemplate");
            newTemplate = false;
            if (currentTemplateId == 0)
                currentTemplateId = ChunkTemplates.templatesContainer.templates.Count - 1;
            else
                currentTemplateId--;
            SetTemplate(ChunkTemplates.templatesContainer.templates[currentTemplateId]);
            switchTemplate();
        }

        Block SpawnBlock(int x, int y, bool toMatrix)
        {
            GameObject b = Instantiate(mapGenBlock, this.transform, false);
            b.transform.localPosition = new Vector3((float)x / 2, (float)-(y + 1) / 2);
            b.GetComponent<Block>().coordinates = new Vector2Int(x, y);

            //Это тут потому что юнити тупит
            b.GetComponent<BoxCollider2D>().enabled = false;
            b.GetComponent<BoxCollider2D>().enabled = true;

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
    }
}
