using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ProgressionSetUpDungeon : MonoBehaviour {

	public Transform dungeon_Container;

	public DungeonButtonController prefabDungeon;

	ProgressionSystem ProgressionSystem;

	public PGameData PGameData;

	public DungeonFactory DungeonFactory;

    public PDungeonData[] ProgressionDatas;

    public void InitializeDungeon(ProgressionSystem ProgressionSystem){

		for (int i = 1; i <= PGameData.AmountOfDungeon; i++) {

			DungeonButtonController dungeon;
			dungeon = Instantiate (prefabDungeon, dungeon_Container);
			dungeon.transform.SetParent (dungeon_Container);
			dungeon.name = "Dungeon_" + i;
			dungeon.ProgressionSystem = ProgressionSystem;

			dungeon.LocalPDungeon = DungeonFactory.CreateDungeon (ProgressionDatas[Random.Range (0, ProgressionDatas.Length)]);
            
            //dungeon.LocalPDungeon.Name = "Dungeon_" + i;
		    //dungeon.LocalPDungeon.Index = i-1;

		    //dungeon.LocalPDungeon.Difficulty.SetValueTo (1);
		    //dungeon.LocalPDungeon.Rewards.SetValueTo (i*2);
			//print(pDungeon.Rewards.Value);

			ProgressionSystem.AllDungeonsControllers.Add(dungeon);

			dungeon.DungeonDescriptionText.text = dungeon.name + " Difficulty " + dungeon.LocalPDungeon.Difficulty.Value + " Reward : " + dungeon.LocalPDungeon.Rewards.Value;
		}
	}
}
