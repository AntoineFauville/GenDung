using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_Room_TreasureController : MonoBehaviour {

	private static Explo_Room_TreasureController instance;
	private GameObject tresorCanvas;
    Explo_Room_Treasure explo_Room_Treasure;
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

		if (GameObject.Find ("ExploGridCanvas").transform.Find ("PanelGrid/Tile_" + Explo_GridController.Instance.Grid.ExploTiles [X, Y].x + "_" + Explo_GridController.Instance.Grid.ExploTiles [X, Y].y).GetComponent<ExploTileController> ().isAlreadyDiscovered == false) {
			animTresorImageAnimator.Play ("Normal");
		}
	}

	public void OpenTreasure()
	{
		if (GameObject.Find ("ExploGridCanvas").transform.Find ("PanelGrid/Tile_" + Explo_GridController.Instance.Grid.ExploTiles [X, Y].x + "_" + Explo_GridController.Instance.Grid.ExploTiles [X, Y].y).GetComponent<ExploTileController> ().isAlreadyDiscovered == false) {
            //RandomPicker ();
            if (explo_Room_Treasure.Trap)
                Itsatrap();
            else
                OpenChest();
			animTresorImageAnimator.Play ("Highlighted");
			GameObject.Find ("ExploGridCanvas").transform.Find ("PanelGrid/Tile_" + Explo_GridController.Instance.Grid.ExploTiles [X, Y].x + "_" + Explo_GridController.Instance.Grid.ExploTiles [X, Y].y).GetComponent<ExploTileController> ().isAlreadyDiscovered = true;
		}

        GameObject.Find("ExploUnit(Clone)/Unit").GetComponent<ExploUnitController>().ResetMovement();


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

	public void Itsatrap() // Status Effect 
	{
		//if not locked
		logT.AddLogLine("It's a trap");
        int playerTouched = Random.Range(0, 3);

        while (explo_Room_Treasure.Dungeon.Data.Players[playerTouched].Dead)
        {
            playerTouched = Random.Range(0, 3);
        }

        explo_Room_Treasure.Dungeon.Data.Players[playerTouched].ChangeHealth(5);
        logT.AddLogLine(explo_Room_Treasure.Dungeon.Data.Players[playerTouched].Name + " has lost 5 HP");
	}

	public void OpenChest()
	{
		//if not locked
		logT.AddLogLine ("Chest Opened !");
        //GenerateGoldRand ();
        logT.AddLogLine("You gained : " + explo_Room_Treasure.GoldAmount + " Gold !");
        GameObject.Find("DontDestroyOnLoad").GetComponent<Explo_DataController>().ModifyGold(explo_Room_Treasure.GoldAmount);
    }

	public void ClosingTab()
	{
		tresorCanvas.GetComponent<Canvas>().sortingOrder -= 40;
        GameObject.Find("ExploUnit(Clone)/Unit").GetComponent<ExploUnitController>().ResetMovement();
        //lock local closing to not trigger IF locked
    }

	public static Explo_Room_TreasureController Instance
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

    public Explo_Room_Treasure Explo_Room_Treasure
    {
        get
        {
            return explo_Room_Treasure;
        }

        set
        {
            explo_Room_Treasure = value;
        }
    }
}
