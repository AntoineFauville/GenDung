using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_Room {

    int index;
    Explo_Dungeon dungeon;

    public Explo_Room()
    {

    }

    public int Index
    {
        get
        {
            return index;
        }

        set
        {
            index = value;
        }
    }

    public Explo_Dungeon Dungeon
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
}
