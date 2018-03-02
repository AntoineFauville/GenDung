using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreCombatController : MonoBehaviour {

    private static PreCombatController instance;

    private int tileX, tileY;
    private bool placementDone = false, combatStarted = false;
    private GameObject monster_go, monsterPrefab;
	private ExploMap foeData;
    private int monsterNmb, rndNmb;
    private List<int> monsterPos;
    private FoeController foe;
    private Dictionary<GameObject, int> initiativeList = new Dictionary<GameObject, int>();
    private List<GameObject> sortedGameobjectInit = new List<GameObject>();

    void CreateInstance()
    {
        if (instance != null)
        {
            Debug.Log("There should never have two PreCombatControllers.");
        }
        instance = this;
    }

    void Start ()
    {
        CreateInstance();

        if (SceneManager.GetActiveScene().name != "Editor")
        {
			foeData = GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().exploDungeonList.explorationDungeons [MapController.Instance.DungeonIndex];  
			// dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex];
			monsterNmb = foeData.enemiesList.Count;
            monsterPos = new List<int>();
        }
    }

    public void ConfirmCharaPosition(int x, int y)
    {
        if (GridController.Instance.Grid.Tiles[x, y].isStarterTile == true)
        {
            tileX = x;
            tileY = y;
            GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).GetComponent<TileController>().MoveTo();
            CombatUIController.Instance.SwitchStartVisual();
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
            GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).GetComponent<TileController>().UpdateTileUI();
            GridController.Instance.Unit.ResetMove();
            GridController.Instance.Unit.ResetAction();

            for (int i = 0; i < GridController.Instance.SpawnTilesList.Count; i++) // Update de l'UI des Tiles servant de Zones de placement pré-combat.
            {
                GridController.Instance.Grid.Tiles[Mathf.RoundToInt(GridController.Instance.SpawnTilesList[i].x), Mathf.RoundToInt(GridController.Instance.SpawnTilesList[i].y)].state = Tile.TileState.Neutral;
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + GridController.Instance.SpawnTilesList[i].x + "_" + GridController.Instance.SpawnTilesList[i].y).GetComponent<TileController>().UpdateTileUI();
            }

            CombatUIController.Instance.SwitchStartVisual();

            GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/ActualPlayerPanel").GetComponent<CanvasGroup>().alpha = 1f;
            GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/Spells").GetComponent<CanvasGroup>().alpha = 1f;

            CombatBeginning(); // Le Joueur confirme son positionnement, on lance le début du Combat.
            CombatController.Instance.SetMovementRangeOnGrid();
        }
    }

    public void CombatBeginning()
    {
        SpawnMonster(); // Le combat se lance; 1 ére étape: Spawn du(des) monstre(s).
        GatherCharacterInitiative();
        CombatUIController.Instance.OrganizeUIBattleOrder(sortedGameobjectInit);
        CombatController.Instance.NextEntityTurn();
    }

    public void SpawnMonster()
    {
        monsterPrefab = Resources.Load("Prefab/Foe") as GameObject;
        CombatController.Instance.MonsterNmb = monsterNmb;
        CombatUIController.Instance.CreatePlayerUIBattleOrder();



		for (int x = 0; x < foeData.enemiesList.Count; x++)
        {
            /* Instantiate this foe */
            monster_go = Instantiate(monsterPrefab);
            monster_go.name = "Foe_" + x;
            monster_go.transform.Find("Unit/Cube/Image").GetComponent<Animator>().runtimeAnimatorController = foeData.enemiesList[x].enemyAnimator;
            foe = monster_go.transform.Find("Unit").GetComponent<FoeController>();
            /* */
            /* Give Foe intels for this foe */
            foe.FoeID = x;
            foe.FoeName = foeData.enemiesList[x].enemyName;
            foe.FoeHealth = foeData.enemiesList[x].health;
            foe.FoeMaxHealth = foeData.enemiesList[x].health;
            foe.FoePA = foeData.enemiesList[x].pa;
            foe.FoePM = foeData.enemiesList[x].pm;
            foe.FoeAtk = foeData.enemiesList[x].atk;
            foe.FoeInitiative = foeData.enemiesList[x].initiative;
            foe.Spell = foeData.enemiesList[x].enemyRange;
            /* */
            CombatUIController.Instance.CreateMonsterUIBattleOrder(x);

            /* Get some random number to choose a random position in the List and place the spawn monster at this position */

			/*
            int spawnMonsterNumber = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>()dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.MonsterSpawningPoints.Count;
            rndNmb = Random.Range(0, spawnMonsterNumber);
            while (monsterPos.Contains(rndNmb))
            {
                rndNmb = Random.Range(0, spawnMonsterNumber);
                if (rndNmb == spawnMonsterNumber)
                    rndNmb = rndNmb - 1;
            }
			*/

			/*
            Vector2 tile = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.MonsterSpawningPoints[rndNmb];
            foe.SetDefaultSpawn(GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tile.x + "_" + tile.y).transform.position);
            foe.TileX = Mathf.RoundToInt(tile.x);
            foe.TileY = Mathf.RoundToInt(tile.y);
            foe.Pos = tile;
            foe.SetTileAsOccupied();
            monsterPos.Add(rndNmb);

			*/
            /* */
        }
    }

    public void GatherCharacterInitiative()
    {
        // On récupére l'initiative des personnages du Joueur ainsi que celle des ennemis.
        // On stocke ces informations; Pourquoi pas dans une Liste d'objets spécifiques composés du GameObject du personnages (Joueur ou monstres) ainsi que de la valeur de son initiative.
        // Ainsi, on récupére le gameobject et on l'utilise pour le reste du code ( Voir Dictionary de Unity si réalisable).

        for (int p = 0; p < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedSizeOfTheTeam; p++) // On parcourt la liste des Personnages du Joueur.
        {
            initiativeList.Add(GameObject.Find("Character_" + p).transform.Find("Unit").gameObject, GameObject.Find("Character_" + p).transform.Find("Unit").gameObject.GetComponent<UnitController>().Initiative);
        }

		for (int m = 0; m < foeData.enemiesList.Count; m++)
        {
            initiativeList.Add(GameObject.Find("Foe_" + m).transform.Find("Unit").gameObject, GameObject.Find("Foe_" + m).transform.Find("Unit").gameObject.GetComponent<FoeController>().FoeInitiative);
        }

        sortedGameobjectInit = initiativeList.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();

        /*for (int i = 0; i < sortedGameobjectInit.Count; i++)
        {
            Debug.Log(sortedGameobjectInit[i].transform.parent.name);
        }
        */
    }

    /* Accessors Methods */
    public static PreCombatController Instance
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
    public FoeController Foe
    {
        get
        {
            return foe;
        }
        set
        {
            foe = value;
        }
    }
	public ExploMap FoeData
    {
        get
        {
            return foeData;
        }
        set
        {
            foeData = value;
        }
    }
    public List<GameObject> SortedGameobjectInit
    {
        get
        {
            return sortedGameobjectInit;
        }
        set
        {
            sortedGameobjectInit = value;
        }
    }
}
