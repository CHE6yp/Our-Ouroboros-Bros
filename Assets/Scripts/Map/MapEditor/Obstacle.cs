using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MapEditor
{
    public class Obstacle : MonoBehaviour
    {
        public static Obstacle instance;

        public delegate void EditorChunkEvent();
        public event EditorChunkEvent switchTemplate;

        public static bool newTemplate = true;
        public static int currentTemplateId = 0;

        public GameObject mapGenBlock;
        public int h = 3;
        public Block[][] mapGenBlocks = new Block[ChunkTemplates.obstacleHeight][];

        public static ChunkTemplates.ObstacleTemplate currentObstacleTemplate;


        public void Awake()
        {
            instance = this;
        }

        public void Start()
        {
            GenerateTemplate();
            ChunkTemplates.GetObstaclesFromJson();
        }


        /// <summary>
        /// Подготовка чанка, расстановка блоков итд
        /// </summary>
        void GenerateTemplate()
        {

            Debug.Log("Generating chunk (" + ChunkTemplates.obstacleWidth + "x" + ChunkTemplates.obstacleHeight + ")");
            for (int y = 0; y < ChunkTemplates.obstacleHeight; y++)
            {
                mapGenBlocks[y] = new Block[ChunkTemplates.obstacleWidth];
                for (int x = 0; x < ChunkTemplates.obstacleWidth; x++)
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
        void SetTemplate(ChunkTemplates.ObstacleTemplate template)
        {
            currentObstacleTemplate = template;
            for (int y = 0; y < ChunkTemplates.obstacleHeight; y++)
            {
                for (int x = 0; x < ChunkTemplates.obstacleWidth; x++)
                {
                    mapGenBlocks[y][x].SetBlockType(template.elements[y * ChunkTemplates.obstacleWidth + x].ttype);
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
                ChunkTemplates.obstacleTemplatesContainer.templates[currentTemplateId] = GetTemplateMatrix();
                ChunkTemplates.SaveObstaclesToTxt(); 
            }
        }

        void SaveTemplateAsNew()
        {
            ChunkTemplates.obstacleTemplatesContainer.templates.Add(GetTemplateMatrix());
            ChunkTemplates.SaveObstaclesToTxt();
        }

        /// <summary>
        /// Чистый темплейт
        /// </summary>
        public void NewTemplate()
        {
            newTemplate = true;
            SetTemplate(new ChunkTemplates.ObstacleTemplate());
            switchTemplate();
        }

        /// <summary>
        /// Удалить существующий темплейт
        /// </summary>
        public void DeleteTemplate()
        {
            if (newTemplate)
            {
                Debug.LogWarning("Can't delete new template");
                return;
            }

            ChunkTemplates.obstacleTemplatesContainer.templates.RemoveAt(currentTemplateId);
            Debug.Log("DELETED CHUNK TEMPLATE!!!!");
            PreviousTemplate();
        }



        public void NextTemplate()
        {
            newTemplate = false;
            if (currentTemplateId == ChunkTemplates.obstacleTemplatesContainer.templates.Count - 1)
                currentTemplateId = 0;
            else
                currentTemplateId++;
            SetTemplate(ChunkTemplates.obstacleTemplatesContainer.templates[currentTemplateId]);
            switchTemplate();
        }

        public void PreviousTemplate()
        {
            newTemplate = false;
            if (currentTemplateId == 0)
                currentTemplateId = ChunkTemplates.obstacleTemplatesContainer.templates.Count - 1;
            else
                currentTemplateId--;
            SetTemplate(ChunkTemplates.obstacleTemplatesContainer.templates[currentTemplateId]);
            switchTemplate();
        }


        Block SpawnBlock(int x, int y, bool toMatrix)
        {

            GameObject b = Instantiate(mapGenBlock, this.transform, false);
            b.transform.localPosition = new Vector3((float)x / 2, (float)-(y + 1) / 2);
            b.GetComponent<Block>().coordinates = new Vector2Int(x, y);

            //Debug.Log("MapGenBlock collider issue brute solving is here");
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
        public ChunkTemplates.ObstacleTemplate GetTemplateMatrix()
        {

            ChunkTemplates.ObstacleTemplate template = new ChunkTemplates.ObstacleTemplate();
            for (int i = 0; i < mapGenBlocks.Length; i++)
            {
                for (int k = 0; k < mapGenBlocks[i].Length; k++)
                {
                    ChunkTemplates.Block block = new ChunkTemplates.Block();
                    block.ttype = mapGenBlocks[i][k].blockType;
                    block.coordinates = new Vector2Int(k, i);

                    template.elements[i * ChunkTemplates.obstacleWidth + k] = block;
                }
            }
            //template.ttype = currentObstacleTemplate.ttype;


            return template;
        }

        public ChunkTemplates.ObstacleTemplate GetTemplate()
        {
            ChunkTemplates.ObstacleTemplate template = new ChunkTemplates.ObstacleTemplate();

            for (int i = 0; i < mapGenBlocks.Length; i++)
            {
                for (int k = 0; k < mapGenBlocks[i].Length; k++)
                {

                    ChunkTemplates.Block block = new ChunkTemplates.Block();
                    block.ttype = mapGenBlocks[i][k].blockType;
                    block.coordinates = new Vector2Int(k, i);

                    template.elements[i * ChunkTemplates.obstacleWidth + k] = block;

                }
            }

            return template;
        }

        public void ClearEvents()
        {

        }



    }
}
