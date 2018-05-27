using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDungeonForest : Pdungeon {

	//have forest enemies
	//is hard difficult
	//public PDungeonData PDungeonData;
	public int DungeonDifficulty = 1;
	public int DungeonReward = 3;

	private void Start(){
		Difficulty.Value = DungeonDifficulty;
		Debug.Log (DungeonDifficulty);
	}
}
