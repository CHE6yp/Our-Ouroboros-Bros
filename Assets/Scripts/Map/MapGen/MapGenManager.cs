using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapGenManager : MonoBehaviour
{



    private string gameDataFileName = "chunks.json";

    // Start is called before the first frame update
    void Start()
    {
        SaveToJson();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadGameData()
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Application.streamingAssetsPath + "/" + gameDataFileName;
        Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            ChunkTemplates chT = JsonUtility.FromJson<ChunkTemplates>(dataAsJson);

            Debug.Log(chT.ToString());
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

    public void SaveToJson()
    {
        string filePath = Application.streamingAssetsPath + "/" + gameDataFileName;
        Debug.Log(filePath);


        if (File.Exists(filePath))
        {
            File.WriteAllText(filePath, GetTemplateMatrixJson());
        }
    }

    string GetTemplateMatrixJson()
    {

        string newTemplate = "{ \"templates\":\n\t[";
        for (int j = 0; j < ChunkTemplates.templates.Length; j++)
        {
            newTemplate += "\n\t\t[ ";

            for (int i = 0; i < ChunkTemplates.templates[j].Length; i++)
            {
                //new int[32] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1  },
                newTemplate += "\n\t\t\t[ ";
                for (int k = 0; k < ChunkTemplates.templates[j][i].Length; k++)
                {
                    newTemplate += ChunkTemplates.templates[j][i][k].ToString();
                    newTemplate += (k == ChunkTemplates.templates[j][i].Length - 1) ? "" : ",";
                }
                newTemplate += "]";
                newTemplate += (i == ChunkTemplates.templates[j].Length - 1) ? "" : ",";
            }
            newTemplate += "\n\t\t]";
            newTemplate += (j == ChunkTemplates.templates.Length - 1) ? "" : ",";
        }
        newTemplate += "\n\t]\n}";
        return newTemplate;
    }
}
