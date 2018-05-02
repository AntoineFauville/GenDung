using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_Room {

    Explo_Tile tile;

    public enum roomType { Empty, Fight, Treasure };

    public Explo_Room()
    {

    }

	public Explo_Room(Explo_Tile _tile)
    {
        this.tile = _tile;
    }

    public Explo_Tile Tile
    {
        get
        {
            return tile;
        }
        set
        {
            tile = value;
        }
    }
}
