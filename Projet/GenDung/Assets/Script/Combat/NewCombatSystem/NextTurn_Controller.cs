using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextTurn_Controller : MonoBehaviour {

	BattleSystem BS;
	GameObject next_Button;
	FighterIndicator_Controller indicator_Controller;
	SpellHolder_Controller spellHolder_Controller;
	StartTurnEffect_Controller startTurnEffect_Controller;

	GameObject scriptBattleHolder;

	public void Start(){
		scriptBattleHolder = GameObject.Find ("BattleSystem/ScriptBattle");

		BS = scriptBattleHolder.GetComponent<BattleSystem> ();
		indicator_Controller = scriptBattleHolder.GetComponent<FighterIndicator_Controller> ();
		spellHolder_Controller = scriptBattleHolder.GetComponent<SpellHolder_Controller> ();
		startTurnEffect_Controller = scriptBattleHolder.GetComponent<StartTurnEffect_Controller> ();

		next_Button = GameObject.Find ("NextPanel");
	}

	public void NextTurn() // Call by the "Next" (BattleSystem/Panel/NextPanel) Button into the Scene.
	{
		HideShowNext(false);

		BS.actuallyPlaying++;
		if (BS.actuallyPlaying >= BS.FighterList.Count) {
			BS.actuallyPlaying = 0;
		}



		if(BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().dead)
		{
			NextTurn ();
		}
		else
		{
			//indicator_Controller.SetArrow();
			//BS.resetActionPoint(BS.actuallyPlaying);
			startTurnEffect_Controller.ManageStatusEffects ();
			//gere les effets et ensuite lance le reste de la fight
			spellHolder_Controller.UpdateFighterPanel();
		}
	}

	public void HideShowNext (bool hide){
		next_Button.GetComponent<Image> ().enabled = hide;
		next_Button.GetComponent<Button> ().interactable = hide;
		next_Button.GetComponent<Button> ().enabled = hide;
		GameObject.Find ("NextPanel/NextText").GetComponent<Text> ().enabled = hide;
	}
}
