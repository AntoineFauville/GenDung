using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

	public enum TileType
    {
        Floor,Wall,
    }

    TileType type = TileType.Floor;
    int x;
    int y;
    public bool isWalkable = true;
    public float movementCost = 1;
    Dungeon dungeon;

    public Tile(Dungeon _dungeon, int _x, int _y)
    {
        this.dungeon = _dungeon;
        this.X = _x;
        this.Y = _y;
    }

    /* Accessors Methods */

    public int X
    {
        get
        {
            return x;
        }

        set
        {
            x = value;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }
        
        set
        {
            y = value;
        }
    }

    public TileType Type
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
        }
    }
    /* */
}
