using UnityEngine;

public class DungeonFactory : MonoBehaviour {

    public ProgressionDungeon CreateDungeon(ProgressionDungeonData data)
    {
        return new ProgressionDungeon(data.Name, data.Difficulty, data.GoldReward);
    }
}
