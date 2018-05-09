using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explo_Room_FightController : MonoBehaviour
{
    private static Explo_Room_FightController instance;
	private int roomRand;
    Explo_Room_Fight explo_Room_Fight;
    private GameObject combatCanvas;
    private GameObject combatUI;
    private GameObject combatRoom;
    private GameObject combatUnit;
    private GameObject combatGrid;

    private GameObject exploCanvas;
    private GameObject exploRoom;
    private GameObject exploUnit;
    private GameObject exploGrid;
	private GameObject exploUI;
	private GameObject battleSystemUI;

    private RoomObject actualFightRoom;

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
		battleSystemUI = GameObject.Find("BattleSystem");

        combatCanvas = GameObject.Find("FightRoomUI");
        combatUI = GameObject.Find("CanvasUIDungeon");
        combatRoom = GameObject.Find("Room1");

		battleSystemUI.GetComponent<Canvas>().sortingOrder = 40;

		combatCanvas.GetComponent<Canvas>().sortingOrder = 39;
		combatUI.GetComponent<Canvas>().sortingOrder = 41;
		combatRoom.transform.Find("Canvas").GetComponent<Canvas>().sortingOrder = 30;

        exploUnit = GameObject.Find("ExploUnit(Clone)"); //set canvas when instantiated
        exploGrid = GameObject.Find("ExploGridCanvas"); //set canvas when instantiated
		exploUI = GameObject.Find("CanvasUIExplo"); 

		exploUI.GetComponent<Canvas>().sortingOrder = 79;

		logT = GameObject.Find ("DontDestroyOnLoad").GetComponent<LogGestionTool> ();
    }

    public void LinkToRoom()
    {
		print ("heyheyhey");

        combatCanvas.GetComponent<Canvas>().sortingOrder = 79;// Pass the fightCanvas 

		for (int i = 0; i < explo_Room_Fight.FoesList.Count; i++) {
			
			GameObject enemyPanelUI;

			enemyPanelUI = Instantiate(Resources.Load("UI_Interface/EnemiesPanelUI"), GameObject.Find ("FightRoomUI/PanelBackground/PanelBackground2/FightPresentationsUI/PanelEnemies").transform) as GameObject;

            enemyPanelUI.transform.Find("IconMask/Icon").GetComponent<Image>().sprite = explo_Room_Fight.FoesList[i].EntitiesSprite;

            if (explo_Room_Fight.FoesList[i].EntitiesAnimator != null)
                enemyPanelUI.transform.Find("IconMask/Icon").GetComponent<Animator>().runtimeAnimatorController = explo_Room_Fight.FoesList[i].EntitiesAnimator;

        }
	}

    public void SetFightRoom()
    {

		for (int i = 0; i < explo_Room_Fight.FoesList.Count ; i++) {

			GameObject.Find ("EnemiesPanelUI(Clone)").SetActive (false);
		}

		logT.AddLogLine ("Let the fight begin");

		battleSystemUI.GetComponent<Canvas>().sortingOrder += 40;

        combatUI.GetComponent<Canvas>().sortingOrder += 40;
        combatRoom.transform.Find("Canvas").GetComponent<Canvas>().sortingOrder += 40;
        combatCanvas.GetComponent<Canvas>().sortingOrder -= 40;

        exploUnit.GetComponent<Canvas>().sortingOrder -= 40;
        exploGrid.GetComponent<Canvas>().sortingOrder -= 40;
		exploUI.GetComponent<Canvas>().sortingOrder -= 40;

        roomRand = Random.Range(0, explo_Room_Fight.Dungeon.Data.Rooms.Count);
        combatRoom.transform.Find("Canvas/Panel/background of the room").GetComponent<Image>().sprite = explo_Room_Fight.Background;
    }

	public void CleanFinishedFightRoom () {

		logT.AddLogLine ("Let me clean that for you, now explore again, fool");

		battleSystemUI.GetComponent<Canvas>().sortingOrder -= 40;

		combatUI.GetComponent<Canvas>().sortingOrder -= 40;
        combatRoom.transform.Find("Canvas").GetComponent<Canvas>().sortingOrder -= 40;
		combatCanvas.GetComponent<Canvas>().sortingOrder = 39;


        exploUnit.GetComponent<Canvas>().sortingOrder += 40;
        exploGrid.GetComponent<Canvas>().sortingOrder += 40;
		exploUI.GetComponent<Canvas>().sortingOrder += 40;

        exploUnit.transform.Find("Unit").GetComponent<ExploUnitController>().ResetMovement();
    }

    /* Accessors Methods */

    public static Explo_Room_FightController Instance
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

    public Explo_Room_Fight Explo_Room_Fight
    {
        get
        {
            return explo_Room_Fight;
        }

        set
        {
            explo_Room_Fight = value;
        }
    }
}
