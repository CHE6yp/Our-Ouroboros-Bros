using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Min(2)]
    public int mapLength = 5;
    public GameObject chunkPrefab;
    public float chunkDistance = 16f;
    public GameObject camBoundary;

    public bool generateAtStart = true;


    // Start is called before the first frame update
    void Start()
    {
        if (MapEditor.Chunk.playTesting)
            PlayTestTemplate(MapEditor.Chunk.playTestTemplate);
        else
            if (generateAtStart)
                GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateMap()
    {
        ChunkTemplates.GetFromTxt();

        Debug.Log(ChunkTemplates.templates[0][0][0]);

        GameObject start_ch = Instantiate(chunkPrefab, transform, false);
        start_ch.transform.localPosition = new Vector3(0, 0);
        start_ch.GetComponent<Chunk>().Generate(0);
        Instantiate(camBoundary, new Vector3(7f, 0),Quaternion.identity);

        for (int i = 1; i < mapLength-1; i++)
        {
            GameObject ch = Instantiate(chunkPrefab,  transform, false);
            ch.transform.localPosition = new Vector3(i * chunkDistance, 0);
            ch.GetComponent<Chunk>().Generate(Random.Range( 2, ChunkTemplates.templates.Count));
        }

        GameObject finish_ch = Instantiate(chunkPrefab, transform, false);
        finish_ch.transform.localPosition = new Vector3((mapLength-1)*chunkDistance, 0);
        finish_ch.GetComponent<Chunk>().Generate(1);
        Instantiate(camBoundary, new Vector3(finish_ch.transform.position.x + 24.5f, 0), Quaternion.identity);
    }

    void PlayTestTemplate(int[][] template)
    {
        Clear();


        GameObject start_ch = Instantiate(chunkPrefab, transform, false);
        start_ch.transform.localPosition = new Vector3(0, 0);
        start_ch.GetComponent<Chunk>().Generate(0);
        Instantiate(camBoundary, new Vector3(7f, 0), Quaternion.identity);

        for (int i = 1; i < mapLength - 1; i++)
        {
            GameObject ch = Instantiate(chunkPrefab, transform, false);
            ch.transform.localPosition = new Vector3(i * chunkDistance, 0);
            ch.GetComponent<Chunk>().Generate(template);
        }

        GameObject finish_ch = Instantiate(chunkPrefab, transform, false);
        finish_ch.transform.localPosition = new Vector3((mapLength - 1) * chunkDistance, 0);
        finish_ch.GetComponent<Chunk>().Generate(1);
        Instantiate(camBoundary, new Vector3(finish_ch.transform.position.x + 24.5f, 0), Quaternion.identity);
    }

    public void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
