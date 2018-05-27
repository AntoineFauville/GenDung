using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDungeonDesert : Pdungeon {

	//have desert enemies
	//is hard difficult
	//public PDungeonData PDungeonData;
	public int DungeonDifficulty = 3;
	public int DungeonReward = 5;

	private void Start(){
		Difficulty.Value = DungeonDifficulty;
		Debug.Log (DungeonDifficulty);
	}
}
