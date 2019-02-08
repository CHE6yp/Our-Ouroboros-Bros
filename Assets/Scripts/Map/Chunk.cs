using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //ChunkTemplates.templatesContainer.templates[Random.Range(0, ChunkTemplates.templatesContainer.templates.Count)]
        int randomTemplateId = Random.Range(0, ChunkTemplates.templatesContainer.templates.Count);
        ChunkTemplates.Template randomTemplate = ChunkTemplates.templatesContainer.templates[randomTemplateId];
        if ((ttype == 0 && randomTemplate.ttype != 4 && randomTemplate.ttype != 5) || randomTemplate.ttype == ttype)
        {
            Generate(randomTemplateId);
        }
        else
            GenerateRandomByType(ttype);

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

            SpawnBlock((int)block.coordinates.x, (int)block.coordinates.y, block.ttype);
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
