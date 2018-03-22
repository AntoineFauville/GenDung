using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Explo_ExitRoom : MonoBehaviour {

	private static Explo_ExitRoom instance;
	private GameObject endCanvas;

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
	}

	public void LinkToRoom()
	{
		endCanvas.GetComponent<Canvas>().sortingOrder += 40;// Pass the fightCanvas 
		logT.AddLogLine("Leaving already ?");

        GameObject.Find("Text EndDungeon").GetComponent<Text>().text = "You reached the end of the dungeon ! "+ '\n' + '\n' +  "You found nearby the end a treasure containing : " + GameObject.Find("DontDestroyOnLoad").GetComponent<Explo_Data>().exploGold + " gold." + '\n' + '\n' + "What do you want to do now that you found the treasure ? ";
	}

	public void ContinueExploration()
	{
		endCanvas.GetComponent<Canvas>().sortingOrder -= 40;// Pass the fightCanvas 
		logT.AddLogLine("You.. you are greedy I see.. ?");
        GameObject.Find("ExploUnit(Clone)/Unit").GetComponent<ExploUnitController>().ResetMovement();
    }

	public void EndExploration()
	{
        GameObject.Find("DontDestroyOnLoad").GetComponent<Explo_Data>().SendToSave();

		//need to add dungeon done etc etc

		if (GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.dungeonUnlocked [MapController.Instance.dungeonIndex] == false) 
		{
			GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.dungeonUnlocked[MapController.Instance.dungeonIndex] = true;
			MapController.Instance.UnlockNextDungeon ();
		}

        SceneManager.LoadScene("Map"); // load map
    }

	public static Explo_ExitRoom Instance
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
