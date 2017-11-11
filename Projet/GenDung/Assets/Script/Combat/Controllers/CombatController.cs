using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CombatController : MonoBehaviour {

    private static CombatController instance;
    private bool placementDone = false;
    private bool combatStarted = false;
    private bool attackMode = false;
    private int tileX;
    private int tileY;
    private Button btnStartGame;
    private Button btnCACMode;
    private Button btnDistanceMode;
    private FoeController foe;

    GameObject monster_go;
    GameObject monsterPrefab;

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

            btnCACMode = GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/Spells/Panel/Button_CAC").GetComponent<Button>();
            btnCACMode.onClick.AddListener(SwitchToCACAttack);

            btnDistanceMode = GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/Spells/Panel/Button_Distance").GetComponent<Button>();
            btnDistanceMode.onClick.AddListener(SwitchToDistanceAttack);
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



    /* Code de gestion du Mode Attaque ou Mode Déplacement */

    public void SwitchToCACAttack()
    {
        Debug.Log("CAC Attack Mode has been selected");
        attackMode = true;
        // Afficher la portée sur la grille (en Rouge).
        // CAC : donc portée de 1 autour de la cible (par facilité)
    }

    public void SwitchToDistanceAttack()
    {
        Debug.Log("Distance Attack Mode has been selected");
        attackMode = true;
        // Afficher la portée sur la grille (en Rouge).
        // Distance : donc portée de 2 maximale autour de la cible (Test)
    }

    /* Code de gestion du début de combat */

    public void CombatBeginning()
    {
        SpawnMonster(); // Le combat se lance; 1 ére étape: Spawn du(des) monstre(s).
    }

    public void SpawnMonster()
    {
        monsterPrefab = Resources.Load("Prefab/Foe") as GameObject;

        for (int x = 0; x < GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().roomListDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonIndex].RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].enemies; x++)
        {
            monster_go = Instantiate(monsterPrefab);
            monster_go.name = "Foe_" + x;
            foe = monster_go.transform.Find("Unit").GetComponent<FoeController>();

            int spawnMonsterNumber = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().roomListDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonIndex].RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.MonsterSpawningPoints.Count;
            int rndNmb = Random.Range(0, spawnMonsterNumber);
            if (rndNmb == spawnMonsterNumber)
                rndNmb = rndNmb - 1;

            Vector2 tile = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().roomListDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonIndex].RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.MonsterSpawningPoints[rndNmb];
            foe.SetDefaultSpawn(GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tile.x + "_" + tile.y).transform.position);
        }
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
    public bool AttackMode
    {
        get
        {
            return attackMode;
        }

        set
        {
            attackMode = value;
        }
    }
    /**/
}
