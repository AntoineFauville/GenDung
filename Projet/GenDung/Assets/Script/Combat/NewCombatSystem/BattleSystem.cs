using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class BattleSystem : MonoBehaviour {


	public int initialAmountOfPlayer;
	public int amountOfPlayerLeft;
	public SpellObject SelectedSpellObject;

	public int amountOfEnemies = 2; // random from data for room.
	public int amountOfEnemiesLeft;

	public List<GameObject> FighterList = new List<GameObject>();
	public int actuallyPlaying;

	public bool attackMode;
	public bool effectEnded;

	GameObject scriptBattleHolder;

	NextTurn_Controller nextTurn_Controller;
	FighterIndicator_Controller indicator_Controller;
	PlayerSetup_Controller playerSetup_Controller;
	EnemySetup_Controller enemySetup_Controller;
	Initiative_Controller initiative_Controller;
	Spell_Controller spell_Controller;
	EnemyTurn_Controller enemyTurn_Controller;

	// first time you launch a battle
	public void ResetFightStart (int roomImIn) {
		Links ();
		nextTurn_Controller.HideShowNext (true);
		playerSetup_Controller.SetupPlayers ();
		enemySetup_Controller.SetupEnemies (roomImIn);
		initiative_Controller.SetFighterIndex ();
		indicator_Controller.SetArrow ();
		spell_Controller.SetupFighterPanel ();
		enemyTurn_Controller.SetupFirstTurnAsEnemy ();
	}

	public void Links(){
		scriptBattleHolder = GameObject.Find ("BattleSystem/ScriptBattle");
		nextTurn_Controller = scriptBattleHolder.GetComponent<NextTurn_Controller> ();
		indicator_Controller = scriptBattleHolder.GetComponent<FighterIndicator_Controller> ();
		playerSetup_Controller = scriptBattleHolder.GetComponent<PlayerSetup_Controller> ();
		enemySetup_Controller = scriptBattleHolder.GetComponent<EnemySetup_Controller> ();
		initiative_Controller = scriptBattleHolder.GetComponent<Initiative_Controller> ();
		spell_Controller = scriptBattleHolder.GetComponent<Spell_Controller> ();
		enemyTurn_Controller = scriptBattleHolder.GetComponent<EnemyTurn_Controller> ();
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

	public void ContinueFightAfterEffect(){

		if (!FighterList[actuallyPlaying].GetComponent<LocalDataHolder>().player)
		{
			enemyTurn_Controller.EnemyTurn();
		}
		else
		{
			nextTurn_Controller.HideShowNext(true);
		}
	}

	public void EndBattleAllPlayerDead () {

		attackMode = false;

		SceneManager.LoadScene ("Map");
	}

	public void EndBattleAllMonsterDead () {

		attackMode = false;

		if (SceneManager.GetActiveScene ().name != "NewCombatTest") {
			GameObject.Find ("ExploGridPrefab").GetComponent<Explo_Room_FightController> ().CleanFinishedFightRoom ();
		} else {
			SceneManager.LoadScene ("Init");
		}

		//reset for next fight
		resetFight();
		//need un reset en fonction de la salle des monstres
	}

	public void resetFight(){
		amountOfEnemiesLeft = amountOfEnemies;

		initiative_Controller.UnOrderedFighterList.Clear();

		nextTurn_Controller.HideShowNext (false);

		spell_Controller.SetSpellLinks (false);

		for (int i = 0; i < FighterList.Count; i++) {

			GameObject.Find ("UIBattleOrderDisplay(Clone)").SetActive (false);
		}
		//clean enemy and reset them
		for (int i = 0; i < 4; i++) {
			GameObject.Find (enemySetup_Controller.enemyString + i).GetComponent<LocalDataHolder> ().dead = false;
			GameObject.Find (enemySetup_Controller.enemyString + i).transform.Find("EnemyBackground").GetComponent<Button> ().enabled = true;
			GameObject.Find (enemySetup_Controller.enemyString + i).transform.Find("EnemyBackground").GetComponent<Image> ().color = Color.white;
		}
	}

	public void resetActionPoint(int index){
		FighterList [index].GetComponent<LocalDataHolder> ().actionPointPlayer = FighterList [index].GetComponent<LocalDataHolder> ().maxActionPointPlayer;
	}
}

