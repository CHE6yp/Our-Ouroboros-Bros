using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using ClipperLib;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;

public class Map : MonoBehaviour
{

    public static Map instance;
    [Min(2)]
    public int mapLayoutLength = 4;

    public GameObject camBoundary;

    public bool generateAtStart = true;
    public bool chunkDebugMode = false;

    public int[][] mapLayout = new int[4][];
    public int[][] mapTemplate;
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
            layout[y] = new int[mapLayoutLength];
            for (int x = 0; x < mapLayoutLength; x++)
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

        if (x != mapLayoutLength - 1)
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
        Debug.Log(mapLayout.ToDebugLogString("Map layout"));

        //Конвертируем лэйаут пути в улучшенный лэйаут, с учетом стен и прочего.
        mapLayout = ConvertLayout(mapLayout);

        //Выдаем лэйаут в консоль
        Debug.Log(mapLayout.ToDebugLogString("Converted map layout"));


        //Импортируем из джейсона 
        ChunkTemplates.GetFromJson();
        ChunkTemplates.GetObstaclesFromJson();

        //create big map template, //!fill it with zeros
        int mapHeight = mapLayout.Length * ChunkTemplates.chunkHeight;
        int mapkWidth = mapLayoutLength * ChunkTemplates.chunkWidth;

        mapTemplate = new int[mapHeight][];
        for (int y = 0; y < mapHeight; y++)
        {
            mapTemplate[y] = new int[mapkWidth];
            for (int x = 0; x < mapkWidth; x++)
            {
                mapTemplate[y][x] = 0;
            }
        }

        //write all into mapTemplate
        for (int y = 0; y < mapLayout.Length; y++)
        {
            for (int x = 0; x < mapLayoutLength; x++)
            {
                int [][] templateMatrix = GenerateRandomByType(mapLayout[y][x]);
                for (int yM = 0; yM < templateMatrix.Length; yM++)
                {
                    for (int xM = 0; xM < templateMatrix[y].Length; xM++)
                    {


                        //if not obstacle space
                        if (templateMatrix[yM][xM]!=13)
                            mapTemplate[y * ChunkTemplates.chunkHeight + yM][x * ChunkTemplates.chunkWidth + xM] = templateMatrix[yM][xM];
                        //if obstacle
                        if (templateMatrix[yM][xM] == 12)
                        {
                            int[][] obstacleMatrix = ChunkTemplates.obstacleTemplatesContainer.templates.OrderBy(n => Random.value).FirstOrDefault().GetMatrix();
                            for (int yO = 0; yO < ChunkTemplates.obstacleHeight; yO++)
                            {
                                for (int xO = 0; xO < ChunkTemplates.obstacleWidth; xO++)
                                {
                                    mapTemplate[y * ChunkTemplates.chunkHeight + yM+yO][x * ChunkTemplates.chunkWidth + xM+xO] = obstacleMatrix[yO][xO];
                                }
                            }
                        }
                        
                    }
                }
            }
        }

