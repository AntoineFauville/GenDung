﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explo_FightRoom : MonoBehaviour
{
    private static Explo_FightRoom instance;
	private int roomRand;
    private GameObject fightCanvas;
    private GameObject fightRoomCanvas;

    private CombatGestion cmbtGestion;

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
        fightCanvas = GameObject.Find("FightRoomUI");
        fightRoomCanvas = GameObject.Find("Room1");

        fightCanvas.GetComponent<Canvas>().sortingOrder = 0;
        fightRoomCanvas.transform.Find("Canvas").GetComponent<Canvas>().sortingOrder = 0;
    }

    public void LinkToRoom()
    {
        // Check if already done.
        // ! 
        fightCanvas.GetComponent<Canvas>().sortingOrder = 100; // Pass the fightCanvas 
        // Gérer Effet Pop-Up
        // Combat Module Loading
        // End of Combat 
    }

    public void SetFightRoom()
    {
		GameObject.Find("CanvasUIDungeon(Clone)/Panel/Panel/ActualPlayerPanel").GetComponent<CanvasGroup>().alpha = 1;
		GameObject.Find("CanvasUIDungeon(Clone)/Panel/Panel/Spells").GetComponent<CanvasGroup>().alpha = 1;


		//print ("le room rand est de " + roomRand);
        fightRoomCanvas.transform.Find("Canvas").GetComponent<Canvas>().sortingOrder = 49;
        fightCanvas.GetComponent<Canvas>().sortingOrder = 0;
 
        roomRand = Random.Range(0, GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().exploDungeonList.explorationDungeons[MapController.Instance.DungeonIndex].rooms.Count);
		//print ("le room rand est de " + roomRand);
		actualFightRoom = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().exploDungeonList.explorationDungeons[MapController.Instance.DungeonIndex].rooms[roomRand];
        fightRoomCanvas.transform.Find("Canvas/Panel/background of the room").GetComponent<Image>().sprite = actualFightRoom.back;
    }

	public void CleanFinishedFightRoom () {

		print ("room. over. stop.");
		fightCanvas.GetComponent<Canvas>().sortingOrder = 100;
		fightRoomCanvas.transform.Find("Canvas").GetComponent<Canvas>().sortingOrder = 0;

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
