using UnityEngine;

public class ProgressionSetUpDungeon : MonoBehaviour {

	public Transform dungeon_Container;

	public DungeonButtonController prefabDungeon;

	ProgressionSystem ProgressionSystem;

	public ProgressionGameData PGameData;

	public DungeonFactory DungeonFactory;

    public ProgressionDungeonData[] ProgressionDatas;

    public void InitializeDungeon(ProgressionSystem ProgressionSystem){

		for (int i = 1; i <= PGameData.AmountOfDungeon; i++) {

			DungeonButtonController dungeon;
			dungeon = Instantiate (prefabDungeon, dungeon_Container);
			dungeon.transform.SetParent (dungeon_Container);
			dungeon.name = "Dungeon_" + i;
			dungeon.ProgressionSystem = ProgressionSystem;

			dungeon.LocalPDungeon = DungeonFactory.CreateDungeon (ProgressionDatas[Random.Range (0, ProgressionDatas.Length)]);

			ProgressionSystem.AllDungeonsControllers.Add(dungeon);

			dungeon.DungeonDescriptionText.text = dungeon.name + " Difficulty " + dungeon.LocalPDungeon.Difficulty.Value + " Reward : " + dungeon.LocalPDungeon.Rewards.Value;
		}
	}
}
