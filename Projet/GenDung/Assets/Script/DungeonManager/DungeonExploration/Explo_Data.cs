using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_Data : MonoBehaviour {

    public int exploGold = 0;
	public DungeonData dungeonData;

	int randOfEnemies;
	int enemyRand;

	public int amountOfFightRoom;

	// Use this for initialization
	public void SoftStart () {

		amountOfFightRoom = dungeonData.amountOfFightRoomsInData;

		print ("total amount of rooms : " + amountOfFightRoom);

		for (int i = 0; i < amountOfFightRoom; i++) {

			dungeonData.RoomData.Add (new RoomData());

			int dungeon = GameObject.Find ("DontDestroyOnLoad").GetComponent<MapController> ().dungeonIndex;
			randOfEnemies = Random.Range (1, GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons [dungeon].enemyMax);

			print ("room " + i + " has " + randOfEnemies + " enemies");

			for (int j = 0; j < randOfEnemies; j++) {

				GetRandEnemies (i, dungeon);

				print ("room " + i + " and contains the enemy : " + GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons [dungeon].enemiesList [enemyRand]);
			}
		}

		//start a french linking battle system
		ClearDataTemporaryCharacter();
		GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FreshStart ();
	}

	void GetRandEnemies (int i, int dungeon)
	{
		enemyRand = Random.Range (0, GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons [dungeon].enemiesList.Count);

		dungeonData.RoomData [i].enemyInRoom.Add (GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons [dungeon].enemiesList [enemyRand]);
	}

    public void ModifyGold(int value)
    {
        exploGold += value;
    }

    public void SendToSave()
    {
        GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.PlayerMoney += exploGold;
    }

	void ClearDataTemporaryCharacter(){
		for (int i = 0; i < 4; i++) {
			dungeonData.characterObject [i].died = false;
		}
	}
}
