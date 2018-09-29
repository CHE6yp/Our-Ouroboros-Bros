using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public GameObject solidBox;
    public int[][] templ1 = new int[10][] 
    {
        new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[16] { 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[16] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
    };

    public void GenerateChunk(int tempId)
    {

        int[][] template = ChunkTemplates.templates[tempId];
        for (int i = 0; i < template.Length; i++)
        {
            for (int k = 0; k < template[i].Length; k++)
            {
                SpawnBlock(k, i, template[i][k]);
                SpawnBlock(k, -i, template[i][k]);
            }
        }
    }

    void SpawnBlock(int x, int y, int type)
    {
        switch (type)
        {
            default:
                break;
            case 0:
                break;
            case 1:
                GameObject b = Instantiate(solidBox, this.transform, false);
                b.transform.localPosition = new Vector3(x, -y);
                break;
        }

    }
}
