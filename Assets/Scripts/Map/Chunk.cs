using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Chunk : MonoBehaviour
{

    public int[][] chunkTemplate;
    public ChunkTemplates.Template chunkTemplateJson;

    public GameObject backgroundBox;

    //debugging
    public GameObject debuggingGameobjects;
    public TextMesh chunkTypeText;


    public void GenerateRandomByType(int ttype)
    {
        //1 left right; 2 up down; 3 up left; 4 down left; 5 up rigth; 6 down right;
        //7 комната спауна, 8 комната выхода
        ChunkTemplates.Template template;

        //по умолчанию (ttype == 0) это может быть любая комната, кроме стартовой и конечной
        template = ChunkTemplates.templatesContainer.templates.Where(i => i.ttype != 4 && i.ttype != 5).OrderBy(n => UnityEngine.Random.value).FirstOrDefault();

        //start room
        if (ttype == 7)
            template = ChunkTemplates.templatesContainer.templates.Where(i => i.ttype == 4).OrderBy(n => UnityEngine.Random.value).FirstOrDefault(); //use whatever you prefer for random
        //exit room
        if (ttype == 8)
            template = ChunkTemplates.templatesContainer.templates.Where(i => i.ttype == 5).OrderBy(n => UnityEngine.Random.value).FirstOrDefault();

        if (ttype == 1)
            template = ChunkTemplates.templatesContainer.templates.Where(i => i.leftExit == true && i.rightExit == true && i.ttype != 4 && i.ttype != 5).OrderBy(n => UnityEngine.Random.value).FirstOrDefault(); //use whatever you prefer for random
        if (ttype == 2)
            template = ChunkTemplates.templatesContainer.templates.Where(i => i.topExit == true && i.bottomExit == true && i.ttype != 4 && i.ttype != 5).OrderBy(n => UnityEngine.Random.value).FirstOrDefault();
        if (ttype == 3)
            template = ChunkTemplates.templatesContainer.templates.Where(i => i.topExit == true && i.leftExit == true && i.ttype != 4 && i.ttype != 5).OrderBy(n => UnityEngine.Random.value).FirstOrDefault(); //use whatever you prefer for random
        if (ttype == 4)
            template = ChunkTemplates.templatesContainer.templates.Where(i => i.bottomExit == true && i.leftExit == true && i.ttype != 4 && i.ttype != 5).OrderBy(n => UnityEngine.Random.value).FirstOrDefault();
        if (ttype == 5)
            template = ChunkTemplates.templatesContainer.templates.Where(i => i.topExit == true && i.rightExit == true && i.ttype != 4 && i.ttype != 5).OrderBy(n => UnityEngine.Random.value).FirstOrDefault(); //use whatever you prefer for random
        if (ttype == 6)
            template = ChunkTemplates.templatesContainer.templates.Where(i => i.bottomExit == true && i.rightExit == true && i.ttype != 4 && i.ttype != 5).OrderBy(n => UnityEngine.Random.value).FirstOrDefault();


        Generate(template);
    }

    public void Generate(int tempId )
    {
        ChunkTemplates.Template template = ChunkTemplates.templatesContainer.templates[tempId];
        Generate(template);
    }

    public void Generate(ChunkTemplates.Template template)
    {
        chunkTemplateJson = template;

        foreach (ChunkTemplates.Block block in chunkTemplateJson.elements)
        {

            SpawnBlock(block.coordinates.x, block.coordinates.y, block.ttype);
            //Debug.Log(block.coordinates.y);
        }

        chunkTypeText.text = "ttype "+template.ttype.ToString(); //debugging
        Map.instance.ChunkDone();
    }


    void SpawnBlock(int x, int y, int type)
    {
        if (type == 0)
            return;

        GameObject block = Instantiate(BlockLibrary.instance.blocks[type-1].prefab, this.transform, false);
        block.transform.localPosition = new Vector3(x, -y);
        if (block.GetComponent<Box>())
        {
            block.GetComponent<Box>().AssignSprite(x, y, this);
            SendBoxColliderCoordinates(x, y);
        }
    }

    public void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    void SendBoxColliderCoordinates(int x, int y)
    {
        List<Vector2> coords = new List<Vector2>() {    new Vector2(transform.localPosition.x + x - 0.5f, transform.localPosition.y -y + 0.5f),
                                                        new Vector2(transform.localPosition.x + x + 0.5f, transform.localPosition.y -y + 0.5f),
                                                        new Vector2(transform.localPosition.x + x + 0.5f, transform.localPosition.y -y - 0.5f),
                                                        new Vector2(transform.localPosition.x + x - 0.5f, transform.localPosition.y -y - 0.5f) };
        //Debug.Log(coords);
        Map.instance.colliderCoordinates.Add(coords);   
    }

    public void DebugMode(bool flag)
    {
        debuggingGameobjects.SetActive(flag);
    }
}
