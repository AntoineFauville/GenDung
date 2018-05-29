using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonFactory : MonoBehaviour {

    public ProgressionDungeon CreateDungeon(ProgressionDungeonData data)
    {
        return new ProgressionDungeon(data.Name, data.Difficulty, data.GoldReward);
    }
}
