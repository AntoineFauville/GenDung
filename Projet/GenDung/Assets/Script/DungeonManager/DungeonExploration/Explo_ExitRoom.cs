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
	}

	public void ContinueExploration()
	{
		endCanvas.GetComponent<Canvas>().sortingOrder -= 40;// Pass the fightCanvas 
		logT.AddLogLine("You.. you are greedy I see.. ?");
	}

	public void EndExploration()
	{
		SceneManager.LoadScene("Map"); // load map
		//need to add dungeon done etc etc

		if (GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.dungeonUnlocked [MapController.Instance.dungeonIndex] == false) 
		{
			GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.dungeonUnlocked[MapController.Instance.dungeonIndex] = true;
			MapController.Instance.UnlockNextDungeon ();
		}
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
