using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonButtonController : MonoBehaviour {

	public ProgressionSystem progressionSystem;

	public Pdungeon LocalPDungeon;

	public Text DungeonDescriptionText;
	public GameObject DungeonButton;

	public void ExploreDungeon(){

		progressionSystem.CalculateOverallPower ();		
		progressionSystem.ExploreDung (LocalPDungeon);
	}

}
