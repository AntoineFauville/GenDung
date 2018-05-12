using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySetup_Controller : MonoBehaviour {

	[Header("Enemies")]
	public string enemyString = "Enemy ";
	public int rndAttackEnemy;
	MapController map_Controller;

	BattleSystem BS;
	Explo_DataController explo_Data;
	Initiative_Controller initiative_Controller;

	GameObject scriptBattleHolder;
	GameObject dontDestroyOnLoad;

	// Use this for initialization
	void Start () {
		dontDestroyOnLoad = GameObject.Find ("DontDestroyOnLoad");
		scriptBattleHolder = GameObject.Find ("BattleSystem/ScriptBattle");

		BS = scriptBattleHolder.GetComponent<BattleSystem> ();
		explo_Data = dontDestroyOnLoad.GetComponent<Explo_DataController> ();
		map_Controller = dontDestroyOnLoad.GetComponent<MapController> ();
		initiative_Controller = scriptBattleHolder.GetComponent<Initiative_Controller> ();
	}
	
	public void SetupEnemies(int roomNumber)
	{
		BS.amountOfEnemies =  explo_Data.dungeonData.RoomData[roomNumber].enemyInRoom.Count;
		BS.amountOfEnemiesLeft = BS.amountOfEnemies;

		print (BS.amountOfEnemiesLeft + " est le montant d'enemi left et " + BS.amountOfEnemies + " le montant d'enemi"); 

		for (int i = 0; i < BS.amountOfEnemies; i++) {

			if (SceneManager.GetActiveScene ().name != "NewCombatTest") 
			{
				int dungeon = map_Controller.dungeonIndex;

				EnemyObject enemy = explo_Data.dungeonData.RoomData [roomNumber].enemyInRoom [i];

				dungeon = map_Controller.dungeonIndex;
				enemy = explo_Data.dungeonData.RoomData [roomNumber].enemyInRoom [i];
				initiative_Controller.UnOrderedFighterList.Add (GameObject.Find (enemyString + i), enemy.initiative);
				GameObject.Find (enemyString + i).GetComponent<LocalDataHolder> ().enemyObject = enemy;
			} 
			else 
			{
				EnemyObject enemy = explo_Data.dungeonData.RoomData [roomNumber].enemyInRoom [i];

				enemy = explo_Data.dungeonData.RoomData [roomNumber].enemyInRoom [i];
				initiative_Controller.UnOrderedFighterList.Add (GameObject.Find (enemyString + i), enemy.initiative);
				GameObject.Find (enemyString + i).GetComponent<LocalDataHolder> ().enemyObject = enemy;
			}

			GameObject.Find (enemyString + i).GetComponent<LocalDataHolder> ().localIndex = i;

			if (GameObject.Find (enemyString + i).GetComponent<LocalDataHolder> ().enemyObject.hasAnimation) {
				GameObject.Find (enemyString + i).transform.Find("EnemyBackground").GetComponent<Animator> ().runtimeAnimatorController = GameObject.Find (enemyString + i).GetComponent<LocalDataHolder> ().enemyObject.enemyAnimator;
			}
		}
	}
}
