using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explo_FightRoom : MonoBehaviour
{
    private static Explo_FightRoom instance;
    private GameObject fightCanvas;
    private GameObject fightRoomCanvas;

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
        // Change UI FightRoom
        // Combat Module Loading
        // End of Combat 
    }

    public void SetFightRoom()
    {
        fightRoomCanvas.transform.Find("Canvas").GetComponent<Canvas>().sortingOrder = 100;
        fightCanvas.GetComponent<Canvas>().sortingOrder = 0;

        fightRoomCanvas.transform.Find("Canvas/Panel/background of the room").GetComponent<Image>().sprite = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().exploDungeonList.explorationDungeons[MapController.Instance.DungeonIndex].rooms[0].back;
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
}
