using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour {

	[Header("Players")]
	string playerString = "Player ";
	public List<GameObject> PlayerList = new List<GameObject>();
	public List<GameObject> DeadPlayerList = new List<GameObject>();

	[Header("Enemies")]
	string enemyString = "Enemy ";
	public int amountOfEnemies = 4;
	public List<GameObject> EnemyList = new List<GameObject>();
	public List<GameObject> DeadEnemyList = new List<GameObject>();

	[Header("Initiative")]
	public List<GameObject> FighterList = new List<GameObject>();
	public List<GameObject> DeadFighterList = new List<GameObject>();
	public Sprite arrow;
	public int actuallyPlaying;

	// keep it because of data holder delay as awake
	void Awake () {
		SetupPlayers ();
		SetupEnemies ();
		SetFighterIndex ();
		SetArrow ();
		UpdateFighterPanel ();
	}

	void SetupPlayers()
	{
		for (int i = 0; i < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedSizeOfTheTeam; i++) {
			//add the players in the gamefight list
			PlayerList.Add (GameObject.Find(playerString + i));
			FighterList.Add (GameObject.Find(playerString + i));
			//load their image depending on the list
			//PlayerList [i].GetComponent<Image> ().sprite = GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedCharacterList [i].ICON;
			PlayerList [i].GetComponent<LocalDataHolder> ().characterObject = GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedCharacterList [i];
			PlayerList [i].GetComponent<LocalDataHolder> ().player = true;
			PlayerList [i].GetComponent<LocalDataHolder> ().localIndex = i;
		}
	}


	void SetupEnemies()
	{
		for (int i = 0; i < amountOfEnemies; i++) {
			//add the enemies in the gamefight list
			EnemyList.Add (GameObject.Find(enemyString + i));
			FighterList.Add (GameObject.Find(enemyString + i));
			//load their image depending on the list
			//EnemyList [i].GetComponent<Image> ().sprite = GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons[0].enemiesList[0].enemyIcon;
			EnemyList [i].GetComponent<LocalDataHolder> ().enemyObject = GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons[0].enemiesList[0];
			EnemyList [i].GetComponent<LocalDataHolder> ().localIndex = i;
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

		SetArrow ();
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
		GameObject.Find ("Button_Spell_1").GetComponent<Image> ().sprite = FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().characterObject.SpellList [0].spellIcon;
	}

	void EnemyTurn () {
		int rnd = Random.Range (0, PlayerList.Count);
		PlayerList[rnd].GetComponent<LocalDataHolder> ().looseLife(1);

		//hide next button
		HideShowNext(false);

		//next turn
		StartCoroutine(slowEnemyTurn());
	}

	void HideShowNext (bool hide){
		GameObject.Find ("NextPanel").GetComponent<Image> ().enabled = hide;
		GameObject.Find ("NextPanel").GetComponent<Button> ().enabled = hide;
		GameObject.Find ("NextPanel/NextText").GetComponent<Text> ().enabled = hide;
	}

	IEnumerator slowEnemyTurn(){
		yield return new WaitForSeconds (0.3f);
		NextTurn();
	}
}
