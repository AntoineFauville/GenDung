using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Explo_GridController : MonoBehaviour {


    private static Explo_GridController instance;
    private Explo_Grid grid;
    private Node[,] graph;
    private ExploUnitController unit;
    private Vector3 worldPosTemp;
    private List<Vector2> spawnTilesList = new List<Vector2>();

    void CreateInstance()
    {
        if (instance != null)
        {
            Debug.Log("There should never have two world controllers.");
        }
        instance = this;
    }

    void Start()
    {
        CreateInstance();
        GenerateMapData();
        GeneratePathfindingGraph();


        /* Charge le prefab du Joueur */
        GameObject unit_go = Instantiate(Resources.Load("Prefab/ExploUnit")) as GameObject;
        /* */

        if (SceneManager.GetActiveScene().name != "Editor") // Check si la scéne est différente de l'Editeur (juste pour éviter des erreurs).
        {
            unit = unit_go.transform.Find("Unit").GetComponent<ExploUnitController>();

            /* Assure le positionnement hors écran durant la phase de placement */
            unit.SetDefaultSpawn(new Vector3(-1000, -1000, 0));
            worldPosTemp = new Vector3(-1000, -1000, 0);
            /* */

            /*
            SetWallTiles();
            SetSpawnTiles();
            SetMonsterSpawnTiles();
            */
        }
    }

    public void GenerateMapData()
    {
        Grid = new Explo_Grid();

        /* Creation du GridCanvas */
        GameObject PrefabGrid = Resources.Load("UI_Interface/GridCanvas") as GameObject;
        GameObject c = GameObject.Instantiate(PrefabGrid);
        /* */

        // Load du Prefab de la Tile // 
        GameObject tileUIPrefab = Resources.Load("UI_Interface/ExploTile") as GameObject;

        /* Creation Tile par Tile de la grille représentant le Grid */
        for (int x = 0; x < Grid.Width; x++)
        {
            for (int y = 0; y < Grid.Height; y++)
            {
                GameObject tile_canvas = GameObject.Instantiate(tileUIPrefab, c.transform.Find("PanelGrid"));
                tile_canvas.name = "Tile_" + x + "_" + y;
                tile_canvas.transform.localPosition =
                    new Vector2(
                         (Screen.currentResolution.width / 14 + x * Screen.currentResolution.width / 60),
                         (Screen.currentResolution.height / 20 + y * Screen.currentResolution.height / 33.5f)
                    );
                tile_canvas.GetComponent<ExploTileController>().X = x;
                tile_canvas.GetComponent<ExploTileController>().Y = y;

                Grid.ExploTiles[x, y].isWalkable = true;

                Grid.ExploTiles[x, y].Type = Explo_Tile.Explo_TileType.Empty; //Explo_Tile.Explo_TileType.Wall;

            }
        }
        c.transform.Find("PanelGrid").transform.localScale = new Vector3(0.95f, 1, 1f);
        c.transform.Find("PanelGrid").transform.localPosition = new Vector3(-Screen.currentResolution.width / 3.732f, -Screen.currentResolution.width / 7.3f, 0);
        /* */
    }

    void GeneratePathfindingGraph()
    {
        // Initialisation du Array
        graph = new Node[Grid.Width, Grid.Height];

        // Initialize a Node for each spot in the array
        for (int x = 0; x < Grid.Width; x++)
        {
            for (int y = 0; y < Grid.Height; y++)
            {
                graph[x, y] = new Node();
                graph[x, y].x = x;
                graph[x, y].y = y;
            }
        }

        // Now that all the nodes exist, calculate their neighbours
        for (int x = 0; x < Grid.Width; x++)
        {
            for (int y = 0; y < Grid.Height; y++)
            {
                // This is the 4-way connection version:
                if (x > 0)
                    graph[x, y].neighbours.Add(graph[x - 1, y]);
                if (x < Grid.Width - 1)
                    graph[x, y].neighbours.Add(graph[x + 1, y]);
                if (y > 0)
                    graph[x, y].neighbours.Add(graph[x, y - 1]);
                if (y < Grid.Height - 1)
                    graph[x, y].neighbours.Add(graph[x, y + 1]);

                /*
                // This is the 8-way connection version (allows diagonal movement)
                // Try left
                if (x > 0)
                {
                    graph[x, y].neighbours.Add(graph[x - 1, y]);
                    if (y > 0)
                        graph[x, y].neighbours.Add(graph[x - 1, y - 1]);
                    if (y < Grid.Height - 1)
                        graph[x, y].neighbours.Add(graph[x - 1, y + 1]);
                }

                // Try Right
                if (x < Grid.Width - 1)
                {
                    graph[x, y].neighbours.Add(graph[x + 1, y]);
                    if (y > 0)
                        graph[x, y].neighbours.Add(graph[x + 1, y - 1]);
                    if (y < Grid.Height - 1)
                        graph[x, y].neighbours.Add(graph[x + 1, y + 1]);
                }

                // Try straight up and down
                if (y > 0)
                    graph[x, y].neighbours.Add(graph[x, y - 1]);
                if (y < Grid.Height - 1)
                    graph[x, y].neighbours.Add(graph[x, y + 1]);
                */
            }
        }
    }

    public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY)
    {
        float cost = 0;

        Explo_Tile target = GetTileAtWorldCoord(new Vector3(targetX, targetY, 0));
        cost = 0;

        if (UnitCanEnterTile(targetX, targetY) == false)
        {
            return Mathf.Infinity;
        }

        if (sourceX != targetX && sourceY != targetY)
        {
            cost += 0.001f;
        }

        return cost;
    }

    public void GeneratePathTo(int x, int y)
    {
        unit.CurrentPath = null;

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        List<Node> unvisited = new List<Node>();

        Node source;

        /*if (!PreCombatController.Instance.CombatStarted)
            source = graph[unit.TileX, unit.TileY];
        else if (CombatController.Instance.Turn == CombatController.turnType.Player)
            source = graph[CombatController.Instance.TargetUnit.TileX, CombatController.Instance.TargetUnit.TileY];
        else
            source = graph[CombatController.Instance.TargetFoe.TileX, CombatController.Instance.TargetFoe.TileY];
            */

        source = graph[unit.TileX, unit.TileY];

        Node target = graph[x, y];

        dist[source] = 0;
        prev[source] = null;

        foreach (Node v in graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }

            unvisited.Add(v);
        }

        while (unvisited.Count > 0)
        {
            Node u = null;

            foreach (Node possibleU in unvisited)
            {
                if (u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            if (u == target)
            {
                break;
            }
            unvisited.Remove(u);

            foreach (Node v in u.neighbours)
            {
                float alt = dist[u] + CostToEnterTile(u.x, u.y, v.x, v.y);
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        if (prev[target] == null)
        {
            return;
        }

        List<Node> currentPath = new List<Node>();

        Node curr = target;

        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }

        currentPath.Reverse();

        /*
        if (!PreCombatController.Instance.CombatStarted)
            unit.CurrentPath = currentPath;
        else if (CombatController.Instance.Turn == CombatController.turnType.Player)
            CombatController.Instance.TargetUnit.CurrentPath = currentPath;
        else
            CombatController.Instance.TargetFoe.CurrentPath = currentPath;
        */

        unit.CurrentPath = currentPath;
    }

    public bool UnitCanEnterTile(int x, int y)
    {
        //return Grid.Tiles[x,y].isWalkable; OLD CODE

        if (Grid.ExploTiles[x, y].Type == Explo_Tile.Explo_TileType.Empty)
            return true;
        else
            return false;
    }

    public Explo_Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = (int)Mathf.Floor(coord.x);
        int y = (int)Mathf.Floor(coord.y);

        return grid.GetTileAt(x, y);
    }

    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        return new Vector3(Screen.currentResolution.width / 14 + x * Screen.currentResolution.width / 60, Screen.currentResolution.height / 20 + y * Screen.currentResolution.height / 33.5f, 0);
    }

    public Vector3 TileCoordFromClick(int x, int y)
    {
        return worldPosTemp;
    }

    /* Indique aux Tiles concernées qu'elles sont des murs */
    /*
    public void SetWallTiles()
    {
        int wallsNumber = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.Walls.Count;
        for (int x = 0; x < wallsNumber; x++)
        {
            Vector2 tile = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.Walls[x];
            Grid.ExploTiles[Mathf.RoundToInt(tile.x), Mathf.RoundToInt(tile.y)].isWalkable = false;
            Grid.ExploTiles[Mathf.RoundToInt(tile.x), Mathf.RoundToInt(tile.y)].Type = Explo_Tile.Explo_TileType.Wall;
        }
    }
    */
    /* */

    /* Indique aux Tiles concernées qu'elles sont des zones possibles de placement pré-Combat */
    /*
    public void SetSpawnTiles()
    {
        int spawnNumber = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.SpawningPoints.Count;
        for (int y = 0; y < spawnNumber; y++)
        {
            Vector2 tile = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.SpawningPoints[y];
            Grid.ExploTiles[Mathf.RoundToInt(tile.x), Mathf.RoundToInt(tile.y)].isStarterTile = true;
            spawnTilesList.Add(tile);
        }
    }
    */
    /* */

    /* Indique aux Tiles concernées qu'elles sont des zones de spawn possibles de monstres */
    /*
    public void SetMonsterSpawnTiles()
    {
        int spawnMonsterNumber = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.MonsterSpawningPoints.Count;
        for (int z = 0; z < spawnMonsterNumber; z++)
        {
            Vector2 tile = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.MonsterSpawningPoints[z];
            Grid.ExploTiles[Mathf.RoundToInt(tile.x), Mathf.RoundToInt(tile.y)].Type = Explo_Tile.Explo_TileType.Fight;
        }
    }
    */
    /* */

    /* Accessors Methods */

    public static Explo_GridController Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    public Explo_Grid Grid
    {
        get
        {
            return grid;
        }

        set
        {
            grid = value;
        }
    }

    public Vector3 WorldPosTemp
    {
        get
        {
            return worldPosTemp;
        }

        set
        {
            worldPosTemp = value;
        }
    }

    public ExploUnitController Unit
    {
        get
        {
            return unit;
        }
        set
        {
            unit = value;
        }
    }

    public List<Vector2> SpawnTilesList
    {
        get
        {
            return spawnTilesList;
        }
        set
        {
            spawnTilesList = value;
        }
    }
    /* */
}
