using UnityEngine;

[CreateAssetMenu(fileName = "ProgressionSystem", menuName = "ProgressionSystem/ProgressionDungeonData", order = 1)]
public class ProgressionDungeonData : ScriptableObject
{
    public string Name;
	public int Difficulty;
	public int GoldReward;
}
