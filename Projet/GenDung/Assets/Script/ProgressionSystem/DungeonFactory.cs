using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonFactory : MonoBehaviour {

    public ProgressionDungeon CreateDungeon(PDungeonData data)
    {
        return new ProgressionDungeon(data.Name, data.Index, data.Difficulty, data.GoldReward);
    }
}
