using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explo_FightRoom : MonoBehaviour
{
    private static Explo_FightRoom instance;
	private int roomRand;
    private GameObject combatCanvas;
    private GameObject combatUI;
    private GameObject combatRoom;
    private GameObject combatUnit;
    private GameObject combatGrid;

    private GameObject exploCanvas;
    private GameObject exploRoom;
    private GameObject exploUnit;
    private GameObject exploGrid;

    private RoomObject actualFightRoom;

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
        combatCanvas = GameObject.Find("FightRoomUI");
        combatUI = GameObject.Find("CanvasUIDungeon");
        combatRoom = GameObject.Find("Room1");

        exploUnit = GameObject.Find("ExploUnit(Clone)");
        exploGrid = GameObject.Find("ExploGridCanvas");

        combatCanvas.GetComponent<Canvas>().sortingOrder = 0;
    }

    public void LinkToRoom()
    {
        // Check if already done.
        // ! 
        combatCanvas.GetComponent<Canvas>().sortingOrder = 79;// Pass the fightCanvas 
        // Gérer Effet Pop-Up
        // Combat Module Loading
        // End of Combat 
    }

    public void SetFightRoom()
    {
		GameObject.Find("CanvasUIDungeon/Panel/Panel/ActualPlayerPanel").GetComponent<CanvasGroup>().alpha = 1;
		GameObject.Find("CanvasUIDungeon/Panel/Panel/Spells").GetComponent<CanvasGroup>().alpha = 1;


        //print ("le room rand est de " + roomRand);
        combatUI.GetComponent<Canvas>().sortingOrder = 79;
        combatRoom.transform.Find("Canvas").GetComponent<Canvas>().sortingOrder += 40;
        combatCanvas.GetComponent<Canvas>().sortingOrder -= 40;

        exploUnit.GetComponent<Canvas>().sortingOrder -= 40;
        exploGrid.GetComponent<Canvas>().sortingOrder -= 40;

        roomRand = Random.Range(0, GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().exploDungeonList.explorationDungeons[MapController.Instance.DungeonIndex].rooms.Count);
		//print ("le room rand est de " + roomRand);
		actualFightRoom = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().exploDungeonList.explorationDungeons[MapController.Instance.DungeonIndex].rooms[roomRand];
        combatRoom.transform.Find("Canvas/Panel/background of the room").GetComponent<Image>().sprite = actualFightRoom.back;
    }

	public void CleanFinishedFightRoom () {

		print ("room. over. stop.");
        combatCanvas.GetComponent<Canvas>().sortingOrder = 0;
        combatRoom.transform.Find("Canvas").GetComponent<Canvas>().sortingOrder -= 40;

        exploUnit.GetComponent<Canvas>().sortingOrder += 40;
        exploGrid.GetComponent<Canvas>().sortingOrder += 40;

        Destroy(GameObject.Find(""));

    }

    /* Accessors Methods */

    public static Explo_FightRoom Instance
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

	public int RoomRand
	{
		get
		{
			return roomRand;
		}

		set
		{
			roomRand = value;
		}
	}
}
