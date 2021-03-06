﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreCombatController : MonoBehaviour {
//	
//    private static PreCombatController instance;
//
//    private int tileX, tileY;
//    private bool placementDone = false, combatStarted = false;
//    private GameObject monster_go, monsterPrefab;
//	private ExploMap foeData;
//    private GameData playerData;
//    private UnitController unit;
//    private int monsterNmb, rndNmb;
//    private List<int> monsterPos;
//    private FoeController foe;
//    private Dictionary<GameObject, int> initiativeList = new Dictionary<GameObject, int>();
//    private List<GameObject> sortedGameobjectInit = new List<GameObject>();
//    private int monsterAmount;
//    private Explo_FightRoom exploFight;
//    private int localIndex;
//	LogGestionTool logT;
//    
//
//    void CreateInstance()
//    {
//        if (instance != null)
//        {
//            Debug.Log("There should never have two PreCombatControllers.");
//        }
//        instance = this;
//    }
//
//    void Start ()
//    {
//        CreateInstance();
//
//        if (SceneManager.GetActiveScene().name != "Editor")
//        {
//            exploFight = GameObject.Find("ExploGridPrefab").GetComponent<Explo_FightRoom>();
//			foeData = GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons [MapController.Instance.DungeonIndex];  
//			// dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex];
//			monsterNmb = foeData.enemiesList.Count;
//            monsterPos = new List<int>();
//        }
//
//        SpawnPlayer();
//
//        logT = GameObject.Find ("DontDestroyOnLoad").GetComponent<LogGestionTool> ();
//    }
//
//    // 

//    public void ConfirmCharaPosition(int x, int y)
//    {
//        if (GridController.Instance.Grid.Tiles[x, y].isStarterTile == true)
//        {
//            tileX = x;
//            tileY = y;
//            GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).GetComponent<TileController>().MoveTo();
//            CombatUIController.Instance.SwitchStartVisual();
//        }
//        else
//            Debug.Log("Not a Starter Tile, forget about it");
//    }
//
//    public void StartCombatMode()
//    {
//        placementDone = true;
//
//        if (placementDone && !combatStarted)
//        {
//            combatStarted = true;
//            GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).GetComponent<TileController>().UpdateTileUI();
//            GridController.Instance.Unit.ResetMove();
//            GridController.Instance.Unit.ResetAction();
//
//            for (int i = 0; i < GridController.Instance.SpawnTilesList.Count; i++) // Update de l'UI des Tiles servant de Zones de placement pré-combat.
//            {
//                GridController.Instance.Grid.Tiles[Mathf.RoundToInt(GridController.Instance.SpawnTilesList[i].x), Mathf.RoundToInt(GridController.Instance.SpawnTilesList[i].y)].state = Tile.TileState.Neutral;
//                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + GridController.Instance.SpawnTilesList[i].x + "_" + GridController.Instance.SpawnTilesList[i].y).GetComponent<TileController>().UpdateTileUI();
//            }
//
//            CombatUIController.Instance.SwitchStartVisual();
//
//            GameObject.Find("CanvasUIDungeon").transform.Find("Panel/Panel/ActualPlayerPanel").GetComponent<CanvasGroup>().alpha = 1f;
//            GameObject.Find("CanvasUIDungeon").transform.Find("Panel/Panel/Spells").GetComponent<CanvasGroup>().alpha = 1f;
//
//            CombatBeginning(); // Le Joueur confirme son positionnement, on lance le début du Combat.
//            CombatController.Instance.SetMovementRangeOnGrid();
//        }
//    }
//
//    public void CombatBeginning()
//    {
//        //SpawnPlayer();
//        SpawnMonster(); // Le combat se lance; 1 ére étape: Spawn du(des) monstre(s).
//        GatherCharacterInitiative();
//        CombatUIController.Instance.OrganizeUIBattleOrder(sortedGameobjectInit);
//        CombatController.Instance.NextEntityTurn();
//    }
//
//    public void SpawnPlayer()
//    {
//        if (SceneManager.GetActiveScene().name != "Editor") // Check si la scéne est différente de l'Editeur (juste pour éviter des erreurs).
//        {
//            playerData = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData;
//
//            for (int i = 0; i < playerData.SavedSizeOfTheTeam; i++)
//            {
//                /* Charge le prefab du Joueur */
//                GameObject unit_go = Instantiate(Resources.Load("Prefab/Unit")) as GameObject;
//                /* */
//
//                unit = unit_go.transform.Find("Unit").GetComponent<UnitController>();
//
//                unit.transform.Find("Cube/Image").GetComponent<Image>().sprite = playerData.SavedCharacterList[i].TempSprite;
//
//                //setup the animator for the idle animation
//                if (GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].hasAnimations)
//                {
//                    unit.transform.Find("Cube/Image").GetComponent<Animator>().runtimeAnimatorController = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].persoAnimator;
//                }
//
//                unit_go.name = "Character_" + i;
//
//                unit.ID = i;
//                unit.Health = playerData.SavedCharacterList[i].Health_PV;
//                unit.MaxHealth = playerData.SavedCharacterList[i].Health_PV;
//                unit.PA = playerData.SavedCharacterList[i].ActionPoints_PA;
//                unit.PM = playerData.SavedCharacterList[i].MovementPoints_PM;
//                unit.PlayerSpells = playerData.SavedCharacterList[i].SpellList;
//                unit.Initiative = playerData.SavedCharacterList[i].Initiative;
//
//                /* Assure le positionnement hors écran durant la phase de placement */
//                unit.SetDefaultSpawn(new Vector3(-1000, -1000, 0));
//                unit.transform.parent.GetComponent<Canvas>().sortingOrder = 71;
//                /* */
//
//                unit.GetComponent<UnitController>().enabled = false;
//            }
//        }
//    }
//
//    public void SpawnMonster()
//    {
//        monsterPrefab = Resources.Load("Prefab/Foe") as GameObject;
//        CombatController.Instance.MonsterNmb = monsterNmb;
//        CombatUIController.Instance.CreatePlayerUIBattleOrder();
//
//        int monsterAmountMax = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().exploDungeonList.explorationDungeons[MapController.Instance.DungeonIndex].enemyMax;
//
//        monsterAmount = Random.Range(1, monsterAmountMax);
//
//        // Set the monster Amount to reflect the random obtained above.
//        CombatController.Instance.MonsterNmb = monsterAmount;
//
//        for (int x = 0; x < monsterAmount; x++)
//        {
//            int rnd = Random.Range(0, GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().exploDungeonList.explorationDungeons[MapController.Instance.DungeonIndex].enemiesList.Count);
//
//            /* Instantiate this foe */
//            monster_go = Instantiate(monsterPrefab);
//            monster_go.name = "Foe_" + x;
//            monster_go.GetComponent<Canvas>().sortingOrder = 71;
//            monster_go.transform.Find("Unit/Cube/Image").GetComponent<Animator>().runtimeAnimatorController = foeData.enemiesList[rnd].enemyAnimator;
//            foe = monster_go.transform.Find("Unit").GetComponent<FoeController>();
//            /* */
//            /* Give Foe intels for this foe */
//            foe.FoeID = x;
//            foe.FoeName = foeData.enemiesList[rnd].enemyName;
//            foe.FoeHealth = foeData.enemiesList[rnd].health;
//            foe.FoeMaxHealth = foeData.enemiesList[rnd].health;
//            foe.FoePA = foeData.enemiesList[rnd].pa;
//            foe.FoePM = foeData.enemiesList[rnd].pm;
//            foe.FoeAtk = foeData.enemiesList[rnd].atk;
//            foe.FoeInitiative = foeData.enemiesList[rnd].initiative;
//            foe.Spell = foeData.enemiesList[rnd].enemyRange;
//            /* */
//            CombatUIController.Instance.CreateMonsterUIBattleOrder(x, rnd);
//
//			//logT.AddLogLine ("M = " + x + rnd);
//
//            /* Get some random number to choose a random position in the List and place the spawn monster at this position */
//
//            //int spawnMonsterNumber = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>()dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.MonsterSpawningPoints.Count;
//            int spawnMonsterNumber = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().exploDungeonList.explorationDungeons[MapController.Instance.DungeonIndex].rooms[exploFight.RoomRand].MonsterSpawningPoints.Count; 
//            rndNmb = Random.Range(0, spawnMonsterNumber);
//            while (monsterPos.Contains(rndNmb))
//            {
//                rndNmb = Random.Range(0, spawnMonsterNumber);
//                if (rndNmb == spawnMonsterNumber)
//                    rndNmb = rndNmb - 1;
//            }
//
//            Vector2 tile = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().exploDungeonList.explorationDungeons[MapController.Instance.DungeonIndex].rooms[exploFight.RoomRand].MonsterSpawningPoints[rndNmb];
//            foe.SetDefaultSpawn(GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tile.x + "_" + tile.y).transform.position);
//            foe.TileX = Mathf.RoundToInt(tile.x);
//            foe.TileY = Mathf.RoundToInt(tile.y);
//            foe.Pos = tile;
//            foe.SetTileAsOccupied();
//            monsterPos.Add(rndNmb);
//
//            /* */
//        }
//    }
//
//    public void GatherCharacterInitiative()
//    {
//        // On récupére l'initiative des personnages du Joueur ainsi que celle des ennemis.
//        // On stocke ces informations; Pourquoi pas dans une Liste d'objets spécifiques composés du GameObject du personnages (Joueur ou monstres) ainsi que de la valeur de son initiative.
//        // Ainsi, on récupére le gameobject et on l'utilise pour le reste du code ( Voir Dictionary de Unity si réalisable).
//
//        for (int p = 0; p < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedSizeOfTheTeam; p++) // On parcourt la liste des Personnages du Joueur.
//        {
//            initiativeList.Add(GameObject.Find("Character_" + p).transform.Find("Unit").gameObject, GameObject.Find("Character_" + p).transform.Find("Unit").gameObject.GetComponent<UnitController>().Initiative);
//			print ("Added Character_" + p);
//        }
//
//		for (int m = 0; m < monsterAmount; m++)
//        {
//            initiativeList.Add(GameObject.Find("Foe_" + m).transform.Find("Unit").gameObject, GameObject.Find("Foe_" + m).transform.Find("Unit").gameObject.GetComponent<FoeController>().FoeInitiative);
//			print ("Added Foe_" + m + "with initiative of " + GameObject.Find("Foe_" + m).transform.Find("Unit").gameObject.GetComponent<FoeController>().FoeInitiative);
//        }
//
//        sortedGameobjectInit = initiativeList.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();
//
//		logT.AddLogLine ("And the battle order is : ");
//
//        for (int i = 0; i < sortedGameobjectInit.Count; i++)
//        {
//            //Debug.Log(sortedGameobjectInit[i].transform.parent.name);
//			logT.AddLogLine (i + 1 + " " + sortedGameobjectInit[i].transform.parent.name);
//        }
//    }
//
//    public void SelectionSwitch (int index)
//    {
//        switch (index)
//        {
//            case 0:
//                logT.AddLogLine("Selected Character "+ index);
//                localIndex = index;
//                GridController.Instance.Grid.Tiles[GameObject.Find("Character_" + index).transform.Find("Unit").GetComponent<UnitController>().TileX, GameObject.Find("Character_" + index).transform.Find("Unit").GetComponent<UnitController>().TileY ].Type = Tile.TileType.Floor;
//                GameObject.Find("CombatGridPrefab(Clone)").GetComponent<GridController>().SetUnit(index);
//                unit.SetDefaultSpawn(new Vector3(-1000, -1000, 0));
//                break;
//
//            case 1:
//                logT.AddLogLine("Selected Character " + index);
//                localIndex = index;
//                GridController.Instance.Grid.Tiles[GameObject.Find("Character_" + index).transform.Find("Unit").GetComponent<UnitController>().TileX, GameObject.Find("Character_" + index).transform.Find("Unit").GetComponent<UnitController>().TileY].Type = Tile.TileType.Floor;
//                GameObject.Find("CombatGridPrefab(Clone)").GetComponent<GridController>().SetUnit(index);
//                unit.SetDefaultSpawn(new Vector3(-1000, -1000, 0));
//                break;
//
//            case 2:
//                logT.AddLogLine("Selected Character " + index);
//                localIndex = index;
//                GridController.Instance.Grid.Tiles[GameObject.Find("Character_" + index).transform.Find("Unit").GetComponent<UnitController>().TileX, GameObject.Find("Character_" + index).transform.Find("Unit").GetComponent<UnitController>().TileY].Type = Tile.TileType.Floor;
//                GameObject.Find("CombatGridPrefab(Clone)").GetComponent<GridController>().SetUnit(index);
//                unit.SetDefaultSpawn(new Vector3(-1000, -1000, 0));
//                break;
//
//            case 3:
//                logT.AddLogLine("Selected Character " + index);
//                localIndex = index;
//                GridController.Instance.Grid.Tiles[GameObject.Find("Character_" + index).transform.Find("Unit").GetComponent<UnitController>().TileX, GameObject.Find("Character_" + index).transform.Find("Unit").GetComponent<UnitController>().TileY].Type = Tile.TileType.Floor;
//                GameObject.Find("CombatGridPrefab(Clone)").GetComponent<GridController>().SetUnit(index);
//                unit.SetDefaultSpawn(new Vector3(-1000, -1000, 0));
//                break;
//        }
//    }
//
//    /* Accessors Methods */
//    public static PreCombatController Instance
//    {
//        get
//        {
//            return instance;
//        }
//
//        set
//        {
//            instance = value;
//        }
//    }
//    public bool PlacementDone
//    {
//        get
//        {
//            return placementDone;
//        }
//
//        set
//        {
//            placementDone = value;
//        }
//    }
//    public bool CombatStarted
//    {
//        get
//        {
//            return combatStarted;
//        }
//
//        set
//        {
//            combatStarted = value;
//        }
//    }
//    public FoeController Foe
//    {
//        get
//        {
//            return foe;
//        }
//        set
//        {
//            foe = value;
//        }
//    }
//	public ExploMap FoeData
//    {
//        get
//        {
//            return foeData;
//        }
//        set
//        {
//            foeData = value;
//        }
//    }
//    public List<GameObject> SortedGameobjectInit
//    {
//        get
//        {
//            return sortedGameobjectInit;
//        }
//        set
//        {
//            sortedGameobjectInit = value;
//        }
//    }
//    public int MonsterAmount
//    {
//        get
//        {
//            return monsterAmount;
//        }
//        set
//        {
//            monsterAmount = value;
//        }
//    }
//    public int LocalIndex
//    {
//        get
//        {
//            return localIndex;
//        }
//        set
//        {
//            localIndex = value;
//        }
//    }
}
