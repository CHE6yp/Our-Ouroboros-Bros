using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public GameObject solidBox;
    public GameObject solidBoxHell;
    public GameObject enemyPref;
    public GameObject spikes;
    public GameObject spikesHell;
    public GameObject spikesRev;
    public GameObject spikesHellRev;
    public GameObject pickup;

    public GameObject shooter;
    public GameObject shooterDown;
    public GameObject shooterLeft;
    public GameObject shooterRight;

    public int[][] chunkTemplate;

    public GameObject backgroundBox;

    public void Generate(int tempId)
    {
        int[][] template = ChunkTemplates.templates[tempId];
        Generate(template);
    }

    public void Generate(int[][] template)
    {
        chunkTemplate = template;
        for (int i = 0; i < template.Length; i++)
        {
            for (int k = 0; k < template[i].Length; k++)
            {
                SpawnBlock(k, i + 1, template[i][k]);
                //SpawnBlock(k, -(i + 1), template[i][k], true);
            }
        }
    }

    void SpawnBlock(int x, int y, int type, bool hell = false)
    {
        switch (type)
        {
            default:
                break;
            case 1:
                GameObject b = Instantiate(solidBox, this.transform, false);
                b.transform.localPosition = new Vector3(x, -y);
                b.GetComponent<Box>().AssignSprite(x,y,this);
                break;
            case 2:
                GameObject e = Instantiate(enemyPref, this.transform, false);
                e.transform.localPosition = new Vector3(x, -y);
                break;
            case 3:
                GameObject s = Instantiate(spikes, this.transform, false);
                s.transform.localPosition = new Vector3(x, -y);
                break;
            case 4:
                GameObject sr = Instantiate(spikesRev, this.transform, false);
                sr.transform.localPosition = new Vector3(x, -y);
                break;
            case 5:
                GameObject p = Instantiate(pickup, this.transform, false);
                p.transform.localPosition = new Vector3(x, -y);
                break;
            case 6:
                GameObject sh = Instantiate(shooter, this.transform, false);
                sh.transform.localPosition = new Vector3(x, -y);
                sh.GetComponent<Box>().AssignSprite(x, y, this);
                break;
            case 7:
                GameObject shd = Instantiate(shooterDown, this.transform, false);
                shd.transform.localPosition = new Vector3(x, -y);
                shd.GetComponent<Box>().AssignSprite(x, y, this);
                break;
            case 8:
                GameObject shl = Instantiate(shooterLeft, this.transform, false);
                shl.transform.localPosition = new Vector3(x, -y);
                shl.GetComponent<Box>().AssignSprite(x, y, this);
                break;
            case 9:
                GameObject shr = Instantiate(shooterRight, this.transform, false);
                shr.transform.localPosition = new Vector3(x, -y);
                shr.GetComponent<Box>().AssignSprite(x, y, this);
                break;
        }


        //GameObject backgroundB = Instantiate(backgroundBox, this.transform, false);
        //backgroundB.transform.localPosition = new Vector3(x, -y, 5);
        //backgroundB.GetComponent<Box>().AssignSprite();


        //stop
        if (!hell)
            return;
        else
        {
            return;
            switch (type)
            {
                default:
                    break;
                case 1:
                    GameObject b = Instantiate(solidBoxHell, this.transform, false);
                    b.transform.localPosition = new Vector3(x, -y);
                    break;
                case 2:
                    GameObject e = Instantiate(enemyPref, this.transform, false);
                    e.transform.localPosition = new Vector3(x, -y);
                    break;
                case 3:
                    GameObject s = Instantiate(spikesHellRev, this.transform, false);
                    s.transform.localPosition = new Vector3(x, -y);
                    break;
                case 4:
                    GameObject sr = Instantiate(spikesHell, this.transform, false);
                    sr.transform.localPosition = new Vector3(x, -y);
                    break;
                case 5:
                    GameObject p = Instantiate(pickup, this.transform, false);
                    p.transform.localPosition = new Vector3(x, -y);
                    break;
                case 6:
                    GameObject sh = Instantiate(shooterDown, this.transform, false);
                    sh.transform.localPosition = new Vector3(x, -y);
                    break;
                case 7:
                    GameObject shd = Instantiate(shooter, this.transform, false);
                    shd.transform.localPosition = new Vector3(x, -y);
                    break;
                case 8:
                    GameObject shl = Instantiate(shooterLeft, this.transform, false);
                    shl.transform.localPosition = new Vector3(x, -y);
                    break;
                case 9:
                    GameObject shr = Instantiate(shooterRight, this.transform, false);
                    shr.transform.localPosition = new Vector3(x, -y);
                    break;
            }
        }


    }

    public void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
