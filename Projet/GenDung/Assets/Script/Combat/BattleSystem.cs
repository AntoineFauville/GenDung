using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleSystem : MonoBehaviour {

	[Header("Players")]
	string playerString = "Player ";
	public int initialAmountOfPlayer;
	public int amountOfPlayerLeft;
	//public List<GameObject> PlayerList = new List<GameObject>();
	//public List<GameObject> DeadPlayerList = new List<GameObject>();
	public SpellObject SelectedSpellObject;

	[Header("Enemies")]
	string enemyString = "Enemy ";
	public int amountOfEnemies = 4;
	public int amountOfEnemiesLeft;
	//public List<GameObject> EnemyList = new List<GameObject>();
	//public List<GameObject> DeadEnemyList = new List<GameObject>();
	int rndAttackEnemy;

	[Header("Initiative")]
	public List<GameObject> FighterList = new List<GameObject>();
	//public List<GameObject> DeadFighterList = new List<GameObject>();
	public Sprite arrow;
	public int actuallyPlaying;

	public bool attackMode;

	// keep it because of data holder delay as awake
	void Awake () {
		SetupPlayers ();
		SetupEnemies ();
		SetFighterIndex ();
		SetArrow ();
		SetupFighterPanel ();
	}

	void Update () {
		if (amountOfPlayerLeft <= 0) {
			EndBattleAllPlayerDead ();
		}
		if (amountOfEnemiesLeft <= 0) {
			EndBattleAllMonsterDead ();
		}

		/*
		if (attackMode) {
			print ("attack mode " + attackMode);
			HideShowNext (false);
		} else {
			print ("attack mode " + attackMode);
			HideShowNext(true);
		}
		*/

		if (SelectedSpellObject != null) {
			if (SelectedSpellObject.spellCost > FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().actionPointPlayer && FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().player) {
				attackMode = false;
			}
		}

	}

	void SetupPlayers()
	{
		initialAmountOfPlayer = GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedSizeOfTheTeam;

		amountOfPlayerLeft = initialAmountOfPlayer;

		for (int i = 0; i < initialAmountOfPlayer; i++) {
			//add the players in the gamefight list
			//PlayerList.Add (GameObject.Find(playerString + i));
			FighterList.Add (GameObject.Find(playerString + i));
			//load their image depending on the list
			//PlayerList [i].GetComponent<Image> ().sprite = GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedCharacterList [i].ICON;
			FighterList [i].GetComponent<LocalDataHolder> ().characterObject = GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedCharacterList [i];
			FighterList [i].GetComponent<LocalDataHolder> ().player = true;

			FighterList [i].GetComponent<LocalDataHolder> ().localIndex = i;
		}
	}


	void SetupEnemies()
	{
		amountOfEnemiesLeft = amountOfEnemies;

		for (int i = 0; i < amountOfEnemies; i++) {
			//add the enemies in the gamefight list
			//EnemyList.Add (GameObject.Find(enemyString + i));
			FighterList.Add (GameObject.Find(enemyString + i));
			//load their image depending on the list
			//EnemyList [i].GetComponent<Image> ().sprite = GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons[0].enemiesList[0].enemyIcon;
			FighterList [i + initialAmountOfPlayer].GetComponent<LocalDataHolder> ().enemyObject = GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons[0].enemiesList[0];

			FighterList [i + initialAmountOfPlayer].GetComponent<LocalDataHolder> ().localIndex = i;
		}
	}

	void SetFighterIndex(){
		for (int i = 0; i < FighterList.Count; i++) {
			FighterList[i].GetComponent<LocalDataHolder> ().fighterIndex = i;
		}
	}

	void SetArrow () {

		GameObject.Find ("Pastille").GetComponent<Image> ().sprite = arrow;

		Vector3 actualPosition = FighterList [actuallyPlaying].GetComponent<RectTransform> ().position;
		GameObject.Find ("Pastille").GetComponent<RectTransform>().position = actualPosition + new Vector3(0,32,0);
	}

	public void NextTurn(){
		//checkTurnRound ();

		actuallyPlaying++;
		if (actuallyPlaying >= FighterList.Count) {
			actuallyPlaying = 0;
		}
		if(FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().dead){
			NextTurn ();
			return;
		}


		if (actuallyPlaying >= FighterList.Count) {
			actuallyPlaying = 0;
		}

		UpdateFighterPanel ();

		FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().UpdateLife();

		if (!FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().player) {
			EnemyTurn ();
		} else {
			HideShowNext(true);
		}

		resetActionPoint (actuallyPlaying);
		SetArrow ();
	}

	void SetupFighterPanel () {
		SetSpellLinks ();
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
		for (int i = 0; i < 3; i++) {
			GameObject.Find ("Button_Spell_" + i).GetComponent<Image> ().sprite = FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().characterObject.SpellList [i].spellIcon;
			GameObject.Find ("Button_Spell_" + i).GetComponent<SpellPropreties> ().spellObject = FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().characterObject.SpellList[i];
		}

	}

	void checkEnemyToAttack(){

		rndAttackEnemy = Random.Range (0, FighterList.Count);

		if (FighterList [rndAttackEnemy].GetComponent<LocalDataHolder> ().dead || !FighterList [rndAttackEnemy].GetComponent<LocalDataHolder> ().player) {
			checkEnemyToAttack ();
		} else {
			return;
		}
	}

	void EnemyTurn () {
		//attack enemy
		if (amountOfPlayerLeft > 0) {
			checkEnemyToAttack ();

			FighterList [rndAttackEnemy].GetComponent<LocalDataHolder> ().looseLife (1);

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

	void EndBattleAllPlayerDead () {
		//UnityEditor.EditorApplication.isPlaying = false;
		SceneManager.LoadScene ("Map");
	}

	void EndBattleAllMonsterDead () {
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
	}

	void resetActionPoint(int index){
		FighterList [index].GetComponent<LocalDataHolder> ().actionPointPlayer = FighterList [index].GetComponent<LocalDataHolder> ().maxActionPointPlayer;
	}

	IEnumerator slowEnemyTurn(){
		yield return new WaitForSeconds (0.8f);
		NextTurn();
	}
}
