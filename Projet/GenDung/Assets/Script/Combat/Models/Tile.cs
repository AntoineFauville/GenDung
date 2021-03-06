﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

	public enum TileType
    {
        Floor,Wall,Occupied
    }

    public enum TileState
    {
        Neutral,Movement,Range,Wall,Spawn,MonsterSpawn
    }

    TileType type = TileType.Floor;
    public int x;
    public int y;
    public bool isWalkable = true;
    public bool isStarterTile = false;
    public bool isMonsterTile = false;
    public float movementCost = 1;
    public TileState state = TileState.Neutral;
    Grid grid;

    public Tile(Grid _dungeon, int _x, int _y)
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
