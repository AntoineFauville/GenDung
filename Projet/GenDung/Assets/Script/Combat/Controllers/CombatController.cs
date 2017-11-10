using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour {

    private static CombatController instance;
    private bool placementDone = false;
    private bool combatStarted = false;
    private int tileX;
    private int tileY;
    private Button btnStartGame;

    void CreateInstance()
    {
        if (instance != null)
        {
            Debug.Log("There should never have two combat controllers.");
        }
        instance = this;
    }

    public void Start()
    {
        CreateInstance();
        btnStartGame = GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/Panel/Panel/Button_Start_Game").GetComponent<Button>();
        btnStartGame.onClick.AddListener(StartCombatMode);
    }

    /* Code de gestion du placement des personnages Pré-Combat*/

    public void ConfirmCharaPosition(int x,int y)
    {
        if (DungeonController.Instance.Dungeon.Tiles[x,y].isStarterTile == true)
        {
            tileX = x;
            tileY = y;
            placementDone = true;
        }
    }

    public void StartCombatMode()
    {
        if (placementDone == true)
        {
            combatStarted = true;
            GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).GetComponent<TileController>().MoveTo();
        }
    }

    /* Code de gestion de l'Initiative des personnages */



    /* Code de gestion du début de combat */

    public void CombatBeginning()
    {

    }

    /* Code de gestion de fin de combat */



    /* Accessors Methods */
    public static CombatController Instance
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

    public bool PlacementDone
    {
        get
        {
            return placementDone;
        }

        set
        {
            placementDone = value;
        }
    }
    public bool CombatStarted
    {
        get
        {
            return combatStarted;
        }

        set
        {
            combatStarted = value;
        }
    }
    /**/
}
