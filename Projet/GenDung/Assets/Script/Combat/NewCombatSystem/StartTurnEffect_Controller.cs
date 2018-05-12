using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTurnEffect_Controller : MonoBehaviour {

	BattleSystem BS;
	NextTurn_Controller nextTurn_Controller;
	Explo_DataController explo_Data;
	EffectController effect_Controller;

	GameObject scriptBattleHolder;
	GameObject dontDestroyOnLoad;
	GameObject actualFighter;

	public Status PS;

	// Use this for initialization
	void Start () {
		scriptBattleHolder = GameObject.Find ("BattleSystem/ScriptBattle");
		dontDestroyOnLoad = GameObject.Find ("DontDestroyOnLoad");

		BS = this.GetComponent<BattleSystem> ();
		nextTurn_Controller = scriptBattleHolder.GetComponent<NextTurn_Controller> ();
		explo_Data = dontDestroyOnLoad.GetComponent<Explo_DataController> ();
		effect_Controller = dontDestroyOnLoad.GetComponent<EffectController> ();
	}

	public void DisplayUIToolTip(){

		int amountOfEffectsToDisplay;

		BS.FighterList [BS.actuallyPlaying].GetComponent<ToolTipStatus_Controller> ().RemoveEffect ();

		if (BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().player) {
			//max
			amountOfEffectsToDisplay = explo_Data.dungeonData.TempFighterObject [BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().localIndex].playerStatus.Count;
		} else {
			amountOfEffectsToDisplay = explo_Data.dungeonData.TempFighterObject [BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().localIndex + 4].playerStatus.Count;
		}
		for (int i = 0; i < amountOfEffectsToDisplay; i++) 
		{
			if (BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().player) 
			{
				BS.FighterList [BS.actuallyPlaying].GetComponent<ToolTipStatus_Controller> ().AddEffectToUI (explo_Data.dungeonData.TempFighterObject [BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().localIndex].playerStatus [i]);
			} else {
				BS.FighterList [BS.actuallyPlaying].GetComponent<ToolTipStatus_Controller> ().AddEffectToUI (explo_Data.dungeonData.TempFighterObject [BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().localIndex + 4].playerStatus [i]);
			}
		}

	}
	
	public void ManageStatusEffects (){

		nextTurn_Controller.HideShowNext (false);

		//define all the amount of effect for the player
		int maxEffects;
		//check if it's a player
		if (BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().player) {
			//max
			maxEffects = explo_Data.dungeonData.TempFighterObject [BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().localIndex].playerStatus.Count;
			print (maxEffects);
		} else {
			maxEffects = explo_Data.dungeonData.TempFighterObject [BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().localIndex + 4].playerStatus.Count;
			print (maxEffects);
		}
		if (maxEffects > 0) {
			StartCoroutine (waitForEffectEndedStartOfTurn (maxEffects));
		}else {
			BS.ContinueFightAfterEffect ();
			DisplayUIToolTip ();
		}
	}

	public IEnumerator waitForEffectEndedStartOfTurn(int index){

		actualFighter = BS.FighterList [BS.actuallyPlaying];

		//get the status from the player, the one we'll be working with so far.
		if(actualFighter.GetComponent<LocalDataHolder>().player){
			PS = explo_Data.dungeonData.TempFighterObject [actualFighter.GetComponent<LocalDataHolder>().localIndex].playerStatus[index-1];//max = 2 so items in the list are 0 and 1.
		} else {
			PS = explo_Data.dungeonData.TempFighterObject [actualFighter.GetComponent<LocalDataHolder>().localIndex+4].playerStatus[index-1];
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
				actualFighter.transform.Find ("EffectLayer").GetComponent<Animator> ().Play (effect_Controller.effect_List[2]); //poisonned

				//do the reaction for the damage for the fighter
				yield return new WaitForSeconds (1.0f);

				if (actualFighter.GetComponent<LocalDataHolder> ().player) {
					actualFighter.transform.Find ("PersoBackground").GetComponent<Animator> ().Play ("Attacked");
				} else {
					actualFighter.transform.Find ("EnemyBackground").GetComponent<Animator> ().Play ("DamageMonster");
				}

				yield return new WaitForSeconds (1.0f);

				if (actualFighter.GetComponent<LocalDataHolder> ().player) {
					actualFighter.transform.Find ("PersoBackground").GetComponent<Animator> ().Play ("Idle");
				} else {
					actualFighter.transform.Find ("EnemyBackground").GetComponent<Animator> ().Play ("IdleMonster");
				}

				//do the damages to the one affected by the effect, which is the guy playing in this case.
				actualFighter.GetComponent<LocalDataHolder> ().looseLife (-PS.statusDamage, false);
				PS.statusTurnLeft--;

				//check from what type of status it is
			} else if (PS.statusType == Status.StatusType.Healed) {
				print ("i'm healing you");

				//play animation
				actualFighter.transform.Find ("EffectLayer").GetComponent<Animator> ().Play (effect_Controller.effect_List[1]); //healing

				yield return new WaitForSeconds (1.0f);

				//do the damages to the one affected by the effect, which is the guy playing in this case.
				actualFighter.GetComponent<LocalDataHolder> ().looseLife (PS.statusDamage, false);
				PS.statusTurnLeft--;
			} else if (PS.statusType == Status.StatusType.Spike) {
				print ("i'm spanking you");

				//play animation
				actualFighter.transform.Find ("EffectLayer").GetComponent<Animator> ().Play (effect_Controller.effect_List[3]);

				//do the reaction for the damage for the fighter
				yield return new WaitForSeconds (1.0f);

				if (actualFighter.GetComponent<LocalDataHolder> ().player) {
					actualFighter.transform.Find ("PersoBackground").GetComponent<Animator> ().Play ("Attacked");
				} else {
					actualFighter.transform.Find ("EnemyBackground").GetComponent<Animator> ().Play ("DamageMonster");
				}

				yield return new WaitForSeconds (1.0f);

				if (actualFighter.GetComponent<LocalDataHolder> ().player) {
					actualFighter.transform.Find ("PersoBackground").GetComponent<Animator> ().Play ("Idle");
				} else {
					actualFighter.transform.Find ("EnemyBackground").GetComponent<Animator> ().Play ("IdleMonster");
				}

				//do the damages to the one affected by the effect, which is the guy playing in this case.
				actualFighter.GetComponent<LocalDataHolder> ().looseLife (-PS.statusDamage, false);
				PS.statusTurnLeft--;
			} else if (PS.statusType == Status.StatusType.TemporaryLifed) {
				actualFighter.GetComponent<LocalDataHolder> ().looseLife (-PS.statusDamage, false);
				PS.statusTurnLeft--;
			}
		}

		//si l'enemi ou le joueur meurt d'un effet.
		if (actualFighter.GetComponent<LocalDataHolder> ().health <= 0) {
			actualFighter.transform.Find ("EffectLayer").GetComponent<Animator> ().Play (effect_Controller.effect_List[0]);
			nextTurn_Controller.NextTurn ();
		} else {

			//remove the effect if this one is expired
			if (PS.statusTurnLeft <= 0) {
				if (actualFighter.GetComponent<LocalDataHolder> ().player) {
					explo_Data.dungeonData.TempFighterObject [actualFighter.GetComponent<LocalDataHolder> ().localIndex].playerStatus.RemoveAt (index - 1);
					print ("removed effect : " + PS.statusName);
				} else {
					explo_Data.dungeonData.TempFighterObject [actualFighter.GetComponent<LocalDataHolder> ().localIndex + 4].playerStatus.RemoveAt (index - 1);
					print ("removed effect : " + PS.statusName);
				}
			}

			yield return new WaitForSeconds (1.0f);

			//wait for effect to attack player
			actualFighter.transform.Find ("EffectLayer").GetComponent<Animator> ().Play (effect_Controller.effect_List[0]);



			//reduce maximum of effect to deal with start of the turn.
			index -= 1;
			print ("effect left : " + index);

			//do damages or heal depending on dot

			//redo for the next dot that the player has
			if (index > 0) {
				StartCoroutine (waitForEffectEndedStartOfTurn (index));
			} else {
				BS.ContinueFightAfterEffect ();
				DisplayUIToolTip ();
			}
		}
	}
}
