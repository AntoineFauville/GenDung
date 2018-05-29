using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionDungeon
{
    public string Name;
    public int Index;

    public ValueSystem Difficulty;
    public ValueSystem Rewards;

    public ProgressionDungeon(string name, int index, int difficulty, int rewards)
    {
        Name = name;
        Index = index;
        Difficulty = new ValueSystem(difficulty);
        Rewards = new ValueSystem(rewards);
    }
}
