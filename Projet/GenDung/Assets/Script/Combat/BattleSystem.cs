﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class BattleSystem : MonoBehaviour {

	[Header("Players")]
	string playerString = "Player ";
	public int initialAmountOfPlayer;
	public int amountOfPlayerLeft;
	public SpellObject SelectedSpellObject;
	public bool IsItFirstFight;

	[Header("Enemies")]
	string enemyString = "Enemy ";
	public int amountOfEnemies = 2; // random from data for room.
	public int amountOfEnemiesLeft;
	int rndAttackEnemy;

	[Header("Initiative")]
	private Dictionary<GameObject, int> UnOrderedFighterList = new Dictionary<GameObject, int>();
	public List<GameObject> FighterList = new List<GameObject>();
	public Sprite arrow;
	public int actuallyPlaying;

	public bool attackMode;

	// first time you launch a battle
	public void ResetFightStart (int roomImIn) {
		SetupPlayers ();
		SetupEnemies (roomImIn);
		SetFighterIndex ();
		SetArrow ();
		SetupFighterPanel ();
		SetupFirstTurnAsEnemy ();
	}

	void Update ()
    {
		if (SelectedSpellObject != null)
        {
			if (SelectedSpellObject.spellCost > FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().actionPointPlayer && FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().player) {
				attackMode = false;
			}
		}

	}

	void SetupPlayers()
	{
		initialAmountOfPlayer = GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedSizeOfTheTeam;

		if (!IsItFirstFight) {

			amountOfPlayerLeft = initialAmountOfPlayer;
			GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data> ().amountOfPlayerLeft = initialAmountOfPlayer;

			//par contre ici pour éviter de n'avoir que 3 joueurs afficher on doit prendre 4 qui correspond a initialAmount.
			for (int i = 0; i < initialAmountOfPlayer; i++) {

				//for each player in game reset the temporary data
				GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data>().dungeonData.characterObject[i].died = false;
				GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data>().dungeonData.characterObject[i].tempHealth = GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].Health_PV;

				//add the players in the gamefight list
				UnOrderedFighterList.Add (GameObject.Find (playerString + i), GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedCharacterList [i].Initiative);

				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().characterObject = GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedCharacterList [i];
				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().player = true;
				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().localIndex = i;
			}

			IsItFirstFight = true;

		} else {

			amountOfPlayerLeft = GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data> ().amountOfPlayerLeft;

			//par contre ici pour éviter de n'avoir que 3 joueurs afficher on doit prendre 4 qui correspond a initialAmount.
			for (int i = 0; i < initialAmountOfPlayer; i++) {
				//add the players in the gamefight list
				UnOrderedFighterList.Add (GameObject.Find (playerString + i), GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedCharacterList [i].Initiative);

				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().characterObject = GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedCharacterList [i];
				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().player = true;
				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().localIndex = i;

				//if he died well update him.
				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().dead = GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data> ().dungeonData.characterObject [i].died;
			}
		}
	}


	void SetupEnemies(int roomNumber)
	{
		amountOfEnemies =  GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data>().dungeonData.RoomData[roomNumber].enemyInRoom.Count;

		amountOfEnemiesLeft = amountOfEnemies;

		print (amountOfEnemiesLeft + " est le montant d'enemi left et " + amountOfEnemies + " le montant d'enemi"); 

		for (int i = 0; i < amountOfEnemies; i++) {
			//add the enemies in the gamefight list

			if (SceneManager.GetActiveScene ().name != "NewCombatTest") 
			{
				int dungeon = GameObject.Find ("DontDestroyOnLoad").GetComponent<MapController> ().dungeonIndex;

				//int enemyRand = Random.Range (0, GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons [dungeon].enemiesList.Count);

				//int enemy = GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data> ().dungeonData.RoomData [roomNumber].enemyInRoom [i];
				EnemyObject enemy = GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data> ().dungeonData.RoomData [roomNumber].enemyInRoom [i];

				//UnOrderedFighterList.Add (GameObject.Find (enemyString + i), GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons [dungeon].enemiesList [enemy].initiative);

				UnOrderedFighterList.Add (GameObject.Find (enemyString + i), enemy.initiative);

				//load their image depending on the list
				//GameObject.Find (enemyString + i).GetComponent<LocalDataHolder> ().enemyObject = GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons [dungeon].enemiesList [enemy];

				GameObject.Find (enemyString + i).GetComponent<LocalDataHolder> ().enemyObject = enemy;
			} 
			else 
			{
				//int enemyRand = Random.Range (0, GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons [0].enemiesList.Count);

				//int enemy = GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data> ().dungeonData.RoomData [roomNumber].enemyInRoom [i];

				EnemyObject enemy = GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data> ().dungeonData.RoomData [roomNumber].enemyInRoom [i];

				//UnOrderedFighterList.Add (GameObject.Find (enemyString + i), GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons [0].enemiesList [enemy].initiative);

				UnOrderedFighterList.Add (GameObject.Find (enemyString + i), enemy.initiative);

				//load their image depending on the list
				//GameObject.Find (enemyString + i).GetComponent<LocalDataHolder> ().enemyObject = GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons [0].enemiesList [enemy];

				GameObject.Find (enemyString + i).GetComponent<LocalDataHolder> ().enemyObject = enemy;
			}

			GameObject.Find (enemyString + i).GetComponent<LocalDataHolder> ().localIndex = i;
		}
	}

	void SetFighterIndex(){

		FighterList = UnOrderedFighterList.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();

		for (int i = 0; i < FighterList.Count; i++) {
			FighterList[i].GetComponent<LocalDataHolder> ().fighterIndex = i;

			GameObject UiBattleDisplay;

			UiBattleDisplay = Instantiate(Resources.Load("UI_Interface/UIBattleOrderDisplay"), GameObject.Find ("OrderBattlePanel").transform) as GameObject;

			FighterList [i].GetComponent<LocalDataHolder> ().UiOrderObject = UiBattleDisplay;


		}

		//hide the others and make the initializer work
		for (int i = 0; i < 4; i++) {
			GameObject.Find (enemyString + i).GetComponent<LocalDataHolder> ().Initialize();
			GameObject.Find	(playerString + i).GetComponent<LocalDataHolder> ().Initialize();
		}
	}

	void SetupFirstTurnAsEnemy () {

		StartCoroutine(waitForStarting());
	}

	void SetArrow () {

		GameObject.Find ("Pastille").GetComponent<Image> ().sprite = arrow;

		Vector3 actualPosition = FighterList [actuallyPlaying].GetComponent<RectTransform> ().position;
		GameObject.Find ("Pastille").GetComponent<RectTransform>().position = actualPosition + new Vector3(0,32,0);

		for (int i = 0; i < FighterList.Count; i++) {
			FighterList [i].GetComponent<LocalDataHolder> ().UpdateUiOrderOrder (false);
		}
		FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().UpdateUiOrderOrder (true);
	}

	public void NextTurn()
    {
		actuallyPlaying++;
		if (actuallyPlaying >= FighterList.Count) {
			actuallyPlaying = 0;
		}

		if(FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().dead)
		{
			NextTurn ();
		}
        else
        {
            UpdateFighterPanel();

            if (!FighterList[actuallyPlaying].GetComponent<LocalDataHolder>().player)
            {
                EnemyTurn();
            }
            else
            {
                HideShowNext(true);
            }

            resetActionPoint(actuallyPlaying);
            SetArrow();
        }
	}

	void SetupFighterPanel () {
		if (FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().player) {
			SetSpellLinks ();
		}
	}

	void UpdateFighterPanel () {
		if(FighterList[actuallyPlaying].GetComponent<LocalDataHolder> ().player){
			GameObject.Find ("FighterPanel").GetComponent<RectTransform> ().localPosition = new Vector3 (GameObject.Find ("FighterPanel").GetComponent<RectTransform> ().sizeDelta.x,0,0);
			SetSpellLinks ();
		} else {
			GameObject.Find ("FighterPanel").GetComponent<RectTransform> ().localPosition = new Vector3 (-GameObject.Find ("FighterPanel").GetComponent<RectTransform> ().sizeDelta.x,0,0);
		}
	}

	void SetSpellLinks () {
		
		for (int i = 0; i < 3; i++)
        {
			GameObject.Find ("Button_Spell_" + i).GetComponent<Image> ().sprite = FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().characterObject.SpellList [i].spellIcon;
			GameObject.Find ("Button_Spell_" + i).GetComponent<SpellPropreties> ().spellObject = FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().characterObject.SpellList[i];


			GameObject.Find ("Button_Spell_" + i).GetComponent<SpellPropreties> ().StartPersoUpdate ();
		}
	}

	void checkEnemyToAttack(){

        do
        {
            rndAttackEnemy = Random.Range(0, FighterList.Count);
        }
        while (FighterList[rndAttackEnemy].GetComponent<LocalDataHolder>().dead || !FighterList[rndAttackEnemy].GetComponent<LocalDataHolder>().player);
	}

	void EnemyTurn () {
		//attack enemy
		if (amountOfPlayerLeft > 0) {
			checkEnemyToAttack ();

			FighterList [rndAttackEnemy].GetComponent<LocalDataHolder> ().looseLife (FighterList[actuallyPlaying].GetComponent<LocalDataHolder>().enemyObject.atk);

			//hide next button
			HideShowNext(false);

			//next turn
			StartCoroutine(slowEnemyTurn());
		} 
		else 
		{
			EndBattleAllPlayerDead ();
		}
	}

	void HideShowNext (bool hide){
		GameObject.Find ("NextPanel").GetComponent<Image> ().enabled = hide;
		GameObject.Find ("NextPanel").GetComponent<Button> ().interactable = hide;
		GameObject.Find ("NextPanel").GetComponent<Button> ().enabled = hide;
		GameObject.Find ("NextPanel/NextText").GetComponent<Text> ().enabled = hide;
	}

	public void EndBattleAllPlayerDead () {
		//UnityEditor.EditorApplication.isPlaying = false;

		attackMode = false;

		SceneManager.LoadScene ("Map");
	}

	public void EndBattleAllMonsterDead () {

		attackMode = false;

		if (SceneManager.GetActiveScene ().name != "NewCombatTest") {
			GameObject.Find ("ExploGridPrefab").GetComponent<Explo_FightRoom> ().CleanFinishedFightRoom ();
		} else {
			SceneManager.LoadScene ("Init");
		}

		//reset for next fight
		resetFight();
		//need un reset en fonction de la salle des monstres
	}

	void resetFight(){
		amountOfEnemiesLeft = amountOfEnemies;

		UnOrderedFighterList.Clear();

		for (int i = 0; i < FighterList.Count; i++) {
			GameObject.Find ("UIBattleOrderDisplay(Clone)").SetActive (false);

			if (FighterList [i].GetComponent<LocalDataHolder> ().player == false) {
				FighterList [i].GetComponent<LocalDataHolder> ().dead = false;
				FighterList [i].gameObject.GetComponent<Button> ().enabled = true;
				FighterList [i].gameObject.GetComponent<Image> ().color = Color.white;
			}
		}
	}

	void resetActionPoint(int index){
		FighterList [index].GetComponent<LocalDataHolder> ().actionPointPlayer = FighterList [index].GetComponent<LocalDataHolder> ().maxActionPointPlayer;
	}

	IEnumerator slowEnemyTurn(){
		yield return new WaitForSeconds (0.8f);
		NextTurn();
	}

	IEnumerator waitForStarting(){
		yield return new WaitForSeconds (0.5f);

		UpdateFighterPanel();

		if (!FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().player) {

			EnemyTurn();
		}

		resetActionPoint(actuallyPlaying);
		SetArrow();
	}
}

