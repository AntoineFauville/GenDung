using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_Room_Treasure : Explo_Room {

    int goldAmount;
    bool trap;

    public Explo_Room_Treasure(Explo_Dungeon _dungeon)
    {
        base.Dungeon = _dungeon;
    }

    public Explo_Room_Treasure (Explo_Dungeon _dungeon, int _goldAmount)
    {
        base.Dungeon = _dungeon;
        this.goldAmount = _goldAmount;
        this.trap = false;
    }

    public void DefineTrap(int _trapPercentage)
    {
        int rand = Random.Range(0, 100);

        if (rand > _trapPercentage)
        {
            // It's a Trap, someone gonna get hurt.
            // Indicate that the room is actually a Trap;
            trap = true;
        }
    }

    public void DefineReward(int _maxReward)
    {
        goldAmount = Random.Range(1, _maxReward);
    }

    public bool Trap
    {
        get
        {
            return trap;
        }

        set
        {
            trap = value;
        }
    }

    public int GoldAmount
    {
        get
        {
            return goldAmount;
        }

        set
        {
            goldAmount = value;
        }
    }
}
