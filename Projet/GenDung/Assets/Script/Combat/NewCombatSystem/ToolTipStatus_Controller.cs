using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipStatus_Controller : MonoBehaviour {

	LocalDataHolder LDH;
	GameObject UIBattleOrder;

	Transform PanelStatus;

	public void start(){
		LDH = this.GetComponent<LocalDataHolder> ();
		UIBattleOrder = LDH.UiOrderObject;

		PanelStatus = UIBattleOrder.transform.Find ("ToolTipAlpha/TooltipPanel/PanelStatus");
	}

	public void AddEffectToUI(Status status){
		
		GameObject UIEffect;

		UIEffect = Instantiate (Resources.Load ("UI_Interface/StatusRepresentationUI"), PanelStatus) as GameObject;

		UIEffect.GetComponent<Image> ().sprite = status.Icon;
	}

	public void RemoveEffect(){
		int child = PanelStatus.childCount;

		if (child != 0) {
			for (int i = 0; i < child; i++) {
				PanelStatus.GetChild (i).transform.gameObject.SetActive (false);
			}
		}
	}
}
