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
    private List<Vector2> emptyTilesList = new List<Vector2>();
    private List<Vector2> eeTilesList = new List<Vector2>();
    private List<Vector2> fightRoomList = new List<Vector2>();
    private List<Vector2> treasureRoomList = new List<Vector2>();
    private Vector2 entranceTile;
    private int rnd;

	LogGestionTool
	logT;

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

		logT = GameObject.Find ("DontDestroyOnLoad").GetComponent<LogGestionTool> ();


        /* Charge le prefab du Joueur */
        GameObject unit_go = Instantiate(Resources.Load("Prefab/ExploUnit")) as GameObject;
        /* */

        if (SceneManager.GetActiveScene().name != "Editor") // Check si la scéne est différente de l'Editeur (juste pour éviter des erreurs).
        {
            unit = unit_go.transform.Find("Unit").GetComponent<ExploUnitController>();

            /* Assure le positionnement hors écran durant la phase de placement */
            unit.SetDefaultSpawn(new Vector3(-1000, -1000, 0));
            worldPosTemp = new Vector3(-1000, -1000, 0);

            StartCoroutine(WaitForSceneLoading());
        }
    }

    public void GenerateMapData()
    {
        Grid = new Explo_Grid();

        /* Creation du GridCanvas */
        GameObject PrefabGrid = Resources.Load("UI_Interface/GridCanvas") as GameObject;
        GameObject c = GameObject.Instantiate(PrefabGrid);
        c.name = "ExploGridCanvas";
        c.GetComponent<Canvas>().sortingOrder = 71;
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

                Grid.ExploTiles[x, y].Type = Explo_Tile.Explo_TileType.Wall;

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
        float cost = 1;

        Explo_Tile target = GetTileAtWorldCoord(new Vector3(targetX, targetY, 0));
        cost = 1;

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

        unit.CurrentPath = currentPath;
    }

    public bool UnitCanEnterTile(int x, int y)
    {
        //return Grid.Tiles[x,y].isWalkable; OLD CODE

        if (Grid.ExploTiles[x, y].Type != Explo_Tile.Explo_TileType.Wall)
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

    /* Indique aux Tiles concernées qu'elles sont des Zones vides */
    public void SetMovementTiles()
    {
        int movementTilesNumber = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().exploDungeonList.explorationDungeons[MapController.Instance.DungeonIndex].movTiles.Count;
        for (int x = 0; x < movementTilesNumber; x++)
        {
            Vector2 tile = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().exploDungeonList.explorationDungeons[MapController.Instance.DungeonIndex].movTiles[x];
            Grid.ExploTiles[Mathf.RoundToInt(tile.x), Mathf.RoundToInt(tile.y)].Type = Explo_Tile.Explo_TileType.Empty;
            emptyTilesList.Add(tile);
            GameObject.Find("ExploGridCanvas/PanelGrid/Tile_" + tile.x + "_" + tile.y).GetComponent<ExploTileController>().UpdateTileUI();
        }
    }
    /* */

    /* Indique aux Tiles concernées qu'elles sont des zones possibles de placement pré-Combat */
    public void SetEETiles()
    {
        // Amount of Possibilities for Entrance-Exit Tiles.
        int spawnPossibilitiesAmount = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().exploDungeonList.explorationDungeons[MapController.Instance.DungeonIndex].eeTiles.Count;

        for (int y = 0; y < spawnPossibilitiesAmount; y++) // Adding every possible tiles in eeTilesList.
        {
            Vector2 tile = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().exploDungeonList.explorationDungeons[MapController.Instance.DungeonIndex].eeTiles[y];
            eeTilesList.Add(tile);
            if (SceneManager.GetActiveScene().name == "ExploEditor")
            {
                Grid.ExploTiles[Mathf.RoundToInt(tile.x), Mathf.RoundToInt(tile.y)].Type = Explo_Tile.Explo_TileType.Entrance;
                GameObject.Find("ExploGridCanvas/PanelGrid/Tile_" + tile.x + "_" + tile.y).GetComponent<ExploTileController>().UpdateTileUI();
            }
        }

        // Always One Entrance and One Exit.
        // Choosing the tile for Entrance.
        int rnd = Random.Range(0, eeTilesList.Count);
        entranceTile = eeTilesList[rnd];
        Grid.ExploTiles[Mathf.RoundToInt(entranceTile.x), Mathf.RoundToInt(entranceTile.y)].Type = Explo_Tile.Explo_TileType.Entrance;
        eeTilesList.Remove(entranceTile);
        GameObject.Find("ExploGridCanvas/PanelGrid/Tile_" + entranceTile.x + "_" + entranceTile.y).GetComponent<ExploTileController>().UpdateTileUI();

        // Choosing the tile for Exit.
        rnd = Random.Range(0, eeTilesList.Count);
        Vector2 exitTile = eeTilesList[rnd];
        Grid.ExploTiles[Mathf.RoundToInt(exitTile.x), Mathf.RoundToInt(exitTile.y)].Type = Explo_Tile.Explo_TileType.Exit;
        eeTilesList.Remove(exitTile);
        GameObject.Find("ExploGridCanvas/PanelGrid/Tile_" + exitTile.x + "_" + exitTile.y).GetComponent<ExploTileController>().UpdateTileUI();

	}
    /* */

    public void SetRandomFightTiles()
    {
		int fightRoomAmount = Random.Range(1,GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().exploDungeonList.explorationDungeons[MapController.Instance.DungeonIndex].fightRoomAmount);

		//send info to dungeon controller data

		GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data> ().dungeonData.amountOfFightRoomsInData = fightRoomAmount;
		GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data> ().SoftStart ();

		//logT.AddLogLine ("Fight room : 0 / " + fightRoomAmount);

        for (int f = 0; f < fightRoomAmount; f++)
        {
            int emptyTilesCount = emptyTilesList.Count;
            int rnd = Random.Range(0, emptyTilesCount);

            Vector2 tile = emptyTilesList[rnd];
            Grid.ExploTiles[Mathf.RoundToInt(tile.x), Mathf.RoundToInt(tile.y)].Type = Explo_Tile.Explo_TileType.Fight;
            fightRoomList.Add(tile);
            emptyTilesList.RemoveAt(rnd);
            GameObject.Find("ExploGridCanvas/PanelGrid/Tile_" + tile.x + "_" + tile.y).GetComponent<ExploTileController>().UpdateTileUI();
        }
    }

    public void SetRandomTreasureTiles()
    {
		//int treasureRoomAmount = Random.Range(1,GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().exploDungeonList.explorationDungeons[MapController.Instance.DungeonIndex].treasureRoomAmount);
		int treasureRoomAmount = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().exploDungeonList.explorationDungeons[MapController.Instance.DungeonIndex].treasureRoomAmount;

		//logT.AddLogLine ("Treasure room : 0 / " + treasureRoomAmount);

        for (int t = 0; t < treasureRoomAmount; t++)
        {
            int emptyTilesCount = emptyTilesList.Count;
            rnd = Random.Range(0, emptyTilesCount);

            Vector2 tile = emptyTilesList[rnd];
            Grid.ExploTiles[Mathf.RoundToInt(tile.x), Mathf.RoundToInt(tile.y)].Type = Explo_Tile.Explo_TileType.Treasure; // This is a FUCKING TREASURE MAN
            treasureRoomList.Add(tile);
            emptyTilesList.RemoveAt(rnd);
            GameObject.Find("ExploGridCanvas/PanelGrid/Tile_" + tile.x + "_" + tile.y).GetComponent<ExploTileController>().UpdateTileUI();
        }
    }

    public IEnumerator WaitForSceneLoading() // Wait For the Scene to be loaded and then load the empty Tiles and Entrance/Exit Tiles
    {
        yield return new WaitForSeconds(0.3f);

        SetMovementTiles();
        SetEETiles();

        unit.TileX = Mathf.RoundToInt(entranceTile.x);
        unit.TileY = Mathf.RoundToInt(entranceTile.y);
        unit.FirstDiscoverDungeon();
       
        SetRandomFightTiles();
        SetRandomTreasureTiles();
    }

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

    public List<Vector2> EmptyTilesList
    {
        get
        {
            return emptyTilesList;
        }
        set
        {
            emptyTilesList = value;
        }
    }

    public Vector2 EntranceTile
    {
        get
        {
            return entranceTile;
        }
        set
        {
            entranceTile = value;
        }
    }
    /* */
}
