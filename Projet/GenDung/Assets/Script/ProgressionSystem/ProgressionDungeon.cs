public class ProgressionDungeon
{
    public string Name;
    public ValueSystem Difficulty;
    public ValueSystem Rewards;

    public ProgressionDungeon(string name, int difficulty, int rewards)
    {
        Name = name;
        Difficulty = new ValueSystem(difficulty);
        Rewards = new ValueSystem(rewards);
    }
}
