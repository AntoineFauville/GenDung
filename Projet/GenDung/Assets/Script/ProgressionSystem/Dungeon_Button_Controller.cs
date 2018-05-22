using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Button_Controller : MonoBehaviour {

	ProgressionSystem PS;

	public P_Dungeon p_Dungeon;

	// Use this for initialization
	void Start () {
		PS = GameObject.FindObjectOfType<ProgressionSystem> ();
	}

	public void ExploreDungeon(){

		PS.CalculateOverallPower ();		
		PS.ExploreDung (p_Dungeon);
	}

}
