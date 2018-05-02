using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_Room_Fight : Explo_Room
{
    private int monstersAmount;
    private bool boss;
    private bool overPowered;

    public Explo_Room_Fight()
    {

    }

    public Explo_Room_Fight(Explo_Tile _tile)
    {
        this.Tile = _tile;
    }

    public Explo_Room_Fight(Explo_Tile _tile, bool _boss, bool _overPowered)
    {
        this.Tile = _tile;
        this.boss = _boss;
    }
}
