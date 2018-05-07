using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_Room_Fight : Explo_Room
{
    int monstersAmount;
    //liste monstres
    bool boss;
    bool overPowered;

    public Explo_Room_Fight()
    {

    }

    public Explo_Room_Fight(bool _boss, bool _overPowered)
    {
        this.boss = _boss;
        this.overPowered = _overPowered;
    }

    private void LoadRoomPreset(string _preset)
    {
        Debug.Log("Loading Room preset");
        // Do shit here
        Debug.Log("Room preset has been loaded");
    }

    public int MonstersAmount
    {
        get
        {
            return monstersAmount;
        }
        set
        {
            monstersAmount = value;
        }
    }
}
