using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonButtonController : MonoBehaviour {

	public ProgressionSystem ProgressionSystem;

	public Pdungeon LocalPDungeon;

	public Text DungeonDescriptionText;
	public GameObject DungeonButton;

	public void ExploreDungeon(){

		ProgressionSystem.CalculateOverallPower ();		
		ProgressionSystem.ExploreDung (LocalPDungeon);
	}

}
