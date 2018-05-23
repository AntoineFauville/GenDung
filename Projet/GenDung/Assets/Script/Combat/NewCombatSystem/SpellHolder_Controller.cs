using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellHolder_Controller : MonoBehaviour {

	BattleSystem BS;
	Spell_Controller spell_Controller;

	GameObject fighter_Panel;
	GameObject toolTipSpell;

	Text textToolTipSpell;
	Text textToolTipDescription;
	Text textToolTipExplanation;
	public SpellObject spell;

	GameObject scriptBattleHolder;

	// Use this for initialization
	void Start () {
		scriptBattleHolder = GameObject.Find ("BattleSystem/ScriptBattle");

		BS = scriptBattleHolder.GetComponent<BattleSystem> ();
		fighter_Panel = GameObject.Find ("FighterPanel");
		toolTipSpell = GameObject.Find ("ToolTipSpell");
		textToolTipSpell = toolTipSpell.transform.Find ("ToolTipSpellText").GetComponent<Text>();
		textToolTipDescription = toolTipSpell.transform.Find ("ToolTipSpellDescription").GetComponent<Text>();
		textToolTipExplanation = toolTipSpell.transform.Find ("ToolTipSpellExplanation").GetComponent<Text>();
		spell_Controller = this.GetComponent<Spell_Controller> ();

		//ToolTipShow (false);
	}

	//public void UpdateFighterPanel () {
	//	if(BS.FighterList[BS.actuallyPlaying].GetComponent<LocalDataHolder> ().player){
	//		fighter_Panel.GetComponent<RectTransform> ().localPosition = new Vector3 (350,-200,0);
	//		spell_Controller.SetSpellLinks (true);
	//	} else {
	//		fighter_Panel.GetComponent<RectTransform> ().localPosition = new Vector3 (0,-500,0);
	//	}
	//}

	//public void ToolTipShow(bool show){
	//	if (!show) {
	//		toolTipSpell.GetComponent<CanvasGroup> ().alpha = 0;
	//	} else {
	//		toolTipSpell.GetComponent<CanvasGroup> ().alpha = 1;
	//	}
	//}

	//public void UpdateToolTip(){
	//	if (spell != null) {
	//		textToolTipSpell.text = "<size=15><b>" + spell.spellName.ToString () + "</b></size>";

	//		if (spell.spellOccurenceType == SpellObject.SpellOccurenceType.NoTurn) {
	//			textToolTipDescription.text = "Damage : " + spell.spellDamage.ToString () + " Cost : " + spell.spellCost.ToString ()
	//				+ '\n' + "This spell does " + spell.spellLogicType.ToString () + "." + '\n' + "Target : " + spell.spellTargetType.ToString ()
	//				+ ".";
	//		} else {
	//			textToolTipDescription.text = "Damage : " + spell.spellDamage.ToString () + " Cost : " + spell.spellCost.ToString ()
	//				+ '\n' + "This spell does " + spell.spellLogicType.ToString () + "." + '\n' + "Target : " + spell.spellTargetType.ToString ()
	//				+ "." + '\n' + "Place " + spell.spellTargetFeedbackAnimationType.ToString () + " on target for " + spell.spellOccurenceType.ToString ();
	//		}
	//		textToolTipExplanation.text = spell.spellDescription.ToString ();
	//		StartCoroutine (waitABit());
	//	} else {
	//		StartCoroutine (waitABit());
	//	}
	//}

	//IEnumerator waitABit(){
	//	yield return new WaitForSeconds (0.1f);
	//	UpdateToolTip ();
	//}
}
