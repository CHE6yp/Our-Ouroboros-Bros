using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Min(2)]
    public int mapLength = 5;
    public GameObject chunkPrefab;
    public float chunkDistance = 16f;

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
        start_ch.transform.localPosition = new Vector3(0, 4.5f);
        start_ch.GetComponent<Chunk>().GenerateChunk(0);

        for (int i = 1; i < mapLength-1; i++)
        {
            GameObject ch = Instantiate(chunkPrefab,  transform, false);
            ch.transform.localPosition = new Vector3(i * chunkDistance, 4.5f);
            ch.GetComponent<Chunk>().GenerateChunk(Random.Range(2, ChunkTemplates.templates.Length));
        }

        GameObject finish_ch = Instantiate(chunkPrefab, transform, false);
        finish_ch.transform.localPosition = new Vector3((mapLength-1)*chunkDistance, 4.5f);
        finish_ch.GetComponent<Chunk>().GenerateChunk(1);
    }
}
