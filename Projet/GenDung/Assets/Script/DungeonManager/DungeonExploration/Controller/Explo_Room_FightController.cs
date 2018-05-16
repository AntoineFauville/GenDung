using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Explo_Room_FightController : MonoBehaviour
{
    private static Explo_Room_FightController instance;
    // [Data]
	private int roomRand;
    Explo_Room_Fight explo_Room_Fight;
    Explo_DungeonController explo_dungeon;
    private RoomObject actualFightRoom;
    // [Visuals]
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
    // [LogTool]
	LogGestionTool logT;

    // [Fight]
    /* [Initiative] */
    private Dictionary<Entities, int> UnOrderedFighterList = new Dictionary<Entities, int>();
    public List<Entities> FighterList = new List<Entities>();
    public Sprite arrow;
    public int actuallyPlaying;
    /* [Players] */
    string playerString = "Player ";
    public int initialAmountOfPlayer;
    public int amountOfPlayerLeft;
    public SpellObject SelectedSpellObject;
    public bool IsItFirstFight;
    /* [Foes] */
    string enemyString = "Enemy ";
    public int amountOfEnemies = 2; // random from data for room.
    public int amountOfEnemiesLeft;
    int rndAttackEnemy;

    /* [Others] */
    public bool attackMode;
    public bool effectEnded;
    public Status PS;
    SavingSystem saveSystem;
    Explo_DataController explo_Data;
    Explo_DungeonController explo_Dungeon;
    MapController map_Controller;
    EffectController effect_Controller;
    GameObject indicator_Battle;
    GameObject fighter_Panel;
    GameObject next_Button;

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
        explo_dungeon = GameObject.Find("ScriptBattle").GetComponent<Explo_DungeonController>();

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

        /*[Others] */
        saveSystem = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>();
        explo_Data = GameObject.Find("DontDestroyOnLoad").GetComponent<Explo_DataController>();
        explo_Dungeon = GameObject.Find("ScriptBattle").GetComponent<Explo_DungeonController>();
        map_Controller = GameObject.Find("DontDestroyOnLoad").GetComponent<MapController>();
        effect_Controller = GameObject.Find("DontDestroyOnLoad").GetComponent<EffectController>();

        indicator_Battle = GameObject.Find("Pastille");
        fighter_Panel = GameObject.Find("FighterPanel");
        next_Button = GameObject.Find("NextPanel");

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

        // In a First Time, we will setup the Combat: Players, Foes, Initiative , UI. In a Second Time, another Controller will deal with the Combat itself
        SetPlayers();
        SetFoes();
        SetFighterIndex();
        InitializeArrow();
        InitializeSpellPanel();

        GameObject.Find("BattleSystem/ScriptBattle").GetComponent<Explo_FightController>().FightCtrl = this;
    }

   public void SetPlayers()
    {
        for (int i = 0; i < explo_dungeon.Dungeon.Data.Players.Count; i++) // Using Data from Creation for number of Players.
        {
            UnOrderedFighterList.Add(explo_Dungeon.Dungeon.Data.Players[i], explo_dungeon.Dungeon.Data.Players[i].Initiative);

            // Old Code From Antoine's 'BattleSystem' ==> SetupPlayers
            GameObject.Find(playerString + i).GetComponent<LocalDataHolder>().characterObject = saveSystem.gameData.SavedCharacterList[i];
            GameObject.Find(playerString + i).GetComponent<LocalDataHolder>().player = true;
            GameObject.Find(playerString + i).GetComponent<LocalDataHolder>().localIndex = i;

            //if he died well update him.
            GameObject.Find(playerString + i).GetComponent<LocalDataHolder>().dead = GameObject.Find("DontDestroyOnLoad").GetComponent<Explo_DataController>().dungeonData.TempFighterObject[i].died;
            GameObject.Find(playerString + i).GetComponent<LocalDataHolder>().dead = explo_Data.dungeonData.TempFighterObject[i].died;

            if (GameObject.Find(playerString + i).GetComponent<LocalDataHolder>().characterObject.hasAnimations)
            {
                GameObject.Find(playerString + i).transform.Find("PersoBackground").GetComponent<Animator>().runtimeAnimatorController = GameObject.Find(playerString + i).GetComponent<LocalDataHolder>().characterObject.persoAnimator;
            }
            explo_dungeon.Dungeon.Data.Players[i].CreateUI();

            // Set UI Order Panel with Player Data
            explo_Dungeon.Dungeon.Data.Players[i].EntitiesUIOrder.transform.Find("MASK/PlayerRepresentation").GetComponent<Image>().sprite = explo_Dungeon.Dungeon.Data.Players[i].EntitiesSprite;
            explo_Dungeon.Dungeon.Data.Players[i].EntitiesUIOrder.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayName").GetComponent<Text>().text = explo_Dungeon.Dungeon.Data.Players[i].Name.ToString();
            explo_Dungeon.Dungeon.Data.Players[i].EntitiesUIOrder.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPV").GetComponent<Text>().text = "HP = " + explo_Dungeon.Dungeon.Data.Players[i].Health.ToString() + " / " + explo_Dungeon.Dungeon.Data.Players[i].MaxHealth.ToString();
            explo_Dungeon.Dungeon.Data.Players[i].EntitiesUIOrder.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPA").GetComponent<Text>().text = "AP = " + explo_Dungeon.Dungeon.Data.Players[i].ActionPoint.ToString() + " / " + explo_Dungeon.Dungeon.Data.Players[i].MaxActionPoint.ToString();
        }
    }

    public void SetFoes()
    {
        for (int i = 0; i < explo_Room_Fight.MonstersAmount; i++)
        {
            // Instantiate Foe for This Fight and rename it to follow the naming convention of Antoine.
            GameObject instantiatedFoe = Instantiate(Resources.Load("Prefab/Explo_Foe") as GameObject, GameObject.Find("BattleSystem/BattleSystem/EnemyPanelPlacement").transform);
            instantiatedFoe.name = enemyString + i;

            // Set EntitiesGO to the Actual Foe Model and Initialize is Visuals.
            explo_Room_Fight.FoesList[i].EntitiesGO = instantiatedFoe;
            explo_Room_Fight.FoesList[i].InitializeVisual();

            // Add the actual Foe to the UnOrdered List of fighter for Initiative .
            UnOrderedFighterList.Add(explo_Room_Fight.FoesList[i], explo_Room_Fight.FoesList[i].Initiative);

            // Old Code From Antoine's 'BattleSystem' ==> SetupEnemies
            GameObject.Find(enemyString + i).GetComponent<LocalDataHolder>().localIndex = i;
            GameObject.Find(enemyString + i).GetComponent<LocalDataHolder>().player = false;
            GameObject.Find(enemyString + i).GetComponent<LocalDataHolder>().Foe = explo_Room_Fight.FoesList[i];
            if (GameObject.Find(enemyString + i).GetComponent<LocalDataHolder>().Foe.EntitiesAnimator)
            {
                GameObject.Find(enemyString + i).transform.Find("Background").GetComponent<Animator>().runtimeAnimatorController = GameObject.Find(enemyString + i).GetComponent<LocalDataHolder>().Foe.EntitiesAnimator;
            }

            explo_Room_Fight.FoesList[i].CreateUI();

            // Set UI Order Panel with Foe Data
            explo_Room_Fight.FoesList[i].EntitiesUIOrder.transform.Find("MASK/PlayerRepresentation").GetComponent<Image>().sprite = explo_Room_Fight.FoesList[i].EntitiesSprite;
            explo_Room_Fight.FoesList[i].EntitiesUIOrder.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayName").GetComponent<Text>().text = explo_Room_Fight.FoesList[i].Name.ToString();
            explo_Room_Fight.FoesList[i].EntitiesUIOrder.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPV").GetComponent<Text>().text = "HP = " + explo_Room_Fight.FoesList[i].MaxHealth.ToString() + " / " + explo_Room_Fight.FoesList[i].MaxHealth.ToString();
            explo_Room_Fight.FoesList[i].EntitiesUIOrder.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPA").GetComponent<Text>().enabled = false;
        }
    }

   public void SetFighterIndex()
    {
        // Use Linq to order the FighterList with Initiative value from each Entities.
        FighterList = UnOrderedFighterList.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();
        // For Statement to set The UI Order in the Real Order of Fighter.
        for (int i = 0; i < FighterList.Count; i++)
        {
            FighterList[i].EntitiesUIOrder.transform.SetSiblingIndex(i);
        }
    }

    public void InitializeArrow()
    {
        Vector3 actualPosition = new Vector3(0, 0, 0);
        indicator_Battle.GetComponent<RectTransform>().SetParent(FighterList[0].EntitiesGO.transform.Find("Shadow"));
        indicator_Battle.GetComponent<RectTransform>().localPosition = actualPosition;

        for (int i = 0; i < FighterList.Count; i++)
        {
            FighterList[i].EntitiesUIOrder.transform.Find("BouleVerte").GetComponent<Image>().enabled = false;
            FighterList[i].EntitiesUIOrder.transform.Find("Scripts").GetComponent<UIOrderBattle>().Selected(false);
        }
        FighterList[0].EntitiesUIOrder.transform.Find("BouleVerte").GetComponent<Image>().enabled = true;
        FighterList[0].EntitiesUIOrder.transform.Find("Scripts").GetComponent<UIOrderBattle>().Selected(true);
    }

    public void InitializeSpellPanel()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject.Find("Button_Spell_" + i).GetComponent<Image>().sprite = FighterList[0].EntitiesSpells[i].spellIcon;
            GameObject.Find("Button_Spell_" + i).GetComponent<SpellPropreties>().spellObject = FighterList[0].EntitiesSpells[i];
        }
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
