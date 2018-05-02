﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellPropreties : MonoBehaviour {

	public SpellObject spellObject;

	public void attackEnemy(){
		print ("I'll be attacking with spell " + spellObject.spellName);

		//let only interactable object (monsters, or player if heal)
		AttackMode (true);

		GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject = spellObject;

		//activate for all the enemies the pastille to show where you can click
		for (int i = 0; i < GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList.Count; i++) 
		{
			if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellType == SpellObject.SpellType.Enemy) {
				if (!GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [i].GetComponent<LocalDataHolder> ().player) {
					if (!GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [i].GetComponent<LocalDataHolder> ().dead) {
						//you can see where you click on the enemies
						GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [i].transform.Find ("Shadow/Pastille2").GetComponent<Image> ().enabled = true;
					}
				}
			} 
			else if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellType == SpellObject.SpellType.Ally) 
			{
				if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [i].GetComponent<LocalDataHolder> ().player) {
					if (!GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [i].GetComponent<LocalDataHolder> ().dead) {
						//you can see where you click on the allies
						//GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [i].transform.Find ("Shadow/Pastille2").GetComponent<Image> ().enabled = true;
					}
				}
			} 
			else if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellType == SpellObject.SpellType.Self) 
			{
				//you can see where you click on yourself
				//GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().actuallyPlaying].transform.Find ("Shadow/Pastille2").GetComponent<Image> ().enabled = true;
			}
		}
	}

	void AttackMode (bool attackMod){
		GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().attackMode = attackMod;

		GameObject.Find ("NextPanel").GetComponent<Image> ().enabled = !attackMod;
		GameObject.Find ("NextPanel").GetComponent<Button> ().enabled = !attackMod;
		GameObject.Find ("NextPanel/NextText").GetComponent<Text> ().enabled = !attackMod;

	}

	public void clickAway(){
		AttackMode (false);

		//make sure for the enemies to not show if they are not dead the fact that you can click on them
		for (int i = 0; i < GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList.Count; i++) {
			if (!GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [i].GetComponent<LocalDataHolder> ().player) {
				GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [i].transform.Find ("Shadow/Pastille2").GetComponent<Image> ().enabled = false;
			}
		}
	}

	public void StartPersoUpdate (bool onOrOff) {
		StartCoroutine (UpdateSpells (onOrOff));
	}

	void InteractableSpell (bool interact){
		this.GetComponent<Button> ().interactable = interact;
		if (!interact) {
			this.GetComponent<Image> ().color = new Color (150, 150, 150, 1);
		} else {
			this.GetComponent<Image> ().color = new Color (250,250,250,1);
		}

	}

	IEnumerator UpdateSpells (bool onOrOff) {
		yield return new WaitForSeconds (0.1f);

		if (onOrOff) {
			if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().actuallyPlaying].GetComponent<LocalDataHolder> ().player
			    && GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().actuallyPlaying].GetComponent<LocalDataHolder> ().actionPointPlayer > 0) {
				InteractableSpell (true);
			} else {
				InteractableSpell (false);
			}

			if (spellObject != null) {
				if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().actuallyPlaying].GetComponent<LocalDataHolder> ().actionPointPlayer < spellObject.spellCost) {
					InteractableSpell (false);
				}
			}
			StartCoroutine (UpdateSpells (true));
		} else {
			StartCoroutine (UpdateSpells (false));
		}
			
	}
}