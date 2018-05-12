using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellPropreties : MonoBehaviour {

	BattleSystem BS;
	SpellHolder_Controller spellHolder_Controller;

	public SpellObject spellObject;
	GameObject scriptBattleHolder;

	void Start(){
		scriptBattleHolder = GameObject.Find ("BattleSystem/ScriptBattle");

		BS = scriptBattleHolder.GetComponent<BattleSystem> ();
		spellHolder_Controller = scriptBattleHolder.GetComponent<SpellHolder_Controller> ();
	}

	public void attackEnemy(){
		print ("I'll be attacking with spell " + spellObject.spellName);

		for (int i = 0; i < BS.FighterList.Count; i++) {
			if (!BS.FighterList [i].GetComponent<LocalDataHolder> ().player) {
				BS.FighterList [i].transform.Find ("Shadow/Pastille2").GetComponent<Image> ().enabled = false;
			}
		}

		//let only interactable object (monsters, or player if heal)
		AttackMode (true);

		BS.SelectedSpellObject = spellObject;

		//activate for all the enemies the pastille to show where you can click
		for (int i = 0; i < BS.FighterList.Count; i++) 
		{
			if (BS.SelectedSpellObject.spellType == SpellObject.SpellType.Enemy) {
				if (!BS.FighterList [i].GetComponent<LocalDataHolder> ().player) {
					if (!BS.FighterList [i].GetComponent<LocalDataHolder> ().dead) {
						//you can see where you click on the enemies
						BS.FighterList [i].transform.Find ("Shadow/Pastille2").GetComponent<Image> ().enabled = true;
					}
				}
			} 
			else if (BS.SelectedSpellObject.spellType == SpellObject.SpellType.Ally) 
			{
				if (BS.FighterList [i].GetComponent<LocalDataHolder> ().player) {
					if (!BS.FighterList [i].GetComponent<LocalDataHolder> ().dead) {
						//you can see where you click on the allies
						//BS.FighterList [i].transform.Find ("Shadow/Pastille2").GetComponent<Image> ().enabled = true;
					}
				}
			} 
			else if (BS.SelectedSpellObject.spellType == SpellObject.SpellType.Self) 
			{
				//you can see where you click on yourself
				//BS.FighterList [BS.actuallyPlaying].transform.Find ("Shadow/Pastille2").GetComponent<Image> ().enabled = true;
			}
		}
	}

	void AttackMode (bool attackMod){
		BS.attackMode = attackMod;

		GameObject.Find ("NextPanel").GetComponent<Image> ().enabled = !attackMod;
		GameObject.Find ("NextPanel").GetComponent<Button> ().enabled = !attackMod;
		GameObject.Find ("NextPanel/NextText").GetComponent<Text> ().enabled = !attackMod;

	}

	public void clickAway(){
		AttackMode (false);

		//make sure for the enemies to not show if they are not dead the fact that you can click on them
		for (int i = 0; i < BS.FighterList.Count; i++) {
			if (!BS.FighterList [i].GetComponent<LocalDataHolder> ().player) {
				BS.FighterList [i].transform.Find ("Shadow/Pastille2").GetComponent<Image> ().enabled = false;
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

	public void SendSpellToSpellHolderToolTip(){
		spellHolder_Controller.spell = spellObject;
	}

	IEnumerator UpdateSpells (bool onOrOff) {
		yield return new WaitForSeconds (0.1f);

		if (onOrOff) {
			if (BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().player
			    && BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().actionPointPlayer > 0) {
				InteractableSpell (true);
			} else {
				InteractableSpell (false);
			}

			if (spellObject != null) {
				if (BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().actionPointPlayer < spellObject.spellCost) {
					InteractableSpell (false);
				}
			}
			StartCoroutine (UpdateSpells (true));
		} else {
			StartCoroutine (UpdateSpells (false));
		}
			
	}
}
