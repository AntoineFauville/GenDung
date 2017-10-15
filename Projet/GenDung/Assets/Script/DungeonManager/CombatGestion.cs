using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatGestion : MonoBehaviour {

	bool instantiateCombatGrid;

	GameObject 
	canvasTemp,
	combat_grid;

	void Start(){
		canvasTemp = GameObject.FindGameObjectWithTag ("canvasInDungeon");
	}

	void Update (){
		if (Input.GetButtonDown ("Cancel")) {
			FinishedCombat ();
		}
	}

	public void InstantiateCombatPrefab () {
		/* Ajoute de manière dynamique mon DungeonController à l'objet (Pas de modification de scène nécessaire ;) )  */

		/**/if (SceneManager.GetActiveScene().name == "Dungeon" && !instantiateCombatGrid)
		{
			combat_grid = Instantiate (Resources.Load("UI_Interface/CombatGridPrefab")) as GameObject;
			combat_grid.SetActive (true);
			instantiateCombatGrid = true;

			canvasTemp.GetComponent<Canvas> ().enabled = false;
		} /**/
	}

	public void FinishedCombat () {
		instantiateCombatGrid = false;
		canvasTemp.GetComponent<Canvas> ().enabled = true;
		combat_grid.SetActive (false);
		GameObject.FindGameObjectWithTag ("Unit").SetActive (false);
		GameObject.FindGameObjectWithTag ("GridCanvas").SetActive (false);

	}
}
