using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_Room_Treasure : Explo_Room {

    int goldAmount;
    bool trap;
    int randomTarget;

    public Explo_Room_Treasure()
    {

    }

    public Explo_Room_Treasure (int _goldAmount)
    {
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
            // Taking a random target for damage.
            randomTarget = Random.Range(0, 3);
        }
    }
}