        //convert from BlockPlacerMatrix into BlockLibraryMatrix
        for (int y = 0; y < mapTemplate.Length; y++)
        {
            for (int x = 0; x < mapTemplate[y].Length; x++)
            {
                int type = mapTemplate[y][x];
                
                if (type == 0)
                    continue;
                bool flag = false;
                foreach (BlockPlacer.BlockChance blockChance in BlockPlacer.instance.blocks[type - 1].prefabs)
                {
                    if (Random.Range(0, blockChance.divider) == 0)
                    {
                        mapTemplate[y][x] = blockChance.blockLibraryId;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                    mapTemplate[y][x] = 0;
            }
        }

        Debug.Log(mapTemplate.ToDebugLogString("Converted to BlockLibraryMatrix map template"));


        //Set monsters
        for (int y = 1; y < mapTemplate.Length-1; y++)
        {
            for (int x = 1; x < mapTemplate[y].Length - 1; x++)
            {

                if (mapTemplate[y][x] == 0 && mapTemplate[y][x - 1] == 0 && mapTemplate[y][x + 1] == 0 && mapTemplate[y + 1][x] == 1 && mapTemplate[y + 1][x - 1] == 1 && mapTemplate[y + 1][x + 1] == 1)
                {
                    Debug.Log("Found place for enemy to spawn!!");

                    if (Random.Range(0, 10) == 0)
                        mapTemplate[y][x] = 12; //charging skeleton
                    else
                        if (Random.Range(0, 5) == 0)
                            mapTemplate[y][x] = 2;//regular skeleton
                }
                if (mapTemplate[y][x]     == 0 && mapTemplate[y + 1][x] == 1 &&  mapTemplate[y - 1][x] == 1 && 
                  ((mapTemplate[y][x - 1] == 1 && mapTemplate[y][x + 1] == 0)|| (mapTemplate[y][x - 1] == 0 && mapTemplate[y][x + 1] == 1)))
                {
                    if (Random.Range(0, 3) == 0)
                        mapTemplate[y][x] = 5;
                }

            }
        }



        //spawn blocks
        for (int y = 0; y < mapTemplate.Length; y++)
        {
            for (int x = 0; x < mapTemplate[y].Length; x++)
            {
                SpawnBlock(x, y, mapTemplate[y][x]);
            }
        }

        PrepareColliders();

    }

    void SpawnBlock(int x, int y, int type)
    {
        if (type == 0)
            return;

        //Debug.Log(type);
        GameObject block = Instantiate(BlockLibrary.instance.blocks.Where(i => i.id == type).FirstOrDefault().prefab, this.transform, false);
        block.transform.localPosition = new Vector3(x, -y);
        if (block.GetComponent<Box>())
        {
            block.GetComponent<Box>().AssignSprite(x, y);
            SendBoxColliderCoordinates(x, y);
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


    public int[][] GenerateRandomByType(int ttype)
    {
        //1 left right; 2 up down; 3 up left; 4 down left; 5 up rigth; 6 down right;
        //7 комната спауна, 8 комната выхода
        ChunkTemplates.Template template;

        //по умолчанию (ttype == 0) это может быть любая комната, кроме стартовой и конечной
        template = ChunkTemplates.templatesContainer.templates.Where(i => i.ttype != 4 && i.ttype != 5).OrderBy(n => UnityEngine.Random.value).FirstOrDefault();

        //start room
        if (ttype == 7)
            template = ChunkTemplates.templatesContainer.templates.Where(i => i.ttype == 4).OrderBy(n => UnityEngine.Random.value).FirstOrDefault(); //use whatever you prefer for random
        //exit room
        if (ttype == 8)
            template = ChunkTemplates.templatesContainer.templates.Where(i => i.ttype == 5).OrderBy(n => UnityEngine.Random.value).FirstOrDefault();

        if (ttype == 1)
            template = ChunkTemplates.templatesContainer.templates.Where(i => i.leftExit == true && i.rightExit == true && i.ttype != 4 && i.ttype != 5).OrderBy(n => UnityEngine.Random.value).FirstOrDefault(); //use whatever you prefer for random
        if (ttype == 2)
            template = ChunkTemplates.templatesContainer.templates.Where(i => i.topExit == true && i.bottomExit == true && i.ttype != 4 && i.ttype != 5).OrderBy(n => UnityEngine.Random.value).FirstOrDefault();
        if (ttype == 3)
            template = ChunkTemplates.templatesContainer.templates.Where(i => i.topExit == true && i.leftExit == true && i.ttype != 4 && i.ttype != 5).OrderBy(n => UnityEngine.Random.value).FirstOrDefault(); //use whatever you prefer for random
        if (ttype == 4)
            template = ChunkTemplates.templatesContainer.templates.Where(i => i.bottomExit == true && i.leftExit == true && i.ttype != 4 && i.ttype != 5).OrderBy(n => UnityEngine.Random.value).FirstOrDefault();
        if (ttype == 5)
            template = ChunkTemplates.templatesContainer.templates.Where(i => i.topExit == true && i.rightExit == true && i.ttype != 4 && i.ttype != 5).OrderBy(n => UnityEngine.Random.value).FirstOrDefault(); //use whatever you prefer for random
        if (ttype == 6)
            template = ChunkTemplates.templatesContainer.templates.Where(i => i.bottomExit == true && i.rightExit == true && i.ttype != 4 && i.ttype != 5).OrderBy(n => UnityEngine.Random.value).FirstOrDefault();


        return template.GetMatrix();
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


    void PrepareColliders()
    {
        //new Vector2(-0.5f, 0.5f),  верх лево 
        //new Vector2(32 * mapLength + 0.5f, +0.5f), верх право
        //new Vector2(32 * mapLength + 0.5f, -19 - 0.5f), низ право
        //new Vector2(-0.5f, -19 - 0.5f) }; низ лево


        //up
        List<Vector2> coords = new List<Vector2>() {        new Vector2(- 3f, 0.5f),
                                                                new Vector2(ChunkTemplates.chunkWidth*mapLayoutLength + 3f, 0.5f),
                                                                new Vector2(ChunkTemplates.chunkWidth*mapLayoutLength + 3f, 3f),
                                                                new Vector2(- 3f, 3f) };
        //left
        List<Vector2> coords2 = new List<Vector2>() {       new Vector2(- 0.5f, 0.5f),
                                                                new Vector2(-3f,    0.5f),
                                                                new Vector2(-3f,    -ChunkTemplates.chunkHeight*mapLayout.Length + 0.5f),
                                                                new Vector2(- 0.5f, -ChunkTemplates.chunkHeight*mapLayout.Length + 0.5f) };
        //down
        List<Vector2> coords3 = new List<Vector2>() {       new Vector2(- 3f,                                     -ChunkTemplates.chunkHeight*mapLayout.Length - 3),
                                                                new Vector2(ChunkTemplates.chunkWidth*mapLayoutLength + 3f, -ChunkTemplates.chunkHeight*mapLayout.Length - 3),
                                                                new Vector2(ChunkTemplates.chunkWidth*mapLayoutLength + 3f, -ChunkTemplates.chunkHeight*mapLayout.Length + 0.5f),
                                                                new Vector2(- 3f,                                     -ChunkTemplates.chunkHeight*mapLayout.Length + 0.5f) };
        //right
        List<Vector2> coords4 = new List<Vector2>() {       new Vector2(ChunkTemplates.chunkWidth*mapLayoutLength + 3,     0.5f),
                                                                new Vector2(ChunkTemplates.chunkWidth*mapLayoutLength - 0.5f,  0.5f),
                                                                new Vector2(ChunkTemplates.chunkWidth*mapLayoutLength - 0.5f,  -ChunkTemplates.chunkHeight*mapLayout.Length - 3),
                                                                new Vector2(ChunkTemplates.chunkWidth*mapLayoutLength + 3,     -ChunkTemplates.chunkHeight*mapLayout.Length - 3) };

        colliderCoordinates.Add(coords);
        colliderCoordinates.Add(coords2);
        colliderCoordinates.Add(coords3);
        colliderCoordinates.Add(coords4);

        CreateLevelCollider(UniteCollisionPolygons(colliderCoordinates));
    }


}
