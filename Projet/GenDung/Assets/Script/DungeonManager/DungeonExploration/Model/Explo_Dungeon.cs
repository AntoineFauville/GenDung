using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_Dungeon {

    Explo_Grid grid;
    Explo_Data data;

    public Explo_Dungeon(Explo_Grid _grid, Explo_Data _data)
    {
        this.grid = _grid;
        this.data = _data;
    }

    public Explo_Grid Grid
    {
        get
        {
            return grid;
        }
        set
        {
            grid = value;
        }
    }
    public Explo_Data Data
    {
        get
        {
            return data;
        }
        set
        {
            data = value;
        }
    }
}
