using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

using ClipperLib;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;

public class Map : MonoBehaviour
{

    public static Map instance;
    [Min(2)]
    public int mapLength = 5;
    public GameObject chunkPrefab;
    public float chunkDistance = 16f;
    public GameObject camBoundary;

    public bool generateAtStart = true;

    //списки координат для генерации больших коллайдеров
    public List<List<Vector2>> colliderCoordinates = new List<List<Vector2>>();
    int chunkDone = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (MapEditor.Chunk.playTesting)
            PlayTestTemplate(MapEditor.Chunk.playTestTemplate);
        else
            if (generateAtStart)
                GenerateMapJson();
    }

    void GenerateMapJson()
    {
        ChunkTemplates.GetFromJson();

        Debug.Log(ChunkTemplates.templatesContainer.templates[0].ttype);

        GameObject start_ch = Instantiate(chunkPrefab, transform, false);
        start_ch.transform.localPosition = new Vector3(0, 0);
        start_ch.GetComponent<Chunk>().Generate(ChunkTemplates.templatesContainer.templates[0]);
        //Instantiate(camBoundary, new Vector3(7f, 0), Quaternion.identity);

        for (int i = 1; i < mapLength - 1; i++)
        {
            GameObject ch = Instantiate(chunkPrefab, transform, false);
            ch.transform.localPosition = new Vector3(i * chunkDistance, 0);
            ch.GetComponent<Chunk>().Generate(ChunkTemplates.templatesContainer.templates[Random.Range(2, ChunkTemplates.templatesContainer.templates.Count)]);
        }

        GameObject finish_ch = Instantiate(chunkPrefab, transform, false);
        finish_ch.transform.localPosition = new Vector3((mapLength - 1) * chunkDistance, 0);
        finish_ch.GetComponent<Chunk>().Generate(ChunkTemplates.templatesContainer.templates[1]);
        //Instantiate(camBoundary, new Vector3(finish_ch.transform.position.x + 24.5f, 0), Quaternion.identity);
    }

    void PlayTestTemplate(ChunkTemplates.Template template)
    {
        //Clear();

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


    ///
    ///Далее код отсуда
    ///https://gamedev.stackexchange.com/questions/125927/how-do-i-merge-colliders-in-a-tile-based-game

    //this function takes a list of polygons as a parameter, this list of polygons represent all the polygons that constitute collision in your level.
    public List<List<Vector2>> UniteCollisionPolygons(List<List<Vector2>> polygons)
    {
        //this is going to be the result of the method
        List<List<Vector2>> unitedPolygons = new List<List<Vector2>>();
        Clipper clipper = new Clipper();

        //clipper only works with ints, so if we're working with floats, we need to multiply all our floats by
        //a scaling factor, and when we're done, divide by the same scaling factor again
        int scalingFactor = 10000;

        //this loop will convert our List<List<Vector2>> to what Clipper works with, which is "Path" and "IntPoint"
        //and then add all the Paths to the clipper object so we can process them
        for (int i = 0; i < polygons.Count; i++)
        {
            Path allPolygonsPath = new Path(polygons[i].Count);

            for (int j = 0; j < polygons[i].Count; j++)
            {
                allPolygonsPath.Add(new IntPoint(Mathf.Floor(polygons[i][j].x * scalingFactor), Mathf.Floor(polygons[i][j].y * scalingFactor)));
            }
            clipper.AddPath(allPolygonsPath, PolyType.ptSubject, true);

        }

        //this will be the result
        Paths solution = new Paths();

        //having added all the Paths added to the clipper object, we tell clipper to execute an union
        clipper.Execute(ClipType.ctUnion, solution);

        //the union may not end perfectly, so we're gonna do an offset in our polygons, that is, expand them outside a little bit
        ClipperOffset offset = new ClipperOffset();
        offset.AddPaths(solution, JoinType.jtMiter, EndType.etClosedPolygon);
        //5 is the ammount of offset
        offset.Execute(ref solution, 5f);

        //now we just need to conver it into a List<List<Vector2>> while removing the scaling
        foreach (Path path in solution)
        {
            List<Vector2> unitedPolygon = new List<Vector2>();
            foreach (IntPoint point in path)
            {
                unitedPolygon.Add(new Vector2(point.X / (float)scalingFactor, point.Y / (float)scalingFactor));
            }
            unitedPolygons.Add(unitedPolygon);
        }

        //this removes some redundant vertices in the polygons when they are too close from each other
        //may be useful to clean things up a little if your initial collisions don't match perfectly from tile to tile
        unitedPolygons = RemoveClosePointsInPolygons(unitedPolygons);

        //everything done
        return unitedPolygons;
    }

    //create the collider in unity from the list of polygons
    public void CreateLevelCollider(List<List<Vector2>> polygons)
    {
        //GameObject colliderObj = new GameObject("LevelCollision");
        //colliderObj.layer = GR.inst.GetLayerID(Layer.PLATFORM);
        //colliderObj.transform.SetParent(level.levelObj.transform);

        //PolygonCollider2D collider = colliderObj.AddComponent<PolygonCollider2D>();

        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        collider.pathCount = polygons.Count;

        for (int i = 0; i < polygons.Count; i++)
        {
            Vector2[] points = polygons[i].ToArray();

            collider.SetPath(i, points);
        }
    }

    public List<List<Vector2>> RemoveClosePointsInPolygons(List<List<Vector2>> polygons)
    {
        float proximityLimit = 0.001f;

        List<List<Vector2>> resultPolygons = new List<List<Vector2>>();

        foreach (List<Vector2> polygon in polygons)
        {
            List<Vector2> pointsToTest = polygon;
            List<Vector2> pointsToRemove = new List<Vector2>();

            foreach (Vector2 pointToTest in pointsToTest)
            {
                foreach (Vector2 point in polygon)
                {
                    if (point == pointToTest || pointsToRemove.Contains(point)) continue;

                    bool closeInX = Mathf.Abs(point.x - pointToTest.x) < proximityLimit;
                    bool closeInY = Mathf.Abs(point.y - pointToTest.y) < proximityLimit;

                    if (closeInX && closeInY)
                    {
                        pointsToRemove.Add(pointToTest);
                        break;
                    }
                }
            }
            polygon.RemoveAll(x => pointsToRemove.Contains(x));

            if (polygon.Count > 0)
            {
                resultPolygons.Add(polygon);
            }
        }

        return resultPolygons;
    }


    public void ChunkDone()
    {
        chunkDone++;
        if (chunkDone == mapLength)
        {

           
            //new Vector2(-0.5f, 0.5f),  верх лево 
            //new Vector2(32 * mapLength + 0.5f, +0.5f), верх право
            //new Vector2(32 * mapLength + 0.5f, -19 - 0.5f), низ право
            //new Vector2(-0.5f, -19 - 0.5f) }; низ лево


            List<Vector2> coords = new List<Vector2>() {    new Vector2(- 3f, 0.5f),
                                                        new Vector2(32*mapLength + 3f,  + 0.5f),
                                                        new Vector2(32*mapLength + 3f, 3f),
                                                        new Vector2(- 3f, 3f) };
            List<Vector2> coords2 = new List<Vector2>() {    new Vector2(- 0.5f, 0.5f),
                                                        new Vector2(-3f,  + 0.5f),
                                                        new Vector2(-3f, -19 - 0.5f),
                                                        new Vector2(- 0.5f, -19- 0.5f) };
            List<Vector2> coords3 = new List<Vector2>() {    new Vector2(- 3f, -22f),
                                                        new Vector2(32*mapLength + 3f,  -22f),
                                                        new Vector2(32*mapLength + 3f, -19 - 0.5f),
                                                        new Vector2(- 3f, -19- 0.5f) };
            List<Vector2> coords4 = new List<Vector2>() {    new Vector2(32*mapLength+3, 0.5f),
                                                        new Vector2(32*mapLength + 0.5f,  + 0.5f),
                                                        new Vector2(32*mapLength + 0.5f, -19 - 0.5f),
                                                        new Vector2(32*mapLength+3, -19- 0.5f) };

            colliderCoordinates.Add(coords);
            colliderCoordinates.Add(coords2);
            colliderCoordinates.Add(coords3);
            colliderCoordinates.Add(coords4);

            CreateLevelCollider(UniteCollisionPolygons(colliderCoordinates));

        }
           
    }


}
