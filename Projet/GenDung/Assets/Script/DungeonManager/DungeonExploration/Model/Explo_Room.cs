using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_Room {

    int index;
    Sprite background;
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

    public Sprite Background
    {
        get
        {
            return background;
        }

        set
        {
            background = value;
        }
    }
}
