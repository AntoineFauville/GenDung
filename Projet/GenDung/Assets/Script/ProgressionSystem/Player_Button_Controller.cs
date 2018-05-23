using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Button_Controller : MonoBehaviour {

	ProgressionSystem progressionSystem;

	public Pplayer pPlayer;

	// Use this for initialization
	void Start () {
		progressionSystem = GameObject.FindObjectOfType<ProgressionSystem> ();
	}

	public void UpgradePlayerPower(){
		
		progressionSystem.CalculateOverallPower ();

		progressionSystem.UpgradePlayer (pPlayer);
	}
}
