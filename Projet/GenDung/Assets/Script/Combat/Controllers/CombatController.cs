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
    private bool spell1 = false, spell2 = false, spell3 = false;
    private int tileX,tileY , actualSpell = 99;
    private Button btnStartGame,btnSpell1,btnSpell2,btnSpell3;
    private FoeController foe;
    private Room foeData;
    private UnitController targetUnit;
    private int monsterNmb, rndNmb;
    private List<int> monsterPos;
    private GameObject monster_go, monsterPrefab, UIMonsterDisplayPrefab, UIMonsterDisplay, UIPlayerDisplay;

    private List<GameObject> spellCanvasInstantiated = new List<GameObject>();

    private MovementRangeObject movRange;
    private List<Vector2> movRangeList = new List<Vector2>();

    private Dictionary<GameObject, int> initiativeList = new Dictionary<GameObject, int>();

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
            btnStartGame = GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/Button_Start_Game").GetComponent<Button>();
            btnStartGame.onClick.AddListener(StartCombatMode);

            btnStartGame.GetComponent<CanvasGroup>().alpha = 1;
            btnStartGame.GetComponent<CanvasGroup>().interactable = true;

            btnSpell1 = GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/Spells/Panel/Button_Spell_1").GetComponent<Button>();
            btnSpell1.onClick.AddListener(SwitchAttackModeFirst);

            btnSpell2 = GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/Spells/Panel/Button_Spell_2").GetComponent<Button>();
            btnSpell2.onClick.AddListener(SwitchAttackModeSecond);

            btnSpell3 = GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/Spells/Panel/Button_Spell_3").GetComponent<Button>();
            btnSpell3.onClick.AddListener(SwitchAttackModeThird);

            foeData = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().roomListDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonIndex].RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex];
            monsterNmb = foeData.enemies;
            monsterPos = new List<int>();

            GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().InstantiatedCombatModule = true;
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

            for (int i = 0; i < DungeonController.Instance.SpawnTilesList.Count; i++) // Update de l'UI des Tiles servant de Zones de placement pré-combat.
            {
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + DungeonController.Instance.SpawnTilesList[i].x + "_" + DungeonController.Instance.SpawnTilesList[i].y).GetComponent<TileController>().UpdateTileUI();
            }

            btnStartGame.GetComponent<CanvasGroup>().alpha = 0;
            btnStartGame.GetComponent<CanvasGroup>().interactable = false;

            GameObject.Find("CanvasUIDungeon(Clone)/Panel/Panel/ActualPlayerPanel").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("CanvasUIDungeon(Clone)/Panel/Panel/Spells").GetComponent<CanvasGroup>().alpha = 1;

            //targetUnit.Test();

            CombatBeginning(); // Le Joueur confirme son positionnement, on lance le début du Combat.
            SetMovementRangeOnGrid();
        }
    }

    public void NextEntityTurn()
    {
        btnSpell1.GetComponent<CanvasGroup>().alpha = 1;
        btnSpell1.GetComponent<Button>().interactable = true;
        btnSpell2.GetComponent<CanvasGroup>().alpha = 1;
        btnSpell2.GetComponent<Button>().interactable = true;
        btnSpell3.GetComponent<CanvasGroup>().alpha = 1;
        btnSpell3.GetComponent<Button>().interactable = true;
    }

    public void SpellUsable(float rmnPA)
    {
        for (int i = 1; i < 4; i++)
        {
            if(rmnPA < targetUnit.PlayerSpells[i-1].spellCost)
            {
                GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/Spells/Panel/Button_Spell_"+i).GetComponent<Button>().interactable = false;
                GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/Spells/Panel/Button_Spell_" + i).GetComponent<CanvasGroup>().alpha = 0.5f;
            }
        }
    }

    /* Code de gestion de l'Initiative des personnages */

    public void GatherCharacterInitiative()
    {
        // On récupére l'initiative des personnages du Joueur ainsi que celle des ennemis.
        // On stocke ces informations; Pourquoi pas dans une Liste d'objets spécifiques composés du GameObject du personnages (Joueur ou monstres) ainsi que de la valeur de son initiative.
        // Ainsi, on récupére le gameobject et on l'utilise pour le reste du code ( Voir Dictionary de Unity si réalisable).

        for (int p = 0; p < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedSizeOfTheTeam; p++) // On parcourt la liste des Personnages du Joueur.
        {
            initiativeList.Add(GameObject.Find("Character_"+p).transform.Find("Unit").gameObject, GameObject.Find("Character_" + p).transform.Find("Unit").gameObject.GetComponent<UnitController>().Initiative);
        }

        for (int m = 0; m < foeData.enemies; m++)
        {
            initiativeList.Add(GameObject.Find("Foe_" + m).transform.Find("Unit").gameObject, GameObject.Find("Foe_" + m).transform.Find("Unit").gameObject.GetComponent<FoeController>().FoeInitiative);
        }

        for (int x = 0; x < initiativeList.Count; x++)
        {
            
        }
    }

    /* Code de gestion du Mode Attaque ou Mode Déplacement */

    public void SetMovementRangeOnGrid()
    {
        targetUnit = GameObject.Find("Character_0").transform.Find("Unit").GetComponent<UnitController>();

        if(targetUnit.remainingMovement != 0)
        {
            movRange = Resources.Load<MovementRangeObject>("MovementRange/MovementRange_0" + targetUnit.remainingMovement);

            for (int m = 0; m < movRange.movementRange.Count; m++)
            {
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (movRange.movementRange[m].x + targetUnit.TileX) + "_" + (movRange.movementRange[m].y + targetUnit.TileY)).GetComponent<TileController>().SetMovementRange();
                movRangeList.Add(new Vector2((movRange.movementRange[m].x + targetUnit.TileX), (movRange.movementRange[m].y + targetUnit.TileY)));
            }
        }

    }

    public void RemoveMovementRangeOnGrid()
    {
        if (movRange != null)
        {
            for (int m = 0; m < movRange.movementRange.Count; m++)
            {
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (movRange.movementRange[m].x + targetUnit.TileX) + "_" + (movRange.movementRange[m].y + targetUnit.TileY)).GetComponent<TileController>().RemoveRange();
            }

            movRangeList = new List<Vector2>();
        }
    }

    public void SwitchAttackModeFirst()
    {
        if (!attackMode && !spell1 || attackMode && !spell1 || attackMode && spell2 || attackMode && spell3) // Active le spell 0 
        {
            Debug.Log("Attack Mode has been selected, Spell N°1");
            attackMode = true;
            spell1 = true;
            spell2 = false;
            spell3 = false;
            actualSpell = 0;
            // Afficher la portée sur la grille (en Rouge).

            targetUnit = GameObject.Find("Character_0").transform.Find("Unit").GetComponent<UnitController>(); // On récupére le personnage dont c'est le tour.
            // Lié la ligne du dessus avec le code du système d'Initiative. 

            for (int i = 0; i < targetUnit.PlayerSpells[0].range.spellRange.Count; i++)
            {
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[0].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[0].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().SetRange();
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[0].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[0].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().IsInRange = true;
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[0].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[0].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().S = 0;
            }
        }
        else if (attackMode && spell1) // Désactive le spell 0 et clean la Range. 
        {
            attackMode = false;
            spell1 = false;
            actualSpell = 99;
            targetUnit = GameObject.Find("Character_0").transform.Find("Unit").GetComponent<UnitController>(); // On récupére le personnage dont c'est le tour.
            // Lié la ligne du dessus avec le code du système d'Initiative. 

            for (int i = 0; i < targetUnit.PlayerSpells[0].range.spellRange.Count; i++)
            {
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[0].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[0].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().RemoveRange();
            }
        }
    }

    public void SwitchAttackModeSecond()
    {
        if (!attackMode && !spell2 || attackMode && !spell2 || attackMode && spell1 || attackMode && spell3) // Active le spell 1
        {
            Debug.Log("Attack Mode has been selected, Spell N°2");
            attackMode = true;
            spell1 = false;
            spell2 = true;
            spell3 = false;
            actualSpell = 1;
            // Afficher la portée sur la grille (en Rouge).

            targetUnit = GameObject.Find("Character_0").transform.Find("Unit").GetComponent<UnitController>(); // On récupére le personnage dont c'est le tour.
            // Lié la ligne du dessus avec le code du système d'Initiative. 

            for (int i = 0; i < targetUnit.PlayerSpells[1].range.spellRange.Count; i++) 
            {
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[1].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[1].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().SetRange();
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[1].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[1].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().IsInRange = true;
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[1].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[1].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().S = 1;
            }
        }
        else if (attackMode && spell2) // Désactive le spell 1 et clean la Range. 
        {
            attackMode = false;
            spell2 = false;
            actualSpell = 99;
            targetUnit = GameObject.Find("Character_0").transform.Find("Unit").GetComponent<UnitController>(); // On récupére le personnage dont c'est le tour.
            // Lié la ligne du dessus avec le code du système d'Initiative. 

            for (int i = 0; i < targetUnit.PlayerSpells[1].range.spellRange.Count; i++)
            {
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[1].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[1].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().RemoveRange();
            }
        }
    }

    public void SwitchAttackModeThird()
    {
        if (!attackMode && !spell3 || attackMode && !spell3 || attackMode && spell1 || attackMode && spell2) // Active le spell 2
        {
            Debug.Log("Attack Mode has been selected, Spell N°2");
            attackMode = true;
            spell1 = false;
            spell2 = false;
            spell3 = true;
            actualSpell = 2;
            // Afficher la portée sur la grille (en Rouge).

            targetUnit = GameObject.Find("Character_0").transform.Find("Unit").GetComponent<UnitController>(); // On récupére le personnage dont c'est le tour.
            // Lié la ligne du dessus avec le code du système d'Initiative. 

            for (int i = 0; i < targetUnit.PlayerSpells[2].range.spellRange.Count; i++)
            {
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[2].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[2].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().SetRange();
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[2].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[2].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().IsInRange = true;
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[2].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[2].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().S = 2;
            }
        }
        else if (attackMode && spell3) // Désactive le spell 2 et clean la Range. 
        {
            attackMode = false;
            spell3 = false;
            actualSpell = 99;
            targetUnit = GameObject.Find("Character_0").transform.Find("Unit").GetComponent<UnitController>(); // On récupére le personnage dont c'est le tour.
            // Lié la ligne du dessus avec le code du système d'Initiative. 

            for (int i = 0; i < targetUnit.PlayerSpells[2].range.spellRange.Count; i++)
            {
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[2].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[2].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().RemoveRange();
            }
        }
    }

    public void CleanRangeAfterAttack()
    {
        targetUnit = GameObject.Find("Character_0").transform.Find("Unit").GetComponent<UnitController>(); // On récupére le personnage dont c'est le tour.
        // si on clique sur une case en dehors de la Range, voir pour faire correspondre le S avec le bon bouton.

        for (int i = 0; i < targetUnit.PlayerSpells[actualSpell].range.spellRange.Count; i++)
        {
            GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[actualSpell].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[actualSpell].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().RemoveRange();
            StartCoroutine(WaitForAttackToEnd(i));
            GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[actualSpell].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[actualSpell].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().RemoveRange();
        }

        spell1 = false;
        spell2 = false;
        spell3 = false;
    }

    public void SetTileSpellIndicator()
    {
        for (int i = 0; i < targetUnit.PlayerSpells[actualSpell].range.spellRange.Count; i++)
        {
            GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[actualSpell].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[actualSpell].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().S = 99;
        }
        SetMovementRangeOnGrid();
        //actualSpell = 99;
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
            foe.FoeInitiative = foeData.enemiesList[x].initiative;
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
            rndNmb = Random.Range(0, spawnMonsterNumber);
            while (monsterPos.Contains(rndNmb))
            {
                rndNmb = Random.Range(0, spawnMonsterNumber);
                if (rndNmb == spawnMonsterNumber)
                    rndNmb = rndNmb - 1;
            }

            Vector2 tile = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().roomListDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonIndex].RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].room.MonsterSpawningPoints[rndNmb];
            foe.SetDefaultSpawn(GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tile.x + "_" + tile.y).transform.position);
            foe.Pos = tile;
            foe.SetTileAsOccupied();
            monsterPos.Add(rndNmb);
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
        {
            Debug.Log("All Monsters are dead, Combat will finish soon");
            StartCoroutine(WaitBeforeEndBattle());
        }
    }

    /**/

    /* Code de gestion de fin de combat */

    public void EndBattle()
    {
        // Clean Battle Display : 'UIDisplayPlayer_x' and 'UIDisplayMonster_x'
        for (int m = 0; m < foeData.enemies; m++)
        {
            if(GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("OrderOfBattle/OrderBattlePanel/UIDisplayMonster_" + m) != null)
                Destroy(GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("OrderOfBattle/OrderBattlePanel/UIDisplayMonster_" + m).gameObject);
        }

        for (int j = 0; j < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedSizeOfTheTeam; j++)
        {
            Destroy(GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("OrderOfBattle/OrderBattlePanel/UIDisplayPlayer_" + j).gameObject);
        }
        // Clean 'SpellCanvas(Clone)' from Hierarchy

        for (int s = 0; s < spellCanvasInstantiated.Count; s++)
        {
            Destroy(spellCanvasInstantiated[s]);
        }

        btnStartGame.GetComponent<CanvasGroup>().alpha = 1;
        btnStartGame.GetComponent<CanvasGroup>().interactable = true;

        GameObject.Find("FightRoomUI(Clone)").transform.Find("ScriptManager").GetComponent<CombatGestion>().FinishedCombat();
    }

    /*IEnumerator Methods*/

    public IEnumerator WaitForAttackToEnd(int i)
    {
        //Debug.Log("Launch Wait Before inRange Get back to False");
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[actualSpell].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[actualSpell].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().IsInRange = false;
    }

    public IEnumerator WaitBeforeEndBattle()
    {
        yield return new WaitForSeconds(2.5f);
        EndBattle();
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
    public UnitController TargetUnit
    {
        get
        {
            return targetUnit;
        }
        set
        {
            targetUnit = value;
        }
    }
    public int ActualSpell
    {
        get
        {
            return actualSpell;
        }
        set
        {
            actualSpell = value;
        }
    }
    public List<GameObject> SpellCanvasInstantiated
    {
        get
        {
            return spellCanvasInstantiated;
        }
        set
        {
            spellCanvasInstantiated = value;
        }
    }
    /**/
}
