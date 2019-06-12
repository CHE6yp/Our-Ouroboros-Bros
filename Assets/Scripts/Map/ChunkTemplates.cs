using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class ChunkTemplates
{
    public static Templates templatesContainer;
    public static ObstacleTemplates obstacleTemplatesContainer;
    public static int chunkHeight = 8;
    public static int chunkWidth = 10;
    

    public static int obstacleHeight = 3;
    public static int obstacleWidth = 5;



    public static void GetFromJson()
    {
        templatesContainer = new Templates();
        string filePath = Application.streamingAssetsPath + "/chunksJson.txt";
        Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            templatesContainer = JsonUtility.FromJson<Templates>(dataAsJson);
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    public static void GetObstaclesFromJson()
    {
        obstacleTemplatesContainer = new ObstacleTemplates();
        string filePath = Application.streamingAssetsPath + "/obstaclesJson.txt";
        Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            obstacleTemplatesContainer = JsonUtility.FromJson<ObstacleTemplates>(dataAsJson);
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }



    public static void SaveToTxt()
    {
        string filePathJson = Application.streamingAssetsPath + "/chunksJson.txt";

        if (File.Exists(filePathJson))
        {
            File.WriteAllText(filePathJson, GetTemplateMatrixTxtJson());
        }
    }

    public static void SaveObstaclesToTxt()
    {
        string filePathJson = Application.streamingAssetsPath + "/obstaclesJson.txt";

        if (File.Exists(filePathJson))
        {
            File.WriteAllText(filePathJson, GetObstacleTemplateMatrixTxtJson());
        }
    }

    static string GetTemplateMatrixTxtJson()
    {
        string newTemplate = "";
        
        newTemplate = JsonUtility.ToJson(templatesContainer);
        return newTemplate;
    }

    static string GetObstacleTemplateMatrixTxtJson()
    {
        string newTemplate = "";

        newTemplate = JsonUtility.ToJson(obstacleTemplatesContainer);
        return newTemplate;
    }

    [System.Serializable]
    public class Templates
    {
        public List<Template> templates = new List<Template>();
    }

    [System.Serializable]
    public class Template
    {
        public int id;
        public int ttype = 1;
        public bool topExit = false;
        public bool bottomExit = false;
        public bool leftExit = false;
        public bool rightExit = false;
        public Block[] elements = new Block[80];

        public Template()
        {
            for (int i = 0; i < 80; i++)
            {
                elements[i] = new Block();
            }
        }

        public int[][] GetMatrix()
        {
            int[][] matrix = new int[chunkHeight][];
            for (int y = 0; y < chunkHeight; y++)
            {
                matrix[y] = new int[chunkWidth];
            }
            foreach(Block block in elements)
            {
                matrix[block.coordinates.y][block.coordinates.x] = block.ttype;
            }
            return matrix;
        }
    }

    [System.Serializable]
    public class ObstacleTemplates
    {
        public List<ObstacleTemplate> templates = new List<ObstacleTemplate>();
    }

    [System.Serializable]
    public class ObstacleTemplate
    {
        public int id;
        public int ttype = 1;
        public Block[] elements = new Block[15];

        public ObstacleTemplate()
        {
            for (int i = 0; i < 15; i++)
            {
                elements[i] = new Block();
            }
        }

        public int[][] GetMatrix()
        {
            int[][] matrix = new int[obstacleHeight][];
            for (int y = 0; y < obstacleHeight; y++)
            {
                matrix[y] = new int[obstacleWidth];
            }
            foreach (Block block in elements)
            {
                matrix[block.coordinates.y][block.coordinates.x] = block.ttype;
            }
            return matrix;
        }
    }

    [System.Serializable]
    public class Block
    {
        public Vector2Int coordinates;
        public int ttype = 0;
        
    }
}
