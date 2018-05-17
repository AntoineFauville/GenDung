using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn_Controller : MonoBehaviour {

	BattleSystem BS;
	NextTurn_Controller	nextTurn_Controller;
	EnemySetup_Controller enemySetup_Controller;
	SpellHolder_Controller spellHolder_Controller;
	FighterIndicator_Controller indicator_Controller;

	GameObject scriptBattleHolder;
	GameObject dontDestroyOnLoad;
	GameObject actualEnemyPlaying;
	GameObject fighterToAttack;

	int criticalChances = 10;
	bool critic;

	// Use this for initialization
	void Start () {
		scriptBattleHolder = GameObject.Find ("BattleSystem/ScriptBattle");
		dontDestroyOnLoad = GameObject.Find ("DontDestroyOnLoad");

		BS = scriptBattleHolder.GetComponent<BattleSystem> ();
		nextTurn_Controller = scriptBattleHolder.GetComponent<NextTurn_Controller> ();
		enemySetup_Controller = scriptBattleHolder.GetComponent<EnemySetup_Controller> ();
		spellHolder_Controller = scriptBattleHolder.GetComponent<SpellHolder_Controller> ();
		indicator_Controller = scriptBattleHolder.GetComponent<FighterIndicator_Controller> ();
	}

	public void SetupFirstTurnAsEnemy () {

		StartCoroutine(waitForStarting());
	}

	public void EnemyTurn () {
		//attack enemy

		actualEnemyPlaying = BS.FighterList [BS.actuallyPlaying];

		if (BS.amountOfPlayerLeft > 0) {

			EnemyAttack ();

			//hide next button
			nextTurn_Controller.HideShowNext(false);

			//next turn
			StartCoroutine(slowEnemyTurn());
		} 
		else 
		{
			BS.EndBattleAllPlayerDead ();
		}
	}

	public void EnemyAttack () {
		
		checkEnemyToAttack ();

		StartCoroutine(waitForEnemyAttack());
	}

	public void checkEnemyToAttack(){

		do
		{
			enemySetup_Controller.rndAttackEnemy = Random.Range(0, BS.FighterList.Count);
			fighterToAttack = BS.FighterList[enemySetup_Controller.rndAttackEnemy];
		}
		while (fighterToAttack.GetComponent<LocalDataHolder>().dead || !fighterToAttack.GetComponent<LocalDataHolder>().player);
	}

	public IEnumerator waitForEnemyAttack(){
		actualEnemyPlaying.transform.Find("EnemyBackground").GetComponent<Animator> ().Play ("attackMonster");

		yield return new WaitForSeconds (1.0f);

		if (fighterToAttack.GetComponent<LocalDataHolder> ().player) {
			if (fighterToAttack.GetComponent<LocalDataHolder> ().characterObject.hasAnimations) {
				fighterToAttack.transform.Find("PersoBackground").GetComponent<Animator>().Play("Attacked");
			}
		}

		yield return new WaitForSeconds (0.3f);

		if (fighterToAttack.GetComponent<LocalDataHolder> ().player) {
			if (fighterToAttack.GetComponent<LocalDataHolder> ().characterObject.hasAnimations) {
				fighterToAttack.transform.Find("PersoBackground").GetComponent<Animator>().Play("Idle");
			}
		}

		int chances = Random.Range (0, 100);

		if (criticalChances >= chances) {
			fighterToAttack.GetComponent<LocalDataHolder> ().looseLife (-actualEnemyPlaying.GetComponent<LocalDataHolder> ().enemyObject.atk * 1.5f, true);
		} else {
			fighterToAttack.GetComponent<LocalDataHolder> ().looseLife (-actualEnemyPlaying.GetComponent<LocalDataHolder> ().enemyObject.atk, false);
		}
	}

	public IEnumerator slowEnemyTurn(){
		yield return new WaitForSeconds (1.5f);
		nextTurn_Controller.NextTurn();
	}

	public IEnumerator waitForStarting(){
		yield return new WaitForSeconds (1f);

		spellHolder_Controller.UpdateFighterPanel();

		if (!BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().player) {

			EnemyTurn();
		}

		//BS.resetActionPoint(BS.actuallyPlaying);
		//indicator_Controller.SetArrow();
	}
}
