using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell_Controller : MonoBehaviour {

	BattleSystem BS;

	// Use this for initialization
	void Start () {
		BS = this.GetComponent<BattleSystem> ();
	}

	public void SetupFighterPanel () {
		if (BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().player) {
			SetSpellLinks (true);
		}
	}

	public void SetSpellLinks (bool onOrOff) {

		for (int i = 0; i < 3; i++)
		{
			if (onOrOff) {
				GameObject.Find ("Button_Spell_" + i).GetComponent<Image> ().sprite = BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().characterObject.SpellList [i].spellIcon;
				GameObject.Find ("Button_Spell_" + i).GetComponent<SpellPropreties> ().spellObject = BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().characterObject.SpellList [i];
			}

			GameObject.Find ("Button_Spell_" + i).GetComponent<SpellPropreties> ().StartPersoUpdate (onOrOff);
		}
	}
}
