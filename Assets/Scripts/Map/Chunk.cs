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
    public ChunkTemplates.Template chunkTemplateJson;

    public GameObject backgroundBox;

    public void Generate(int tempId )
    {
        //int[][] template = ChunkTemplates.templates[tempId];
        //Generate(template);
    }

    public void Generate(ChunkTemplates.Template template)
    {
        chunkTemplateJson = template;

        foreach (ChunkTemplates.Block block in chunkTemplateJson.elements)
        {

            SpawnBlock((int)block.coordinates.x, (int)block.coordinates.y, block.ttype);
            //Debug.Log(block.coordinates.y);
        }

        Map.instance.ChunkDone();
    }

    void SpawnBlock(int x, int y, int type)
    {
        switch (type)
        {
            default:
                break;
            case 1:
                GameObject b = Instantiate(solidBox, this.transform, false);
                b.transform.localPosition = new Vector3(x, -y);
                b.GetComponent<Box>().AssignSprite(x,y,this);
                SendBoxColliderCoordinates(x, y);
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
                SendBoxColliderCoordinates(x, y);
                break;
            case 7:
                GameObject shd = Instantiate(shooterDown, this.transform, false);
                shd.transform.localPosition = new Vector3(x, -y);
                shd.GetComponent<Box>().AssignSprite(x, y, this);
                SendBoxColliderCoordinates(x, y);
                break;
            case 8:
                GameObject shl = Instantiate(shooterLeft, this.transform, false);
                shl.transform.localPosition = new Vector3(x, -y);
                shl.GetComponent<Box>().AssignSprite(x, y, this);
                SendBoxColliderCoordinates(x, y);
                break;
            case 9:
                GameObject shr = Instantiate(shooterRight, this.transform, false);
                shr.transform.localPosition = new Vector3(x, -y);
                shr.GetComponent<Box>().AssignSprite(x, y, this);
                SendBoxColliderCoordinates(x, y);
                break;
        }


        //GameObject backgroundB = Instantiate(backgroundBox, this.transform, false);
        //backgroundB.transform.localPosition = new Vector3(x, -y, 5);
        //backgroundB.GetComponent<Box>().AssignSprite();
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
        List<Vector2> coords = new List<Vector2>() {    new Vector2(transform.localPosition.x + x - 0.5f, -y + 0.5f),
                                                        new Vector2(transform.localPosition.x + x + 0.5f, -y + 0.5f),
                                                        new Vector2(transform.localPosition.x + x + 0.5f, -y - 0.5f),
                                                        new Vector2(transform.localPosition.x + x - 0.5f, -y - 0.5f) };
        //Debug.Log(coords);
        Map.instance.colliderCoordinates.Add(coords);   
    }
}
