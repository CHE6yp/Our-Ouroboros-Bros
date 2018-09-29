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


    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateMap()
    {

        GameObject start_ch = Instantiate(chunkPrefab, transform, false);
        start_ch.transform.localPosition = new Vector3(0, 5.5f);
        start_ch.GetComponent<Chunk>().GenerateChunk(0);
        Instantiate(camBoundary, new Vector3(-4.5f, 8),Quaternion.identity);

        for (int i = 1; i < mapLength-1; i++)
        {
            GameObject ch = Instantiate(chunkPrefab,  transform, false);
            ch.transform.localPosition = new Vector3(i * chunkDistance, 5.5f);
            ch.GetComponent<Chunk>().GenerateChunk(Random.Range(2, ChunkTemplates.templates.Length));
        }

        GameObject finish_ch = Instantiate(chunkPrefab, transform, false);
        finish_ch.transform.localPosition = new Vector3((mapLength-1)*chunkDistance, 5.5f);
        finish_ch.GetComponent<Chunk>().GenerateChunk(1);
        Instantiate(camBoundary, new Vector3(finish_ch.transform.position.x + 4.5f, 8), Quaternion.identity);
    }
}
