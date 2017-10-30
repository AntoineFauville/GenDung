using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonController : MonoBehaviour {

    private static DungeonController instance;
    private Dungeon dungeon;
    private Node[,] graph;
    private UnitController unit;

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
        unit_go.transform.position = Vector3.zero;
        unit = unit_go.GetComponent<UnitController>();
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
                tile_canvas.transform.position = new Vector2((26 + x * 50), (26 + y * 50));
                tile_canvas.GetComponent<TileController>().X = x;
                tile_canvas.GetComponent<TileController>().Y = y;
            }
        }
		c.transform.Find ("PanelGrid").transform.localScale = new Vector3 (0.975f,1.03f,0f);

        //Dungeon.Tiles[0, 2].isWalkable = false;

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

        Debug.Log("Node and neighbours has been defined");
    }

    public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY)
    {
        float cost = 0;

        Tile target = GetTileAtWorldCoord(new Vector3(targetX, targetY, 0));
        cost = target.movementCost;

        if (UnitCanEnterTile(targetX,targetY) == false)
        {
            Debug.Log("Can't walk on this shit man");
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

        Debug.Log("Path has been calculated");
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
        int x = (int)Mathf.Floor(coord.x) + 9;
        int y = (int)Mathf.Floor(coord.y) + 5;

        return dungeon.GetTileAt(x, y);
    }

    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        return new Vector3(x, y, 0);
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

    /* */
}
