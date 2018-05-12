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
    private Dictionary<GameObject, int> UnOrderedFighterList = new Dictionary<GameObject, int>();
    public List<GameObject> FighterList = new List<GameObject>();
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
        //saveSystem = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>();
       // explo_Data = GameObject.Find("DontDestroyOnLoad").GetComponent<Explo_DataController>();
        //explo_Dungeon = GameObject.Find("ScriptBattle").GetComponent<Explo_DungeonController>();
       // map_Controller = GameObject.Find("DontDestroyOnLoad").GetComponent<MapController>();
        //effect_Controller = GameObject.Find("DontDestroyOnLoad").GetComponent<EffectController>();

        //indicator_Battle = GameObject.Find("Pastille");
        //fighter_Panel = GameObject.Find("FighterPanel");
       // next_Button = GameObject.Find("NextPanel");

    }

    public void LinkToRoom()
    {
		print ("heyheyhey");

        combatCanvas.GetComponent<Canvas>().sortingOrder = 79;// Pass the fightCanvas 

		/*for (int i = 0; i < explo_Room_Fight.FoesList.Count; i++) {
			
			GameObject enemyPanelUI;

			enemyPanelUI = Instantiate(Resources.Load("UI_Interface/EnemiesPanelUI"), GameObject.Find ("FightRoomUI/PanelBackground/PanelBackground2/FightPresentationsUI/PanelEnemies").transform) as GameObject;

            enemyPanelUI.transform.Find("IconMask/Icon").GetComponent<Image>().sprite = explo_Room_Fight.FoesList[i].EntitiesSprite;

            if (explo_Room_Fight.FoesList[i].EntitiesAnimator != null)
                enemyPanelUI.transform.Find("IconMask/Icon").GetComponent<Animator>().runtimeAnimatorController = explo_Room_Fight.FoesList[i].EntitiesAnimator;

        }*/
	}

    public void SetFightRoom()
    {

		/*for (int i = 0; i < explo_Room_Fight.FoesList.Count ; i++) {

			GameObject.Find ("EnemiesPanelUI(Clone)").SetActive (false);
		}*/

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

       // SetPlayers();
       // SetFoes();
       // SetFighterIndex();
       // SetArrow();

    }

   public void SetPlayers()
    {
        for (int i = 0; i < explo_dungeon.Dungeon.Data.Players.Count; i++)
        {
            UnOrderedFighterList.Add(GameObject.Find(playerString + i), explo_dungeon.Dungeon.Data.Players[i].Initiative);

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
        }
    }

    /*public void SetFoes()
    {
        for (int i = 0; i < explo_Room_Fight.MonstersAmount; i++)
        {
            UnOrderedFighterList.Add(GameObject.Find(enemyString+i), explo_Room_Fight.FoesList[i].Initiative);

            // Old Code From Antoine's 'BattleSystem' ==> SetupEnemies
            GameObject.Find(enemyString + i).GetComponent<LocalDataHolder>().localIndex = i;
            GameObject.Find(enemyString + i).GetComponent<LocalDataHolder>().player = false;
            GameObject.Find(enemyString + i).GetComponent<LocalDataHolder>().Foe = explo_Room_Fight.FoesList[i];
            if (GameObject.Find(enemyString + i).GetComponent<LocalDataHolder>().Foe.EntitiesAnimator)
            {
                GameObject.Find(enemyString + i).transform.Find("EnemyBackground").GetComponent<Animator>().runtimeAnimatorController = GameObject.Find(enemyString + i).GetComponent<LocalDataHolder>().Foe.EntitiesAnimator;
            }
        }
    }*/

   public void SetFighterIndex()
    {
        FighterList = UnOrderedFighterList.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();

        for (int i = 0; i < FighterList.Count; i++)
        {
            FighterList[i].GetComponent<LocalDataHolder>().fighterIndex = i;
            GameObject UiBattleDisplay;
            UiBattleDisplay = Instantiate(Resources.Load("UI_Interface/UIBattleOrderDisplay"), GameObject.Find("OrderBattlePanel").transform) as GameObject;
            FighterList[i].GetComponent<LocalDataHolder>().UiOrderObject = UiBattleDisplay;
        }

        //hide the others and make the initializer work
        for (int i = 0; i < 4; i++)
        {
            GameObject.Find(playerString + i).GetComponent<LocalDataHolder>().Initialize();
        }

        //make sure for the enemies to not show if they are not dead the fact that you can click on them
        for (int i = 0; i < FighterList.Count; i++)
        {
            if (!FighterList[i].GetComponent<LocalDataHolder>().player)
            {
                FighterList[i].transform.Find("Shadow/Pastille2").GetComponent<Image>().enabled = false;
            }
        }
    }

    void SetArrow()
    {
        indicator_Battle.GetComponent<Image>().sprite = arrow;
        Vector3 actualPosition = new Vector3(0, 0, 0);
        indicator_Battle.GetComponent<RectTransform>().SetParent(FighterList[actuallyPlaying].transform.Find("Shadow"));
        indicator_Battle.GetComponent<RectTransform>().localPosition = actualPosition;

        for (int i = 0; i < FighterList.Count; i++)
        {
            FighterList[i].GetComponent<LocalDataHolder>().UpdateUiOrderOrder(false);
        }
        FighterList[actuallyPlaying].GetComponent<LocalDataHolder>().UpdateUiOrderOrder(true);
    }

    void SetupFighterPanel()
    {
        if (FighterList[actuallyPlaying].GetComponent<LocalDataHolder>().player)
        {
            SetSpellLinks(true);
        }
    }

    void SetSpellLinks(bool onOrOff)
    {

        for (int i = 0; i < 3; i++)
        {
            if (onOrOff)
            {
                GameObject.Find("Button_Spell_" + i).GetComponent<Image>().sprite = FighterList[actuallyPlaying].GetComponent<LocalDataHolder>().characterObject.SpellList[i].spellIcon;
                GameObject.Find("Button_Spell_" + i).GetComponent<SpellPropreties>().spellObject = FighterList[actuallyPlaying].GetComponent<LocalDataHolder>().characterObject.SpellList[i];
            }

            GameObject.Find("Button_Spell_" + i).GetComponent<SpellPropreties>().StartPersoUpdate(onOrOff);
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
