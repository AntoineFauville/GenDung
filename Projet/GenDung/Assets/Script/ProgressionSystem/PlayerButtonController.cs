using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButtonController : MonoBehaviour {

	public ProgressionSystem ProgressionSystem;

	public Pplayer LocalPPlayer;

	public Text PlayerDescriptionText;
	public GameObject PlayerButton;

	public void UpgradePlayerPower(){
		
		ProgressionSystem.CalculateOverallPower ();

		ProgressionSystem.UpgradePlayer (LocalPPlayer);
	}
}
