using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CombatGestion : MonoBehaviour {

    private static CombatGestion instance;

    bool instantiateCombatGrid;

	GameObject 
	canvasTemp,
	combat_grid;

	Explo_FightRoom EF;

    void CreateInstance()
    {
        if (instance != null)
        {
            Debug.Log("There should never have two world controllers.");
        }
        instance = this;
    }

    void Start(){
		canvasTemp = GameObject.FindGameObjectWithTag ("canvasInDungeon");
		EF = GameObject.Find ("ExploGridPrefab").GetComponent<Explo_FightRoom>();
	}

	void Update (){
		if (Input.GetButtonDown ("Cancel")) {
            PostCombatController.Instance.CleanEndBattle();
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

            //GameObject.Find("CanvasUIDungeon(Clone)/Panel/Panel/ActualPlayerPanel").GetComponent<CanvasGroup>().alpha = 1;
            //GameObject.Find("CanvasUIDungeon(Clone)/Panel/Panel/Spells").GetComponent<CanvasGroup>().alpha = 1;

        } /**/

        combat_grid = Instantiate(Resources.Load("UI_Interface/CombatGridPrefab")) as GameObject;
        combat_grid.SetActive(true);
        instantiateCombatGrid = true;

        //for (int i = 0; i < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedSizeOfTheTeam; i++)
        //{
          //  GameObject.Find("CharacterBG").GetComponent<Image>().enabled = false;
            //GameObject.Find("CharacterShadow").GetComponent<Image>().enabled = false;
        //}

        canvasTemp.GetComponent<Canvas>().enabled = false;

    }

	public void FinishedCombat () {

		instantiateCombatGrid = false;
		GameObject.FindGameObjectWithTag ("Unit").SetActive (false);

		EF.CleanFinishedFightRoom ();

		/*

		canvasTemp.GetComponent<Canvas> ().enabled = true;
		//combat_grid.SetActive (false);

		//GameObject.FindGameObjectWithTag ("GridCanvas").SetActive (false);
        Destroy(GameObject.FindGameObjectWithTag("GridCanvas").gameObject);
        Destroy(GameObject.Find("CombatGridPrefab(Clone)").gameObject);

        GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().InstantiatedCombatModule = false;

		

        for (int i = 0; i < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedSizeOfTheTeam; i++)
        {
            GameObject.Find("CharacterBG").GetComponent<Image>().enabled = true;
            GameObject.Find("CharacterShadow").GetComponent<Image>().enabled = true;
        }
*/
        GameObject.Find("CanvasUIDungeon(Clone)/Panel/Panel/ActualPlayerPanel").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("CanvasUIDungeon(Clone)/Panel/Panel/Spells").GetComponent<CanvasGroup>().alpha = 0;
    }

    public static CombatGestion Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }
}
