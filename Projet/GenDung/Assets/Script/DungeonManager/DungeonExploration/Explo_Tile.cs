using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_Tile
{
    public enum Explo_TileType
    {
        Empty, Wall, Fight, Treasure, Trap, OtterKingdom, Exit, Entrance
    }

    Explo_TileType type = Explo_TileType.Empty; // Wall Type by Default
    public int x;
    public int y;
    public bool isWalkable = true;
    public bool isStarterTile = false;
    Explo_Grid grid;

    public Explo_Tile(Explo_Grid _dungeon, int _x, int _y)
    {
        this.grid = _dungeon;
        this.x = _x;
        this.y = _y;
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

    public Explo_TileType Type
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
