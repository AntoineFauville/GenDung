using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Explo_Room_ExitController : MonoBehaviour {

	private static Explo_Room_ExitController instance;
	private GameObject endCanvas;
    Explo_DungeonController explo_Dungeon;

	LogGestionTool logT;

	void CreateInstance()
	{
		if (instance != null)
		{
			Debug.Log("There should never have two world controllers.");
		}
		instance = this;
	}

	void Start ()
	{
		logT = GameObject.Find ("DontDestroyOnLoad").GetComponent<LogGestionTool> ();
		endCanvas = GameObject.Find("CanvasEndExplo");
		endCanvas.GetComponent<Canvas>().sortingOrder = 38;
        explo_Dungeon = GameObject.Find("ScriptBattle").GetComponent<Explo_DungeonController>();
    }

	public void LinkToRoom()
	{
		endCanvas.GetComponent<Canvas>().sortingOrder += 40;// Pass the fightCanvas 
		logT.AddLogLine("Leaving already ?");

		GameObject.Find("TextEndDungeon").GetComponent<Text>().text = "You reached the end of the dungeon ! "+ '\n' + '\n' +  "You found nearby the end a treasure containing : " + explo_Dungeon.Dungeon.Data.GoldGained + " gold." + '\n' + '\n' + "What do you want to do now that you found the treasure ? ";
	}

	public void ContinueExploration()
	{
		endCanvas.GetComponent<Canvas>().sortingOrder -= 40;// Pass the fightCanvas 
		logT.AddLogLine("You.. you are greedy I see.. ?");
        GameObject.Find("ExploUnit(Clone)/Unit").GetComponent<ExploUnitController>().ResetMovement();
    }

	public void EndExploration()
	{
        GameObject.Find("DontDestroyOnLoad").GetComponent<Explo_DataController>().SendToSave();

		//need to add dungeon done etc etc

		if (GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.dungeonUnlocked [MapController.Instance.dungeonIndex] == false) 
		{
			GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.dungeonUnlocked[MapController.Instance.dungeonIndex] = true;
			MapController.Instance.UnlockNextDungeon ();
		}

        SceneManager.LoadScene("Map"); // load map
    }

	public static Explo_Room_ExitController Instance
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
