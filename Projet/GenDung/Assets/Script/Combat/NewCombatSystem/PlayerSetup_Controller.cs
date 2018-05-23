using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup_Controller : MonoBehaviour {

	[Header("Players")]
	public string playerString = "Player ";
	bool IsItFirstFight;

	BattleSystem BS;
	SavingSystem saveSystem;
	Explo_DataController explo_Data;
	Initiative_Controller initiative_Controller;

	GameObject scriptBattleHolder;
	GameObject dontDestroyOnLoad;

	public void Start(){
		dontDestroyOnLoad = GameObject.Find ("DontDestroyOnLoad");
		scriptBattleHolder = GameObject.Find ("BattleSystem/ScriptBattle");

		BS = GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ();
		saveSystem = dontDestroyOnLoad.GetComponent<SavingSystem> ();
		explo_Data = dontDestroyOnLoad.GetComponent<Explo_DataController> ();
		initiative_Controller = scriptBattleHolder.GetComponent<Initiative_Controller> ();
	}

	public void SetupPlayers()
	{
		BS.initialAmountOfPlayer = saveSystem.gameData.SavedSizeOfTheTeam;

		if (!IsItFirstFight) {

			BS.amountOfPlayerLeft = BS.initialAmountOfPlayer;
			dontDestroyOnLoad.GetComponent<Explo_DataController> ().amountOfPlayerLeft = BS.initialAmountOfPlayer;

			explo_Data.amountOfPlayerLeft = BS.initialAmountOfPlayer;

			//par contre ici pour éviter de n'avoir que 3 joueurs afficher on doit prendre 4 qui correspond a initialAmount.
			for (int i = 0; i < BS.initialAmountOfPlayer; i++) {

				//for each player in game reset the temporary data
				explo_Data.dungeonData.TempFighterObject[i].died = false;
				explo_Data.dungeonData.TempFighterObject[i].tempHealth = saveSystem.gameData.SavedCharacterList[i].Health_PV;

				//add the players in the gamefight list
				initiative_Controller.UnOrderedFighterList.Add (GameObject.Find (playerString + i), saveSystem.gameData.SavedCharacterList [i].Initiative);

				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().characterObject = saveSystem.gameData.SavedCharacterList [i];
				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().player = true;
				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().localIndex = i;

				if (GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().characterObject.hasAnimations) {
					GameObject.Find (playerString + i).transform.Find("Background").GetComponent<Animator> ().runtimeAnimatorController = GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().characterObject.persoAnimator;
				}
			}

			IsItFirstFight = true;

		} else {

			BS.amountOfPlayerLeft = explo_Data.amountOfPlayerLeft;

			//par contre ici pour éviter de n'avoir que 3 joueurs afficher on doit prendre 4 qui correspond a initialAmount.
			for (int i = 0; i < BS.initialAmountOfPlayer; i++) {
				//add the players in the gamefight list
				initiative_Controller.UnOrderedFighterList.Add (GameObject.Find (playerString + i), saveSystem.gameData.SavedCharacterList [i].Initiative);

				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().characterObject = saveSystem.gameData.SavedCharacterList [i];
				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().player = true;
				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().localIndex = i;

				//if he died well update him.
				GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().dead = explo_Data.dungeonData.TempFighterObject [i].died;

				if (GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().characterObject.hasAnimations) {
					GameObject.Find (playerString + i).transform.Find("Background").GetComponent<Animator> ().runtimeAnimatorController = GameObject.Find (playerString + i).GetComponent<LocalDataHolder> ().characterObject.persoAnimator;
				}
			}
		}
	}
}
