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
    private int tileX,tileY;
    private Button btnStartGame,btnCACMode,btnDistanceMode;
    private FoeController foe;
    private Room foeData;
    private UnitController targetUnit;
    private int monsterNmb;

    GameObject monster_go;
    GameObject monsterPrefab;
    GameObject UIMonsterDisplayPrefab;
    GameObject UIMonsterDisplay;
    GameObject UIPlayerDisplay;

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

        foeData = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().roomListDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonIndex].RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex];
        monsterNmb = foeData.enemies;
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

        targetUnit = GameObject.Find("Character_0").transform.Find("Unit").GetComponent<UnitController>();

        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX + 1) + "_" + (targetUnit.TileY + 1)).GetComponent<TileController>().SetRange();
        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX + 1) + "_" + (targetUnit.TileY + 0)).GetComponent<TileController>().SetRange();
        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX + 1) + "_" + (targetUnit.TileY - 1)).GetComponent<TileController>().SetRange();
        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX + 0) + "_" + (targetUnit.TileY + 1)).GetComponent<TileController>().SetRange();
        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX + 0) + "_" + (targetUnit.TileY - 1)).GetComponent<TileController>().SetRange();
        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX - 1) + "_" + (targetUnit.TileY + 1)).GetComponent<TileController>().SetRange();
        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX - 1) + "_" + (targetUnit.TileY + 0)).GetComponent<TileController>().SetRange();
        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX - 1) + "_" + (targetUnit.TileY - 1)).GetComponent<TileController>().SetRange();
    }

    public void SwitchToDistanceAttack()
    {
        Debug.Log("Distance Attack Mode has been selected");
        attackMode = true;
        // Afficher la portée sur la grille (en Rouge).
        // Distance : donc portée de 2 maximale autour de la cible (Test)

        targetUnit = GameObject.Find("Character_0").transform.Find("Unit").GetComponent<UnitController>();

        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX + 2) + "_" + (targetUnit.TileY + 0)).GetComponent<TileController>().SetRange();
        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX + 1) + "_" + (targetUnit.TileY + 1)).GetComponent<TileController>().SetRange();
        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX + 1) + "_" + (targetUnit.TileY + 0)).GetComponent<TileController>().SetRange();
        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX + 1) + "_" + (targetUnit.TileY - 1)).GetComponent<TileController>().SetRange();
        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX + 0) + "_" + (targetUnit.TileY + 2)).GetComponent<TileController>().SetRange();
        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX + 0) + "_" + (targetUnit.TileY + 1)).GetComponent<TileController>().SetRange();
        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX + 0) + "_" + (targetUnit.TileY - 1)).GetComponent<TileController>().SetRange();
        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX + 0) + "_" + (targetUnit.TileY - 2)).GetComponent<TileController>().SetRange();
        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX - 1) + "_" + (targetUnit.TileY + 1)).GetComponent<TileController>().SetRange();
        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX - 1) + "_" + (targetUnit.TileY + 0)).GetComponent<TileController>().SetRange();
        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX - 1) + "_" + (targetUnit.TileY - 1)).GetComponent<TileController>().SetRange();
        //GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.TileX - 2) + "_" + (targetUnit.TileY + 0)).GetComponent<TileController>().SetRange();
    }

    /* Code de gestion du début de combat */

    public void CombatBeginning()
    {
        SpawnMonster(); // Le combat se lance; 1 ére étape: Spawn du(des) monstre(s).
    }

    public void SpawnMonster()
    {
        monsterPrefab = Resources.Load("Prefab/Foe") as GameObject;
        UIMonsterDisplayPrefab = Resources.Load("UI_Interface/UIBattleOrderDisplay") as GameObject;

        for (int i = 0; i < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedSizeOfTheTeam; i++)
        {
            UIPlayerDisplay = Instantiate(UIMonsterDisplayPrefab);
            UIPlayerDisplay.transform.parent = GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("OrderOfBattle/OrderBattlePanel");
            UIPlayerDisplay.transform.localScale = new Vector3(1, 1, 1);
            UIPlayerDisplay.name = "UIDisplayPlayer_" + i;
            UIPlayerDisplay.transform.Find("PVOrderDisplay").GetComponent<Image>().fillAmount = ((float)GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].Health_PV  / (float)GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].Health_PV);

            UIPlayerDisplay.transform.Find("MASK/PlayerRepresentation").GetComponent<Image>().sprite = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].TempSprite;

            UIPlayerDisplay.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayName").GetComponent<Text>().text = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].Name.ToString();
            UIPlayerDisplay.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPV").GetComponent<Text>().text = "PV : " + GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].Health_PV.ToString();
            UIPlayerDisplay.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPA").GetComponent<Text>().text = "PA : " + GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].ActionPoints_PA.ToString();
            UIPlayerDisplay.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPM").GetComponent<Text>().text = "PM : " + GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].MovementPoints_PM.ToString();
        }

        for (int x = 0; x < foeData.enemies; x++)
        {
            /* Instantiate this foe */
            monster_go = Instantiate(monsterPrefab);
            monster_go.name = "Foe_" + x;
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
            /* */

            /* Instantiate the UI Display for this foe */
            UIMonsterDisplay = Instantiate(UIMonsterDisplayPrefab);
            UIMonsterDisplay.transform.parent = GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("OrderOfBattle/OrderBattlePanel");
            UIMonsterDisplay.transform.localScale = new Vector3(1, 1, 1);
            UIMonsterDisplay.name = "UIDisplayMonster_" + x;
            UIMonsterDisplay.transform.Find("PVOrderDisplay").GetComponent<Image>().fillAmount = (foe.FoeHealth / foe.FoeMaxHealth);

            UIMonsterDisplay.transform.Find("MASK/PlayerRepresentation").GetComponent<Image>().sprite = foeData.enemiesList[x].enemyIcon;

            UIMonsterDisplay.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayName").GetComponent<Text>().text = foe.FoeName.ToString();
            UIMonsterDisplay.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPV").GetComponent<Text>().text = "PV : " + foe.FoeHealth.ToString();
            UIMonsterDisplay.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPA").GetComponent<Text>().text = "PA : " + foe.FoePA.ToString();
            UIMonsterDisplay.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPM").GetComponent<Text>().text = "PM : " + foe.FoePM.ToString();
            /* */

            /* Get some random number to choose a random position in the List and place the spawn monster at this position */
            int spawnMonsterNumber = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().roomListDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonIndex].RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.MonsterSpawningPoints.Count;
            int rndNmb = Random.Range(0, spawnMonsterNumber);
            if (rndNmb == spawnMonsterNumber)
                rndNmb = rndNmb - 1;
            Vector2 tile = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().roomListDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonIndex].RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.MonsterSpawningPoints[rndNmb];
            foe.SetDefaultSpawn(GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tile.x + "_" + tile.y).transform.position);
            /* */
        }
    }

    /* Code de gestion du Combat */

    public void UpdateUI(int id)
    {
        GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("OrderOfBattle/OrderBattlePanel/UIDisplayMonster_"+id).transform.Find("PVOrderDisplay").GetComponent<Image>().fillAmount = ((float)GameObject.Find("Foe_"+id).transform.Find("Unit").GetComponent<FoeController>().FoeHealth / (float)GameObject.Find("Foe_" + id).transform.Find("Unit").GetComponent<FoeController>().FoeMaxHealth);
    }

    public void CheckBattleDeath()
    {
        if (monsterNmb <= 0)
            EndBattle();
    }

    /**/

    /* Code de gestion de fin de combat */

    public void EndBattle()
    {
        GameObject.Find("FightRoomUI(Clone)").transform.Find("ScriptManager").GetComponent<CombatGestion>().FinishedCombat();
    }

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
    public int MonsterNmb
    {
        get
        {
            return monsterNmb;
        }
        set
        {
            monsterNmb = value;
        }
    }
    /**/
}
