using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Button_Controller : MonoBehaviour {

	ProgressionSystem progressionSystem;

	public Pdungeon pDungeon;

	// Use this for initialization
	void Start () {
		progressionSystem = GameObject.FindObjectOfType<ProgressionSystem> ();
	}

	public void ExploreDungeon(){

		progressionSystem.CalculateOverallPower ();		
		progressionSystem.ExploreDung (pDungeon);
	}

}
