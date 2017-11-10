﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        if (SceneManager.GetActiveScene().name != "Editor")
        {
            btnStartGame = GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/Panel/Panel/Button_Start_Game").GetComponent<Button>();
            btnStartGame.onClick.AddListener(StartCombatMode);
        }
    }

    /* Code de gestion du placement des personnages Pré-Combat*/

    public void ConfirmCharaPosition(int x,int y)
    {
        if (DungeonController.Instance.Dungeon.Tiles[x, y].isStarterTile == true)
        {
            tileX = x;
            tileY = y;
            GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).GetComponent<TileController>().MoveTo();
        }
        else
            Debug.Log("Not a Starter Tile, forget about it");
    }

    public void StartCombatMode()
    {
        placementDone = true;

        if (placementDone && !combatStarted)
        {
            combatStarted = true;
            GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).GetComponent<TileController>().TileExit();
            DungeonController.Instance.Unit.ResetMove();
            DungeonController.Instance.Unit.ResetAction();
            CombatBeginning(); // Le Joueur confirme son positionnement, on lance le début du Combat.
        }
    }

    /* Code de gestion de l'Initiative des personnages */



    /* Code de gestion du début de combat */

    public void CombatBeginning()
    {
        SpawnMonster(); // Le combat se lance; 1 ére étape: Spawn du(des) monstre(s).
    }

    public void SpawnMonster()
    {
        GameObject monster_go = Instantiate(Resources.Load("Prefab/Foe")) as GameObject;
        int nmbMonster = 0;
        monster_go.name = "Foe_" + nmbMonster;

        int spawnMonsterNumber = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().roomListDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonIndex].RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.MonsterSpawningPoints.Count;
        int rndNmb = Random.Range(0, spawnMonsterNumber);

        if (rndNmb == spawnMonsterNumber)
            rndNmb = rndNmb-1;

        StartCoroutine(Wait());

        Vector2 tile = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().roomListDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonIndex].RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.MonsterSpawningPoints[rndNmb];
        monster_go.transform.position = GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tile.x + "_" + tile.y).transform.position;
    }

    /* Code de gestion de fin de combat */

    /*IEnumerator Methods*/

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.3f);
    }

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
