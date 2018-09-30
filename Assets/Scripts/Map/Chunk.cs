using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public GameObject solidBox;
    public GameObject enemyPref;
    public GameObject spikes;

    public void GenerateChunk(int tempId)
    {

        int[][] template = ChunkTemplates.templates[tempId];
        for (int i = 0; i < template.Length; i++)
        {
            for (int k = 0; k < template[i].Length; k++)
            {
                SpawnBlock(k, i+1, template[i][k]);
                SpawnBlock(k, -(i+1), template[i][k]);
            }
        }
    }

    void SpawnBlock(float x, float y, int type)
    {
        switch (type)
        {
            default:
                break;
            case 1:
                GameObject b = Instantiate(solidBox, this.transform, false);
                b.transform.localPosition = new Vector3(x/2, -y/2);
                break;
            case 2:
                GameObject e = Instantiate(enemyPref, this.transform, false);
                e.transform.localPosition = new Vector3(x / 2, -y / 2);
                break;
            case 3:
                GameObject s = Instantiate(spikes, this.transform, false);
                s.transform.localPosition = new Vector3(x / 2, -y / 2);
                break;
        }

    }
}
