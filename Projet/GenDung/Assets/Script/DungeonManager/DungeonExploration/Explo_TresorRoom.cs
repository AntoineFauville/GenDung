using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_TresorRoom : MonoBehaviour {

	private static Explo_TresorRoom instance;
	private GameObject tresorCanvas;

	private Animator animTresorImageAnimator;

	LogGestionTool logT;

	private int X,Y;

	void CreateInstance()
	{
		if (instance != null)
		{
			Debug.Log("There should never have two world controllers.");
		}
		instance = this;
	}

	// Use this for initialization
	void Start () {
		logT = GameObject.Find ("DontDestroyOnLoad").GetComponent<LogGestionTool> ();
		animTresorImageAnimator = GameObject.Find ("AnimImageTresorExplo").GetComponent<Animator> ();
		tresorCanvas = GameObject.Find("CanvasTresorExplo");
		tresorCanvas.GetComponent<Canvas>().sortingOrder = 38;
	}
	
	public void LinkToRoom(int tileX, int tileY)
	{
		tresorCanvas.GetComponent<Canvas>().sortingOrder += 40;
		X = tileX;
		Y = tileY;

		if (GameObject.Find ("ExploGridCanvas").transform.Find ("PanelGrid/Tile_" + Explo_GridController.Instance.Grid.ExploTiles [X, Y].x + "_" + Explo_GridController.Instance.Grid.ExploTiles [X, Y].y).GetComponent<ExploTileController> ().isChestOpenned == false) {
			animTresorImageAnimator.Play ("Normal");
		}
	}

	public void OpenTreasure()
	{
		RandomPicker ();
		animTresorImageAnimator.Play ("Highlighted");
		GameObject.Find ("ExploGridCanvas").transform.Find ("PanelGrid/Tile_" + Explo_GridController.Instance.Grid.ExploTiles [X, Y].x + "_" + Explo_GridController.Instance.Grid.ExploTiles [X, Y].y).GetComponent<ExploTileController> ().isChestOpenned = true;

		//lock local opening chest to this specific tile
	}

	public void RandomPicker()
	{
		int rand = Random.Range (0,100);

		if (rand <= 60) {
			OpenChest ();
		} else {
			Itsatrap ();
		}
	}

	public void Itsatrap()
	{
		//if not locked
		logT.AddLogLine("It's a trap");
	}

	public void OpenChest()
	{
		//if not locked
		logT.AddLogLine("Chest Opened !");
		GenerateGoldRand ();
	}

	public void GenerateGoldRand()
	{
		int randGold = Random.Range (1,GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().exploDungeonList.explorationDungeons[MapController.Instance.DungeonIndex].chestGoldRewardMax);


		logT.AddLogLine("You gained : " + randGold + " Gold !");
	}

	public void ClosingTab()
	{
		tresorCanvas.GetComponent<Canvas>().sortingOrder -= 40;
		//lock local closing to not trigger IF locked
	}

	public static Explo_TresorRoom Instance
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
