using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonController : MonoBehaviour {

    private static DungeonController instance;
    private Dungeon dungeon;

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

        Dungeon = new Dungeon();

        for(int x = 0; x < Dungeon.Width; x++)
        {
            for (int y = 0; y < Dungeon.Height; y++)
            {
                Tile tile_data = Dungeon.GetTileAt(x, y);

                GameObject tile_go = new GameObject();

                tile_go.name = "Tile_" + x + "_" + y;

                tile_go.transform.position = new Vector3(tile_data.X, tile_data.Y, 0);

                tile_go.transform.SetParent(this.transform, true); 
            }
        }
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
