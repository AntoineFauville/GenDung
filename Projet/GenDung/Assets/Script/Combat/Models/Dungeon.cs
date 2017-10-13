using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon {

    Tile[,] tiles;
    int width;
    int height;

    public Dungeon(int _width = 10, int _height = 10)
    {
        this.Width = _width;
        this.Height = _height;

        tiles = new Tile[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                tiles[x, y] = new Tile(this, x, y);
            }
        }

        Debug.Log("Map has been created with " + (Width * Height) + " tiles.");
    }

    public Tile GetTileAt(int _x, int _y)
    {
        if (_x > Width || _x < 0 || _y > Height || _y < 0)
        {
            Debug.LogWarning("Tile (" + _x + "," + _y + ") is out of range.");
            return null;
        }
        return tiles[_x, _y];
    }

    /* Accessors Methods */

    public int Width
    {
        get
        {
            return width;
        }
        
        set
        {
            width = value;
        }
    }

    public int Height
    {
        get
        {
            return height;
        }

        set
        {
            height = value;
        }
    }
    public Tile[,] Tiles
    {
        get
        {
            return tiles;
        }
        set
        {
            tiles = value;
        }
    }

    /* */
}
