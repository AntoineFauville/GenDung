using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour {

    private static DungeonController instance;
    private Dungeon dungeon;
    private Node[,] graph;
    private Sprite floor = null;

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
        LoadSprites();
        GenerateMapData();
        GeneratePathfindingGraph();
	}

    public void GenerateMapData()
    {
        Dungeon = new Dungeon();

        /* Creation d'un GO (GameObject) vide servant de parent */
        GameObject parent = new GameObject();
        parent.name = "Dungeon";
        parent.transform.position = Vector3.zero;
        /**/

        /* Creation Tile par Tile de la grille représentant le Dungeon */
        for (int x = 0; x < Dungeon.Width; x++)
        {
            for (int y = 0; y < Dungeon.Height; y++)
            {
                Tile tile_data = Dungeon.GetTileAt(x, y);
                GameObject tile_go = new GameObject();
                tile_go.name = "Tile_" + x + "_" + y;
                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y, 0);
                tile_go.transform.localScale = new Vector3(2, 2, 2);
                tile_go.transform.SetParent(parent.transform, true);

                /* Ajoute un SpriteRenderer à chaque Tile et lui assigne le sol par défaut */
                SpriteRenderer tile_sr = tile_go.AddComponent<SpriteRenderer>();
                tile_sr.sprite = floor;
                /* */
            }
        }
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
            for (int y = 0; y < Dungeon.Width; y++)
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

    public void LoadSprites()
    {
        floor = Resources.Load<Sprite>("Sprites/Floor1");
    }

    public Tile GetTileAtWorldCoord(Vector3 coord)
    {
        int x = (int)Mathf.Floor(coord.x);
        int y = (int)Mathf.Floor(coord.y);

        return dungeon.GetTileAt(x, y);
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
