using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProgressionSystem", menuName = "ProgressionSystem/PDungeonData", order = 1)]
public class PDungeonData : ScriptableObject
{
    public string Name;
    public int Index;

	public int Difficulty;
	public int GoldReward;
}
