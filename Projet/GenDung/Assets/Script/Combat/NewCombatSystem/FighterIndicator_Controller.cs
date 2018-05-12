using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterIndicator_Controller : MonoBehaviour {

	BattleSystem BS;
	GameObject indicator_Battle;

	public void Start(){
		BS = GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ();
		indicator_Battle = GameObject.Find ("Pastille");
	}

	public void SetArrow () {

		Vector3 actualPosition = new Vector3 (0,0,0);
		indicator_Battle.GetComponent<RectTransform> ().SetParent (BS.FighterList [BS.actuallyPlaying].transform.Find("Shadow"));
		indicator_Battle.GetComponent<RectTransform> ().localPosition = actualPosition;

		for (int i = 0; i < BS.FighterList.Count; i++) {
			BS.FighterList [i].GetComponent<LocalDataHolder> ().UpdateUiOrderOrder (false);
		}
		BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().UpdateUiOrderOrder (true);
	}
}
