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
	}

	void AttackMode (bool attackMod){
		GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().attackMode = attackMod;

		GameObject.Find ("NextPanel").GetComponent<Image> ().enabled = !attackMod;
		GameObject.Find ("NextPanel").GetComponent<Button> ().enabled = !attackMod;
		GameObject.Find ("NextPanel/NextText").GetComponent<Text> ().enabled = !attackMod;

	}

	public void clickAway(){
		AttackMode (false);
	}

	void Update (){
		//check if i can click on me or not.
		if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().actuallyPlaying].GetComponent<LocalDataHolder> ().player
		    && GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().actuallyPlaying].GetComponent<LocalDataHolder> ().actionPointPlayer > 0) {
			InteractableSpell (true);
		} else {
			InteractableSpell (false);
		}
	}

	void InteractableSpell (bool interact){
		this.GetComponent<Button> ().interactable = interact;
		if (!interact) {
			this.GetComponent<Image> ().color = new Color (150, 150, 150, 1);
		} else {
			this.GetComponent<Image> ().color = new Color (250,250,250,1);
		}

	}
}
