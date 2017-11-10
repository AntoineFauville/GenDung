using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DungeonController : MonoBehaviour {

    private static DungeonController instance;
    private Dungeon dungeon;
    private Node[,] graph;
    private UnitController unit;

    private Vector3 worldPosTemp;

    void CreateInstance()
    {
        if (instance != null)
        {
            Debug.Log("There should never have two world controllers.");
        }
        instance = this;
    }

	void Start ()
    {
        CreateInstance();
        GenerateMapData();
        GeneratePathfindingGraph();

        GameObject unit_go = Instantiate(Resources.Load("Prefab/Unit"))as GameObject;

        if (SceneManager.GetActiveScene().name != "Editor") // Check si la scéne est différente de l'Editeur (juste pour éviter des erreurs).
        {
            Vector2 pos = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().roomListDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonIndex].RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.SpawningPoints[0];
            Vector3 startPos = GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + pos.x + "_" + pos.y).transform.position;
            // Ces lignes récupérent la première position dans la liste des SpawnPoints pour placer par défaut le personnage du joueur sur celle-ci.

            unit = unit_go.transform.Find("Unit").GetComponent<UnitController>();
            unit.SetDefaultSpawn(Vector3.zero/*startPos*/); // Positionne le personnage au Vector3 (0,0,0).

            int wallsNumber = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().roomListDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonIndex].RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.Walls.Count;
            Debug.Log("Number of walls in this Room: " + wallsNumber);
            for (int x = 0; x < wallsNumber; x++)
            {
                Vector2 tile = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().roomListDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonIndex].RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.Walls[x];
                Dungeon.Tiles[Mathf.RoundToInt(tile.x), Mathf.RoundToInt(tile.y)].isWalkable = false;
            }

            int spawnNumber = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().roomListDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonIndex].RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.SpawningPoints.Count;
            for(int y=0; y < spawnNumber; y++)
            {
                Vector2 tile = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().roomListDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonIndex].RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.SpawningPoints[y];
                Dungeon.Tiles[Mathf.RoundToInt(tile.x), Mathf.RoundToInt(tile.y)].isStarterTile = true;
            }
        }
	}

    void Update ()
    {
        if (Input.GetButtonUp("Jump"))
        {
            unit.NextTurn();
        }
    }

    public void GenerateMapData()
    {
        Dungeon = new Dungeon();

        /*Creation du GridCanvas*/
        GameObject PrefabGrid = Resources.Load("UI_Interface/GridCanvas") as GameObject;
        GameObject c = GameObject.Instantiate(PrefabGrid);
        /**/

		GameObject tileUIPrefab = Resources.Load("UI_Interface/Tile") as GameObject;

        /* Creation Tile par Tile de la grille représentant le Dungeon */
        for (int x = 0; x < Dungeon.Width; x++)
        {
            for (int y = 0; y < Dungeon.Height; y++)
            {
				GameObject tile_canvas = GameObject.Instantiate(tileUIPrefab,c.transform.Find("PanelGrid"));
                tile_canvas.name = "Tile_" + x + "_" + y;
                tile_canvas.transform.localPosition = 
                    new Vector2(
                         (Screen.currentResolution.width / 14 + x * Screen.currentResolution.width / 60), 
                         (Screen.currentResolution.height / 20 + y * Screen.currentResolution.height / 33.5f)
                    );
                tile_canvas.GetComponent<TileController>().X = x;
                tile_canvas.GetComponent<TileController>().Y = y;

                Dungeon.Tiles[x, y].isWalkable = true;

            }
        }
		c.transform.Find ("PanelGrid").transform.localScale = new Vector3 (0.95f,1,1f);
        c.transform.Find("PanelGrid").transform.localPosition = new Vector3(-Screen.currentResolution.width / 3.732f, -Screen.currentResolution.width / 7.3f, 0);

        /* */
    }

    void GeneratePathfindingGraph()
    {
        // Initialize the array
        graph = new Node[Dungeon.Width, Dungeon.Height];

        // Initialize a Node for each spot in the array
        for (int x = 0; x < Dungeon.Width; x++)
        {
            for (int y = 0; y < Dungeon.Height; y++)
            {
                graph[x, y] = new Node();
                graph[x, y].x = x;
                graph[x, y].y = y;
            }
        }

        // Now that all the nodes exist, calculate their neighbours
        for (int x = 0; x < Dungeon.Width; x++)
        {
            for (int y = 0; y < Dungeon.Height; y++)
            {
                // This is the 8-way connection version (allows diagonal movement)
                // Try left
                if (x > 0)
                {
                    graph[x, y].neighbours.Add(graph[x - 1, y]);
                    if (y > 0)
                        graph[x, y].neighbours.Add(graph[x - 1, y - 1]);
                    if (y < Dungeon.Height - 1)
                        graph[x, y].neighbours.Add(graph[x - 1, y + 1]);
                }

                // Try Right
                if (x < Dungeon.Width - 1)
                {
                    graph[x, y].neighbours.Add(graph[x + 1, y]);
                    if (y > 0)
                        graph[x, y].neighbours.Add(graph[x + 1, y - 1]);
                    if (y < Dungeon.Height - 1)
                        graph[x, y].neighbours.Add(graph[x + 1, y + 1]);
                }

                // Try straight up and down
                if (y > 0)
                    graph[x, y].neighbours.Add(graph[x, y - 1]);
                if (y < Dungeon.Height - 1)
                    graph[x, y].neighbours.Add(graph[x, y + 1]);

            }
        }
    }

    public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY)
    {
        float cost = 0;

        Tile target = GetTileAtWorldCoord(new Vector3(targetX, targetY, 0));
        cost = target.movementCost;

        if (UnitCanEnterTile(targetX,targetY) == false)
        {
            return Mathf.Infinity;
        }

        if (sourceX!=targetX && sourceY!=targetY)
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

        Node source = graph[unit.TileX, unit.TileY];

        Node target = graph[x, y];

        dist[source] = 0;
        prev[source] = null;

        foreach(Node v in graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }

            unvisited.Add(v);
        }

        while(unvisited.Count > 0)
        {
            Node u = null;

            foreach(Node possibleU in unvisited)
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

        while(curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }

        currentPath.Reverse();

        unit.CurrentPath = currentPath;
    }

    public void LaunchUnitAttack(int _x,int _y)
    {
        unit.Attacking = true;
        Debug.Log("LaunchUnitAttack(" + _x + "," + _y + ")");
        unit.setTileAttackX(_x);
        unit.setTileAttackY(_y);
    }

    public bool UnitCanEnterTile(int x, int y)
    {
        return Dungeon.Tiles[x,y].isWalkable;
    }

    public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = (int)Mathf.Floor(coord.x) ;
        int y = (int)Mathf.Floor(coord.y) ;

        return dungeon.GetTileAt(x, y);
    }

    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        return new Vector3(Screen.currentResolution.width / 14 + x * Screen.currentResolution.width / 60, Screen.currentResolution.height / 20 + y * Screen.currentResolution.height / 33.5f, 0);
    }

    public Vector3 TileCoordFromClick(int x, int y)
    {
        return worldPosTemp;
    }

    /* Accessors Methods */

    public static DungeonController Instance
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

    public Dungeon Dungeon
    {
        get
        {
            return dungeon;
        }

        set
        {
            dungeon = value;
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

    public UnitController Unit
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

    /* */
}
