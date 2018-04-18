using System.Collections;
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

	// keep it because of data holder delay as awake
	void Awake () {
		SetupPlayers ();
		SetupEnemies ();
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

		amountOfPlayerLeft = initialAmountOfPlayer;

		for (int i = 0; i < initialAmountOfPlayer; i++) {
			//add the players in the gamefight list
			UnOrderedFighterList.Add (GameObject.Find(playerString + i), GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedCharacterList [i].Initiative);
			//load their image depending on the list
			GameObject.Find(playerString + i).GetComponent<LocalDataHolder> ().characterObject = GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedCharacterList [i];
			GameObject.Find(playerString + i).GetComponent<LocalDataHolder> ().player = true;
			GameObject.Find(playerString + i).GetComponent<LocalDataHolder> ().localIndex = i;
		}
	}


	void SetupEnemies()
	{
		if (SceneManager.GetActiveScene ().name != "NewCombatTest") {

			int dungeon = GameObject.Find ("DontDestroyOnLoad").GetComponent<MapController> ().dungeonIndex;

			amountOfEnemies = Random.Range (0, GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons [dungeon].enemyMax);

		} else {
			
			amountOfEnemies = 4;
			amountOfEnemiesLeft = amountOfEnemies;
		}

		for (int i = 0; i < amountOfEnemies; i++) {
			//add the enemies in the gamefight list

			if (SceneManager.GetActiveScene ().name != "NewCombatTest") 
			{
				int dungeon = GameObject.Find ("DontDestroyOnLoad").GetComponent<MapController> ().dungeonIndex;

				int enemyRand = Random.Range (0, GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons [dungeon].enemiesList.Count);

				UnOrderedFighterList.Add (GameObject.Find (enemyString + i), GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons [dungeon].enemiesList [enemyRand].initiative);
				//load their image depending on the list
				GameObject.Find (enemyString + i).GetComponent<LocalDataHolder> ().enemyObject = GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons [dungeon].enemiesList [enemyRand];
			} 
			else 
			{
				int enemyRand = Random.Range (0, GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons [0].enemiesList.Count);

				UnOrderedFighterList.Add (GameObject.Find (enemyString + i), GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons [0].enemiesList [enemyRand].initiative);
				//load their image depending on the list
				GameObject.Find (enemyString + i).GetComponent<LocalDataHolder> ().enemyObject = GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons [0].enemiesList [enemyRand];
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

			//FighterList [i].GetComponent<LocalDataHolder> ().SetupUiOrderObject();
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
		SceneManager.LoadScene ("Map");
	}

	public void EndBattleAllMonsterDead () {
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

