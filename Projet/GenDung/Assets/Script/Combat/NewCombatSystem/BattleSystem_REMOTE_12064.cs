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
	public bool effectEnded;

	public Status PS;

	SavingSystem saveSystem;
	Explo_Data explo_Data;
	MapController map_Controller;
	EffectController effect_Controller;

	GameObject indicator_Battle;
	GameObject fighter_Panel;
	GameObject next_Button;

	// first time you launch a battle
	public void ResetFightStart (int roomImIn) {
		Links ();
		HideShowNext (true);
		SetupPlayers ();
		SetupEnemies (roomImIn);
		SetFighterIndex ();
		SetArrow ();
		SetupFighterPanel ();
		SetupFirstTurnAsEnemy ();
	}

	void Links(){
		saveSystem = GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ();
		explo_Data = GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data> ();
		map_Controller = GameObject.Find ("DontDestroyOnLoad").GetComponent<MapController> ();
		effect_Controller = GameObject.Find ("DontDestroyOnLoad").GetComponent<EffectController> ();

		indicator_Battle = GameObject.Find ("Pastille");
		fighter_Panel = GameObject.Find ("FighterPanel");
		next_Button = GameObject.Find ("NextPanel");
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
		initialAmountOfPlayer = saveSystem.gameData.SavedSizeOfTheTeam;

		if (!IsItFirstFight) {

			amountOfPlayerLeft = initialAmountOfPlayer;
			explo_Data.amountOfPlayerLeft = initialAmountOfPlayer;

			//par contre ici pour éviter de n'avoir que 3 joueurs afficher on doit prendre 4 qui correspond a initialAmount.
			for (int i = 0; i < initialAmountOfPlayer; i++) {

				//for each player in game reset the temporary data
				explo_Data.dungeonData.TempFighterObject[i].died = false;
				explo_Data.dungeonData.TempFighterObject[i].tempHealth = saveSystem.gameData.SavedCharacterList[i].Health_PV;

				//add the players in the gamefight list
				UnOrderedFighterList.Add (GameObject.Find (playerString + i), saveSystem.gameData.SavedCharacterList [i].Initiative);

				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().characterObject = saveSystem.gameData.SavedCharacterList [i];
				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().player = true;
				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().localIndex = i;

				if (GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().characterObject.hasAnimations) {
					GameObject.Find (playerString + i).transform.Find("PersoBackground").GetComponent<Animator> ().runtimeAnimatorController = GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().characterObject.persoAnimator;
				}
			}

			IsItFirstFight = true;

		} else {

			amountOfPlayerLeft = explo_Data.amountOfPlayerLeft;

			//par contre ici pour éviter de n'avoir que 3 joueurs afficher on doit prendre 4 qui correspond a initialAmount.
			for (int i = 0; i < initialAmountOfPlayer; i++) {
				//add the players in the gamefight list
				UnOrderedFighterList.Add (GameObject.Find (playerString + i), saveSystem.gameData.SavedCharacterList [i].Initiative);

				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().characterObject = saveSystem.gameData.SavedCharacterList [i];
				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().player = true;
				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().localIndex = i;

				//if he died well update him.
				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().dead = explo_Data.dungeonData.TempFighterObject [i].died;

				if (GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().characterObject.hasAnimations) {
					GameObject.Find (playerString + i).transform.Find("PersoBackground").GetComponent<Animator> ().runtimeAnimatorController = GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().characterObject.persoAnimator;
				}
			}
		}
	}


	void SetupEnemies(int roomNumber)
	{
		amountOfEnemies =  explo_Data.dungeonData.RoomData[roomNumber].enemyInRoom.Count;
		amountOfEnemiesLeft = amountOfEnemies;

		print (amountOfEnemiesLeft + " est le montant d'enemi left et " + amountOfEnemies + " le montant d'enemi"); 

		for (int i = 0; i < amountOfEnemies; i++) {

			if (SceneManager.GetActiveScene ().name != "NewCombatTest") 
			{
				int dungeon = map_Controller.dungeonIndex;
				EnemyObject enemy = explo_Data.dungeonData.RoomData [roomNumber].enemyInRoom [i];
				UnOrderedFighterList.Add (GameObject.Find (enemyString + i), enemy.initiative);
				GameObject.Find (enemyString + i).GetComponent<LocalDataHolder> ().enemyObject = enemy;
			} 
			else 
			{
				EnemyObject enemy = explo_Data.dungeonData.RoomData [roomNumber].enemyInRoom [i];
				UnOrderedFighterList.Add (GameObject.Find (enemyString + i), enemy.initiative);
				GameObject.Find (enemyString + i).GetComponent<LocalDataHolder> ().enemyObject = enemy;
			}

			GameObject.Find (enemyString + i).GetComponent<LocalDataHolder> ().localIndex = i;

			if (GameObject.Find (enemyString + i).GetComponent<LocalDataHolder> ().enemyObject.hasAnimation) {
				GameObject.Find (enemyString + i).transform.Find("EnemyBackground").GetComponent<Animator> ().runtimeAnimatorController = GameObject.Find (enemyString + i).GetComponent<LocalDataHolder> ().enemyObject.enemyAnimator;
			}
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

		//make sure for the enemies to not show if they are not dead the fact that you can click on them
		for (int i = 0; i < FighterList.Count; i++) 
		{
			if (!FighterList [i].GetComponent<LocalDataHolder> ().player) 
			{
				FighterList [i].transform.Find ("Shadow/Pastille2").GetComponent<Image> ().enabled = false;
			}
		}
	}

	void SetupFirstTurnAsEnemy () {

		StartCoroutine(waitForStarting());
	}

	void SetArrow () {

		indicator_Battle.GetComponent<Image> ().sprite = arrow;

		Vector3 actualPosition = new Vector3 (0,0,0);
		indicator_Battle.GetComponent<RectTransform> ().SetParent (FighterList [actuallyPlaying].transform.Find("Shadow"));
		indicator_Battle.GetComponent<RectTransform> ().localPosition = actualPosition;

		for (int i = 0; i < FighterList.Count; i++) {
			FighterList [i].GetComponent<LocalDataHolder> ().UpdateUiOrderOrder (false);
		}
		FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().UpdateUiOrderOrder (true);
	}



	public void NextTurn()
    {
		HideShowNext(false);

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
			SetArrow();
			resetActionPoint(actuallyPlaying);
			ManageStatusEffects ();
			//gere les effets et ensuite lance le reste de la fight
			UpdateFighterPanel();
        }
	}

	void ContinueFightAfterEffect(){

		if (!FighterList[actuallyPlaying].GetComponent<LocalDataHolder>().player)
		{
			EnemyTurn();
		}
		else
		{
			HideShowNext(true);
		}
	}

	void ManageStatusEffects (){

		HideShowNext (false);

		//define all the amount of effect for the player
		int maxEffects;
		//check if it's a player
		if (FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().player) {
			//max
			maxEffects = explo_Data.dungeonData.TempFighterObject [FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().localIndex].playerStatus.Count;
			print (maxEffects);
		} else {
			maxEffects = explo_Data.dungeonData.TempFighterObject [FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().localIndex + 4].playerStatus.Count;
			print (maxEffects);
		}
		if (maxEffects > 0) {
			StartCoroutine (waitForEffectEndedStartOfTurn (maxEffects));
		}else {
			ContinueFightAfterEffect ();
		}
	}

	void SetupFighterPanel () {
		if (FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().player) {
			SetSpellLinks (true);
		}
	}

	void UpdateFighterPanel () {
		if(FighterList[actuallyPlaying].GetComponent<LocalDataHolder> ().player){
			fighter_Panel.GetComponent<RectTransform> ().localPosition = new Vector3 (350,-200,0);
			SetSpellLinks (true);
		} else {
			fighter_Panel.GetComponent<RectTransform> ().localPosition = new Vector3 (0,-500,0);
		}
	}

	void SetSpellLinks (bool onOrOff) {
		
		for (int i = 0; i < 3; i++)
        {
			if (onOrOff) {
				GameObject.Find ("Button_Spell_" + i).GetComponent<Image> ().sprite = FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().characterObject.SpellList [i].spellIcon;
				GameObject.Find ("Button_Spell_" + i).GetComponent<SpellPropreties> ().spellObject = FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().characterObject.SpellList [i];
			}

			GameObject.Find ("Button_Spell_" + i).GetComponent<SpellPropreties> ().StartPersoUpdate (onOrOff);
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

			EnemyAttack ();

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

	void EnemyAttack () {
		checkEnemyToAttack ();

		StartCoroutine(waitForEnemyAttack());
	}

	void HideShowNext (bool hide){
		next_Button.GetComponent<Image> ().enabled = hide;
		next_Button.GetComponent<Button> ().interactable = hide;
		next_Button.GetComponent<Button> ().enabled = hide;
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

		HideShowNext (false);

		SetSpellLinks (false);

		for (int i = 0; i < FighterList.Count; i++) {

			GameObject.Find ("UIBattleOrderDisplay(Clone)").SetActive (false);
		}
		//clean enemy and reset them
		for (int i = 0; i < 4; i++) {
			GameObject.Find (enemyString + i).GetComponent<LocalDataHolder> ().dead = false;
			GameObject.Find (enemyString + i).transform.Find("EnemyBackground").GetComponent<Button> ().enabled = true;
			GameObject.Find (enemyString + i).transform.Find("EnemyBackground").GetComponent<Image> ().color = Color.white;
		}
	}

	void resetActionPoint(int index){
		FighterList [index].GetComponent<LocalDataHolder> ().actionPointPlayer = FighterList [index].GetComponent<LocalDataHolder> ().maxActionPointPlayer;
	}

	IEnumerator slowEnemyTurn(){
		yield return new WaitForSeconds (1.5f);
		NextTurn();
	}

	IEnumerator waitForEnemyAttack(){
		FighterList [actuallyPlaying].transform.Find("EnemyBackground").GetComponent<Animator> ().Play ("attackMonster");

		yield return new WaitForSeconds (1.0f);

		if (FighterList [rndAttackEnemy].GetComponent<LocalDataHolder> ().player) {
			if (FighterList [rndAttackEnemy].GetComponent<LocalDataHolder> ().characterObject.hasAnimations) {
				FighterList [rndAttackEnemy].transform.Find("PersoBackground").GetComponent<Animator>().Play("Attacked");
			}
		}

		yield return new WaitForSeconds (0.3f);

		if (FighterList [rndAttackEnemy].GetComponent<LocalDataHolder> ().player) {
			if (FighterList [rndAttackEnemy].GetComponent<LocalDataHolder> ().characterObject.hasAnimations) {
				FighterList [rndAttackEnemy].transform.Find("PersoBackground").GetComponent<Animator>().Play("Idle");
			}
		}

		FighterList [rndAttackEnemy].GetComponent<LocalDataHolder> ().looseLife (-FighterList[actuallyPlaying].GetComponent<LocalDataHolder>().enemyObject.atk);

	}

	IEnumerator waitForStarting(){
		yield return new WaitForSeconds (1f);

		UpdateFighterPanel();

		if (!FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().player) {

			EnemyTurn();
		}

		resetActionPoint(actuallyPlaying);
		SetArrow();
	}

	IEnumerator waitForEffectEndedStartOfTurn(int index){

		//get the status from the player, the one we'll be working with so far.
		if(FighterList[actuallyPlaying].GetComponent<LocalDataHolder>().player){
			PS = explo_Data.dungeonData.TempFighterObject [FighterList[actuallyPlaying].GetComponent<LocalDataHolder>().localIndex].playerStatus[index-1];//max = 2 so items in the list are 0 and 1.
		} else {
			PS = explo_Data.dungeonData.TempFighterObject [FighterList[actuallyPlaying].GetComponent<LocalDataHolder>().localIndex+4].playerStatus[index-1];
		}

		print ("we'll be working with effect : " + PS.statusName);
		print (PS.statusName + " has still " + PS.statusTurnLeft + " turn left");
		print (PS.statusName + " does " + PS.statusDamage + " damage");
		print (PS.statusName + " is of type " + PS.statusType);

		//now that we have a status lets check some stuff out.
		//1. how much turn left my status does have ?
		if(PS.statusTurnLeft > 0){

			print ("still have enought");

			//check from what type of status it is
			if (PS.statusType == Status.StatusType.Poisonned) {
				print ("i'm poisoning you");

				//2.play animation
				FighterList [actuallyPlaying].transform.Find ("EffectLayer").GetComponent<Animator> ().Play (effect_Controller.effect_List[2]); //poisonned

				//do the reaction for the damage for the fighter
				yield return new WaitForSeconds (1.0f);

				if (FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().player) {
					FighterList [actuallyPlaying].transform.Find ("PersoBackground").GetComponent<Animator> ().Play ("Attacked");
				} else {
					FighterList [actuallyPlaying].transform.Find ("EnemyBackground").GetComponent<Animator> ().Play ("DamageMonster");
				}

				yield return new WaitForSeconds (1.0f);

				if (FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().player) {
					FighterList [actuallyPlaying].transform.Find ("PersoBackground").GetComponent<Animator> ().Play ("Idle");
				} else {
					FighterList [actuallyPlaying].transform.Find ("EnemyBackground").GetComponent<Animator> ().Play ("IdleMonster");
				}

				//do the damages to the one affected by the effect, which is the guy playing in this case.
				FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().looseLife (-PS.statusDamage);
				PS.statusTurnLeft--;

				//check from what type of status it is
			} else if (PS.statusType == Status.StatusType.Healed) {
				print ("i'm healing you");

				//play animation
				FighterList [actuallyPlaying].transform.Find ("EffectLayer").GetComponent<Animator> ().Play (effect_Controller.effect_List[1]); //healing

				yield return new WaitForSeconds (1.0f);

				//do the damages to the one affected by the effect, which is the guy playing in this case.
				FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().looseLife (PS.statusDamage);
				PS.statusTurnLeft--;
			} else if (PS.statusType == Status.StatusType.Spike) {
				print ("i'm spanking you");

				//play animation
				FighterList [actuallyPlaying].transform.Find ("EffectLayer").GetComponent<Animator> ().Play (effect_Controller.effect_List[3]);

				//do the reaction for the damage for the fighter
				yield return new WaitForSeconds (1.0f);

				if (FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().player) {
					FighterList [actuallyPlaying].transform.Find ("PersoBackground").GetComponent<Animator> ().Play ("Attacked");
				} else {
					FighterList [actuallyPlaying].transform.Find ("EnemyBackground").GetComponent<Animator> ().Play ("DamageMonster");
				}

				yield return new WaitForSeconds (1.0f);

				if (FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().player) {
					FighterList [actuallyPlaying].transform.Find ("PersoBackground").GetComponent<Animator> ().Play ("Idle");
				} else {
					FighterList [actuallyPlaying].transform.Find ("EnemyBackground").GetComponent<Animator> ().Play ("IdleMonster");
				}

				//do the damages to the one affected by the effect, which is the guy playing in this case.
				FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().looseLife (-PS.statusDamage);
				PS.statusTurnLeft--;
			} else if (PS.statusType == Status.StatusType.TemporaryLifed) {
				FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().looseLife (-PS.statusDamage);
				PS.statusTurnLeft--;
			}
		}

		//si l'enemi ou le joueur meurt d'un effet.
		if (FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().health <= 0) {
			FighterList [actuallyPlaying].transform.Find ("EffectLayer").GetComponent<Animator> ().Play (effect_Controller.effect_List[0]);
			NextTurn ();
		} else {

			//remove the effect if this one is expired
			if (PS.statusTurnLeft <= 0) {
				if (FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().player) {
					explo_Data.dungeonData.TempFighterObject [FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().localIndex].playerStatus.RemoveAt (index - 1);
					print ("removed effect : " + PS.statusName);
				} else {
					explo_Data.dungeonData.TempFighterObject [FighterList [actuallyPlaying].GetComponent<LocalDataHolder> ().localIndex + 4].playerStatus.RemoveAt (index - 1);
					print ("removed effect : " + PS.statusName);
				}
			}

			yield return new WaitForSeconds (1.0f);

			//wait for effect to attack player
			FighterList [actuallyPlaying].transform.Find ("EffectLayer").GetComponent<Animator> ().Play (effect_Controller.effect_List[0]);



			//reduce maximum of effect to deal with start of the turn.
			index -= 1;
			print ("effect left : " + index);

			//do damages or heal depending on dot

			//redo for the next dot that the player has
			if (index > 0) {
				StartCoroutine (waitForEffectEndedStartOfTurn (index));
			} else {
				ContinueFightAfterEffect ();
			}
		}
	}
}

