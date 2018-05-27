using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionSetUpDungeon : MonoBehaviour {

	public Transform dungeon_Container;

	public DungeonButtonController prefabDungeon;

	ProgressionSystem ProgressionSystem;

	public PGameData PGameData;

	public DungeonFactory DungeonFactory;


	public void InitializeDungeon(ProgressionSystem ProgressionSystem){

		ProgressionSystem = ProgressionSystem;

		for (int i = 1; i <= PGameData.AmountOfDungeon; i++) {

			DungeonButtonController dungeon;
			dungeon = Instantiate (prefabDungeon, dungeon_Container);
			dungeon.transform.SetParent (dungeon_Container);
			dungeon.name = "Dungeon_" + i;
			dungeon.progressionSystem = ProgressionSystem;

			Pdungeon pDungeon = DungeonFactory.CreateDungeon (Random.Range (0, 1));

			pDungeon.Name = "Dungeon_" + i;
			pDungeon.Index = i-1;

			//pDungeon.Difficulty.SetValueTo (i*i*2);
			pDungeon.Rewards.SetValueTo (i*2);
			//print(pDungeon.Rewards.Value);

			ProgressionSystem.all_Dungeons_Controllers.Add(dungeon);

			dungeon.DungeonDescriptionText.text = dungeon.name + " Difficulty " + pDungeon.Difficulty.Value + " Reward : " + pDungeon.Rewards.Value;
			dungeon.LocalPDungeon = pDungeon;
		}
	}
}
