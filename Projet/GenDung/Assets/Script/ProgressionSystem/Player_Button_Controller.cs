using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Button_Controller : MonoBehaviour {

	ProgressionSystem PS;

	public P_Player p_Player;

	// Use this for initialization
	void Start () {
		PS = GameObject.FindObjectOfType<ProgressionSystem> ();
	}

	public void UpgradePlayerPower(){
		
		PS.CalculateOverallPower ();

		PS.UpgradePlayer (p_Player);
	}
}
