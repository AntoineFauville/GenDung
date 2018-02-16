using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_Grid {

    Explo_Tile[,] exploTiles;
    int width;
    int height;

    public Explo_Grid(int _width = 20, int _height = 14)
    {
        this.Width = _width;
        this.Height = _height;

        exploTiles = new Explo_Tile[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                exploTiles[x, y] = new Explo_Tile(this, x, y);
            }
        }

        Debug.Log("Map has been created with " + (Width * Height) + " tiles.");
    }

    public Explo_Tile GetTileAt(int _x, int _y)
    {
        //Debug.Log("GetTileAt( " + _x + "," + _y + ")");

        if (_x > Width || _x < 0 || _y > Height || _y < 0)
        {
            Debug.LogWarning("Tile (" + _x + "," + _y + ") is out of range.");
            return null;
        }
        return exploTiles[_x, _y];
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
    public Explo_Tile[,] ExploTiles
    {
        get
        {
            return exploTiles;
        }
        set
        {
            exploTiles = value;
        }
    }
    /* */
}
