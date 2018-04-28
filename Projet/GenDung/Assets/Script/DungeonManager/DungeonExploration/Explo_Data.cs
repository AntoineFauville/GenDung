using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explo_Data : MonoBehaviour {

    public int exploGold = 0;
	public DungeonData dungeonData;

	int randOfEnemies;
	int enemyRand;

	public int amountOfFightRoom;

	public int amountOfPlayerLeft; //pour que le montant de départ des joueurs soit toujours bien initialisé au début de chaque combat

	public int roomImOn; // sended by the tile when walked to know which room i walked on

	// Use this for initialization
	public void StartEverything () {


		//returns the amount of fight room in the dungeon.
		amountOfFightRoom = dungeonData.amountOfFightRoomsInData;

		print ("total amount of rooms : " + amountOfFightRoom);


		//initialise the data value for the dungeon.
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
	}


	public void SoftStart () {

		StartEverything ();

		//start a french linking battle system
		ClearDataTemporaryCharacter();

	}

	public void LaunchFightFreshStart(){
		GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().ResetFightStart (roomImOn);
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
			dungeonData.TempFighterObject [i].died = false;
		}
	}

	public void EnemyCalculEndDungeon(EnemyObject enemy){
		
		GameObject enemyPanelUI;

		enemyPanelUI = Instantiate(Resources.Load("UI_Interface/EnemiesPanelUI"), GameObject.Find ("CanvasEndExplo/PanelEnemiesEnd").transform) as GameObject;

		enemyPanelUI.transform.Find ("IconMask/Icon").GetComponent<Image> ().sprite = enemy.enemyIcon;

		if (enemy.hasAnimation) {
			enemyPanelUI.transform.Find ("IconMask/Icon").GetComponent<Animator> ().runtimeAnimatorController = enemy.enemyAnimator;
		}

		ModifyGold (enemy.enemyGoldValue);
	}
}
