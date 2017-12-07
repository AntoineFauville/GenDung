using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
		/* Ajoute de manière dynamique mon GridController à l'objet (Pas de modification de scène nécessaire ;) )  */

		/**/if (SceneManager.GetActiveScene().name == "Dungeon" && !instantiateCombatGrid)
		{
			combat_grid = Instantiate (Resources.Load("UI_Interface/CombatGridPrefab")) as GameObject;
			combat_grid.SetActive (true);
			instantiateCombatGrid = true;

            for (int i = 0; i < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedSizeOfTheTeam; i++)
            {
                GameObject.Find("CharacterBG").GetComponent<Image>().enabled = false;
                GameObject.Find("CharacterShadow").GetComponent<Image>().enabled = false;
            }

            canvasTemp.GetComponent<Canvas> ().enabled = false;
		} /**/
	}

	public void FinishedCombat () {
		instantiateCombatGrid = false;
		canvasTemp.GetComponent<Canvas> ().enabled = true;
		//combat_grid.SetActive (false);
		GameObject.FindGameObjectWithTag ("Unit").SetActive (false);
		//GameObject.FindGameObjectWithTag ("GridCanvas").SetActive (false);
        Destroy(GameObject.FindGameObjectWithTag("GridCanvas").gameObject);
        Destroy(GameObject.Find("CombatGridPrefab(Clone)").gameObject);

        GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().InstantiatedCombatModule = false;

        for (int i = 0; i < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedSizeOfTheTeam; i++)
        {
            GameObject.Find("CharacterBG").GetComponent<Image>().enabled = true;
            GameObject.Find("CharacterShadow").GetComponent<Image>().enabled = true;
        }
    }
}
