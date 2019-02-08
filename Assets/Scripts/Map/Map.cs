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
    public bool chunkDebugMode = false;

    public int[][] mapLayout = new int[4][];
    public PlayerSpawn playerSpawn;

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

        PlayerController.instance.playerCharacter.transform.position = playerSpawn.transform.position;
    }

    void GenerateMapLayout()
    {
        mapLayout = EmptyLayout();

        int startY = Random.Range(0, 4);
        int startX = Random.Range(0, 4);
        AssignLayoutFirst(startX);
    }

    /// <summary>
    /// Массив с нулями
    /// </summary>
    /// <returns></returns>
    public int[][] EmptyLayout()
    {
        int[][] layout = new int[mapLayout.Length][];
        //создаем массив с нулями
        for (int y = 0; y < layout.Length; y++)
        {
            layout[y] = new int[mapLength];
            for (int x = 0; x < mapLength; x++)
            {
                layout[y][x] = 0;
            }
        }

        return layout;
    }

    /// <summary>
    /// Первый блок. Сверху вниз.
    /// </summary>
    /// <param name="x"></param>
    void AssignLayoutFirst(int x)
    {
        int layoutType = Random.Range(1, 3);

        mapLayout[0][x] = 4;
        x = (x == mapLayout.Length - 1 || (layoutType == 1 && x != 0) ) ? x - 1 : x + 1;

        AssignLayout(0, x);

    }

    /// <summary>
    /// Переделанный вариант, ближе к Спеланки. Сверху вниз.
    /// </summary>
    /// <param name="y"></param>
    /// <param name="x"></param>
    void AssignLayout(int y, int x)
    {

        int layoutType = Random.Range(1, 6);
        
        if (layoutType == 1 || layoutType == 2)
            layoutType = 1;
        else if (layoutType == 3 || layoutType == 4)
            layoutType = 2;
        else
            layoutType = 3;
        //1 left, 2 right, 3 down

        //если не нижний уровень
        if (y != mapLayout.Length - 1)
        {
            //left
            if (layoutType == 1)
            {
                //минус потому что 0 сверху а 2 снизу
                if (x == 0 || mapLayout[y][x-1] != 0)
                    AssignLayout(y, x);
                else
                {
                    mapLayout[y][x] = layoutType;
                    AssignLayout(y, x-1);
                }
            }
            //right
            if (layoutType == 2)
            {
                if (x == mapLayout.Length-1 || mapLayout[y][x+1] != 0)
                    AssignLayout(y, x);
                else
                {
                    mapLayout[y][x] = layoutType;
                    AssignLayout(y, x+1);
                }
            }
            //down
            if (layoutType == 3)
            {
                mapLayout[y][x] = layoutType;
                AssignLayout(y+1 , x);
            }
        }
        else
        {

            if (layoutType == 1)
            {
                if (x == 0 || mapLayout[y][x - 1] != 0)
                    AssignLayout(y, x);
                else
                {
                    mapLayout[y][x] = layoutType;
                    AssignLayoutBottom(x-1);
                }
            }
            if (layoutType == 2)
            {
                if (x == mapLayout.Length - 1 || mapLayout[y][x + 1] != 0)
                    AssignLayout(y, x);
                else
                {
                    mapLayout[y][x] = layoutType;
                    AssignLayoutBottom(x+1);
                }
            }
            if (layoutType == 3)
            {
                //mapLayout[y][x] = layoutType;
                AssignLayout(y, x);
            }
        }

    }


    void AssignLayoutBottom(int x)
    {
        int y = 3; //эту величину надо брать откудато а не хардкодить

        int layoutType = Random.Range(1, 6);

        if (layoutType == 1 || layoutType == 2)
            layoutType = 1;
        else if (layoutType == 3 || layoutType == 4)
            layoutType = 2;
        else
            layoutType = 3;

        if (layoutType == 1)
        {
            if (x == 0 || mapLayout[y][x - 1] != 0)
            {
                mapLayout[y][x] = 5; // комната с выходом
                return; //end map here
            }
            else
            {
                mapLayout[y][x] = layoutType;
                AssignLayoutBottom(x - 1);
            }
        }
        if (layoutType == 2)
        {
            if (x == mapLayout.Length - 1 || mapLayout[y][x + 1] != 0)
            {
                mapLayout[y][x] = 5; // комната с выходом
                return; //end map here
            }
            else
            {
                mapLayout[y][x] = layoutType;
                AssignLayoutBottom(x + 1);
            }
        }
        if (layoutType == 3)
        {
            mapLayout[y][x] = 5; // комната с выходом
            return; //end map here
        }

    }

    /// <summary>
    /// Изначальный вариант. Слева направо.  
    /// </summary>
    /// <param name="y"></param>
    /// <param name="x"></param>
    void AssignLayoutHorizontal(int y, int x)
    {
        int layoutType = Random.Range(1, 4);
        //1 up, 2 down, 3 right

        if (x != mapLength - 1)
        {
            if (layoutType == 1)
            {
                //минус потому что 0 сверху а 2 снизу
                if (y == 0 || mapLayout[y - 1][x] != 0)
                    AssignLayout(y, x);
                else
                {
                    mapLayout[y][x] = layoutType;
                    AssignLayout(y - 1, x);
                }
            }
            if (layoutType == 2)
            {
                if (y == 2 || mapLayout[y + 1][x] != 0)
                    AssignLayout(y, x);
                else
                {
                    mapLayout[y][x] = layoutType;
                    AssignLayout(y + 1, x);
                }
            }
            if (layoutType == 3)
            {
                mapLayout[y][x] = layoutType;
                AssignLayout(y, x + 1);
            }
        }
        else
        {
            if (layoutType == 1)
            {
                if (y == 0 || mapLayout[y - 1][x] != 0)
                    return; //end map here
                else
                {
                    mapLayout[y][x] = layoutType;
                    AssignLayout(y - 1, x);
                }
            }
            if (layoutType == 2)
            {
                if (y == 2 || mapLayout[y + 1][x] != 0)
                    return; //end map here
                else
                {
                    mapLayout[y][x] = layoutType;
                    AssignLayout(y + 1, x);
                }
            }
            if (layoutType == 3)
            {
                mapLayout[y][x] = layoutType;
                return; //end map here
            }
        }

    }

    void GenerateMapJson()
    {
        //Рекурсивно задаем гарантированный путь уровня
        GenerateMapLayout();

        //Выдаем лэйаут в консоль
        string layoutString = "";
        for (int y = 0; y < mapLayout.Length; y++)
        { 
            for (int x = 0; x < mapLength; x++)
            {
                layoutString += mapLayout[y][x].ToString();
            }
            layoutString += "\n";
        }
        Debug.Log(layoutString);

        //Конвертируем лэйаут пути в улучшенный лэйаут, с учетом стен и прочего.
        mapLayout = ConvertLayout(mapLayout);

        //Выдаем лэйаут в консоль
        layoutString = "";
        for (int y = 0; y < mapLayout.Length; y++)
        {
            for (int x = 0; x < mapLength; x++)
            {
                layoutString += mapLayout[y][x].ToString();
            }
            layoutString += "\n";
        }
        Debug.Log(layoutString);


        //Импортируем из джейсона 
        ChunkTemplates.GetFromJson();
        ChunkTemplates.GetObstaclesFromJson();

        for (int y = 0; y < mapLayout.Length; y++)
        {
            for (int x = 0; x < mapLength; x++)
            {
                GameObject ch = Instantiate(chunkPrefab, transform, false);
                ch.transform.localPosition = new Vector3(x * chunkDistance, -y*8);
                ch.GetComponent<Chunk>().DebugMode(chunkDebugMode);
                ch.GetComponent<Chunk>().GenerateRandomByType(mapLayout[y][x]);
            }
        }
    }


    /// <summary>
    /// конвертируем обычный лэйаут пути, в лейаут показывающий где должны быть обызательные выходы у чанков
    /// </summary>
    /// <returns>лэйаут с выходами</returns>
    int[][] ConvertLayout(int[][] layout)
    {
        int[][] convertedLayout = EmptyLayout();

        //обозначения выходов в новом лэйауте
        //1 left right; 2 up down; 3 up left; 4 down left; 5 up rigth; 6 down right;
        //7 комната спауна, 8 комната выхода

        for (int y = 0; y < layout.Length; y++)
        {
            for (int x = 0; x < layout[0].Length; x++)
            {
                if (layout[y][x] == 1)
                {
                    //если лэаут шел сверху, нужен обязательный выход сверху.
                    if (y != 0 && layout[y - 1][x] == 3)
                        convertedLayout[y][x] = 3; // =5
                    else
                        //если лэйаут шел не сверху, значит он шел справа, нужен выход направо
                        convertedLayout[y][x] = 1; // =12
                }
                if (layout[y][x] == 2)
                {
                    //если лэаут шел сверху, нужен обязательный выход сверху.
                    if (y != 0 && layout[y - 1][x] == 3)
                        convertedLayout[y][x] = 5; //=9
                    else
                        //если лэйаут шел не сверху, значит он шел слева, нужен выход направо
                        convertedLayout[y][x] = 1; //=12
                }
                if (layout[y][x] == 3)
                {
                    if (y != 0 && layout[y - 1][x] == 3)
                        convertedLayout[y][x] = 2; //=3
                    else
                    {
                        if (x != 0 && (layout[y][x - 1] == 2 || layout[y][x - 1] == 4))
                            //слева
                            convertedLayout[y][x] = 4; //=6
                        else
                            //справа
                            convertedLayout[y][x] = 6; //=10
                    }
                }
                if (layout[y][x] == 4)
                {
                    convertedLayout[y][x] = 7;
                }
                if (layout[y][x] == 5)
                {
                    convertedLayout[y][x] = 8;
                }
            }
        }

        return convertedLayout;
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

    #region clipper
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
        int scalingFactor = 1000;

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

    #endregion

    public void ChunkDone()
    {
        chunkDone++;
        if (chunkDone == mapLength*mapLayout.Length)
        {

           
            //new Vector2(-0.5f, 0.5f),  верх лево 
            //new Vector2(32 * mapLength + 0.5f, +0.5f), верх право
            //new Vector2(32 * mapLength + 0.5f, -19 - 0.5f), низ право
            //new Vector2(-0.5f, -19 - 0.5f) }; низ лево


            //up
            List<Vector2> coords = new List<Vector2>() {        new Vector2(- 3f, 0.5f),
                                                                new Vector2(ChunkTemplates.chunkWidth*mapLength + 3f, 0.5f),
                                                                new Vector2(ChunkTemplates.chunkWidth*mapLength + 3f, 3f),
                                                                new Vector2(- 3f, 3f) };
            //left
            List<Vector2> coords2 = new List<Vector2>() {       new Vector2(- 0.5f, 0.5f),
                                                                new Vector2(-3f,    0.5f),
                                                                new Vector2(-3f,    -ChunkTemplates.chunkHeight*mapLayout.Length + 0.5f),
                                                                new Vector2(- 0.5f, -ChunkTemplates.chunkHeight*mapLayout.Length + 0.5f) };
            //down
            List<Vector2> coords3 = new List<Vector2>() {       new Vector2(- 3f,                                     -ChunkTemplates.chunkHeight*mapLayout.Length - 3),
                                                                new Vector2(ChunkTemplates.chunkWidth*mapLength + 3f, -ChunkTemplates.chunkHeight*mapLayout.Length - 3),
                                                                new Vector2(ChunkTemplates.chunkWidth*mapLength + 3f, -ChunkTemplates.chunkHeight*mapLayout.Length + 0.5f),
                                                                new Vector2(- 3f,                                     -ChunkTemplates.chunkHeight*mapLayout.Length + 0.5f) };
            //right
            List<Vector2> coords4 = new List<Vector2>() {       new Vector2(ChunkTemplates.chunkWidth*mapLength + 3,     0.5f),
                                                                new Vector2(ChunkTemplates.chunkWidth*mapLength - 0.5f,  0.5f),
                                                                new Vector2(ChunkTemplates.chunkWidth*mapLength - 0.5f,  -ChunkTemplates.chunkHeight*mapLayout.Length - 3),
                                                                new Vector2(ChunkTemplates.chunkWidth*mapLength + 3,     -ChunkTemplates.chunkHeight*mapLayout.Length - 3) };

            colliderCoordinates.Add(coords);
            colliderCoordinates.Add(coords2);
            colliderCoordinates.Add(coords3);
            colliderCoordinates.Add(coords4);

            CreateLevelCollider(UniteCollisionPolygons(colliderCoordinates));

        }
           
    }


}
