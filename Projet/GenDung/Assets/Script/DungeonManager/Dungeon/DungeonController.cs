using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonController : MonoBehaviour {

    private static DungeonController instance;

    private DungeonLoader dungeonLoader;
    private MapController mapController;
    private bool roomIsLocked;  //permet de verouiller une porte

    void CreateInstance()
    {
        if (instance != null)
        {
            Debug.Log("There should never have two DungeonLoader.");
        }
        instance = this;
    }

    void Start ()
    {
        CreateInstance();
        dungeonLoader = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>();
        mapController = GameObject.Find("DontDestroyOnLoad").GetComponent<MapController>();
    }

    public void Dungeon()
    {
        dungeonLoader = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>();
        mapController = GameObject.Find("DontDestroyOnLoad").GetComponent<MapController>();

        //initialise la référence au background de la salle
        DungeonLoader.Instance.Background = GameObject.FindGameObjectWithTag("backgroundOfRoom");

        //met les infos a jour sur le coté en fonction du joueur actif
        GameObject.Find("ActualPlayerImage").GetComponent<Image>().sprite = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[dungeonLoader.actualPlayer].ICON;

        if (dungeonLoader.InstantiatedCombatModule)
        {
            if (GameObject.Find("CombatGridPrefab(Clone)").GetComponent<PreCombatController>().CombatStarted)
            {
                GameObject.Find("DisplayActualPlayerPA").GetComponent<Text>().text = "PA : " + GameObject.Find("Character_0/Unit").GetComponent<UnitController>().remainingAction.ToString();
                GameObject.Find("DisplayActualPlayerPM").GetComponent<Text>().text = "PM : " + GameObject.Find("Character_0/Unit").GetComponent<UnitController>().remainingMovement.ToString();
            }
        }
        else
        {
            GameObject.Find("DisplayActualPlayerPV").GetComponent<Text>().text = "PV : " + GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[dungeonLoader.actualPlayer].Health_PV.ToString();
            GameObject.Find("DisplayActualPlayerPA").GetComponent<Text>().text = "PA : " + GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[dungeonLoader.actualPlayer].ActionPoints_PA.ToString();
            GameObject.Find("DisplayActualPlayerPM").GetComponent<Text>().text = "PM : " + GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[dungeonLoader.actualPlayer].MovementPoints_PM.ToString();
        }

        //permet de charger la salle room
        LoadRoom ();
    }

    public void StoryLoadingTime()
    {
        Instantiate(Resources.Load("UI_Interface/CanvasStoryGetIntoDungeon"));
        GameObject.Find("EnterD_Text").GetComponent<Text>().text = dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.dungeonStory;
    }

    //load first time room function
    public void LoadRoom()
    {
        if (!dungeonLoader.loadOnce2)
        {
            dungeonLoader.loadOnce2 = true;

            DungeonLoader.Instance.LogT.AddLogLine("Initial Room Loaded");

            dungeonLoader.FadeInOutAnim();

            StoryLoadingTime();

            Instantiate(Resources.Load("UI_Interface/Room1"));

            GameObject.Find("CanvasUIDungeon(Clone)/Panel/Panel/ActualPlayerPanel").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("CanvasUIDungeon(Clone)/Panel/Panel/Spells").GetComponent<CanvasGroup>().alpha = 0;

            StartCoroutine(WaitForRoomToBeInstantiated());
        }
    }

    public void GoDeeperInTheDungeon()
    {
        if (!dungeonLoader.loadOnce3)
        {
            //si la salle n'est pas vérouillée
            if (!roomIsLocked)
            {
                dungeonLoader.loadOnce3 = true;

                DungeonLoader.Instance.LogT.AddLogLine("You have got deeper in the dungeon");

                dungeonLoader.FadeInOutAnim();

                dungeonLoader.actualIndex = dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].number;

                //reset for ui
                dungeonLoader.isUIinstantiated = false;

                //look throught all the stats and asign them to object in the scene depending on the tags
                //Change le background en fonction de la salle
                if (dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].number <= dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon.Count)
                    DungeonLoader.Instance.Background.transform.GetComponent<Image>().sprite = dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].room.back;

                //retire les anciens characters sur la carte de maniere dynamique
                for (int i = 0; i < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedSizeOfTheTeam; i++)
                {
                    GameObject.FindGameObjectWithTag("Character").SetActive(false);
                }


                //montre en fonction de l'équipe que l'on a précédemment choisi les joueurs dans la salle.
                for (int i = 0; i < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedSizeOfTheTeam; i++)
                {
                    //add new
                    DungeonLoader.Instance.CharacterUI = Instantiate(Resources.Load("UI_Interface/Character")) as GameObject;
                    DungeonLoader.Instance.CharacterUI.transform.SetParent(GameObject.Find("PlayerPositions").transform, false);
                    DungeonLoader.Instance.CharacterUI.transform.Find("CharacterBG").GetComponent<Image>().sprite = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].TempSprite;

                    //setup the animator for the idle animation
                    if (GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].hasAnimations)
                    {
                        DungeonLoader.Instance.CharacterUI.transform.Find("CharacterBG").GetComponent<Animator>().runtimeAnimatorController = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].persoAnimator;
                    }

                    DungeonLoader.Instance.CharacterUI.transform.localPosition = new Vector3(dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].room.playerPositions[i].x, dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].room.playerPositions[i].y, 0);
                }

                //charge la porte
                loadDoor();

                //permet de vérifier le type de salle
                GetRoomType();

                //change l'index pour naviger dans le donjon
                //print ("index2 " + index);
                dungeonLoader.index = dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].connectingTo;
                //print ("index3 " + index);

                //attend pour ne pas spammer le bouton de porte
                StartCoroutine(WaitLagForClicking());
            }
        }
    }

    //charge la porte suivante et ses données
    public void loadDoor()
    {
        if (!dungeonLoader.loadOnceDoor)
        {
            dungeonLoader.loadOnceDoor = true;

            //initialise the door at each time we call it
            GameObject[] tempDoor;
            tempDoor = GameObject.FindGameObjectsWithTag("door");

            //desactive les portes suivantes
            if (tempDoor != null)
            {
                for (int i = 0; i < tempDoor.Length; i++)
                {
                    tempDoor[i].SetActive(false);
                }
            }

            //assigne la porte a ses coordonnées
            DungeonLoader.Instance.Doorinstantiated = Instantiate(Resources.Load("UI_Interface/door"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            DungeonLoader.Instance.Doorinstantiated.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
            DungeonLoader.Instance.Doorinstantiated.transform.localPosition = new Vector3(dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].room.doorList[0].coordinate.x, dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].room.doorList[0].coordinate.y, 0);

            //assigne les scripts que la porte a lorsqu'on clique dessus en fonction de son emplacement dans le donjon
            if (dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].doorType.ToString() == "LastDoor")
            {
                Debug.Log("hey im last door");
                DungeonLoader.Instance.Doorinstantiated.GetComponent<Button>().onClick.AddListener(dungeonLoader.LoadSceneMap);
                DungeonLoader.Instance.Doorinstantiated.GetComponent<Button>().onClick.AddListener(mapController.UnlockNextDungeon);
            }
            else {
                DungeonLoader.Instance.Doorinstantiated.GetComponent<Button>().onClick.AddListener(GoDeeperInTheDungeon);
            }

            StartCoroutine(LoadWaitRoom());
        }
    }

    //permet de savoir le type de la room
    public void GetRoomType()
    {
        //cherche pour la salle précise et store son room type
        if (dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].number <= dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon.Count)
            dungeonLoader.roomType = dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].roomType.ToString();

        //--------CHEST---------//
        if (dungeonLoader.roomType == "chest")
        {
            roomIsLocked = true;

            DungeonLoader.Instance.LogT.AddLogLine("Chest room ! Where could it be?");

            if (!dungeonLoader.isUIinstantiated)
            {
                dungeonLoader.isUIinstantiated = true;
                Instantiate(Resources.Load("UI_Interface/ChestRoomUI"));
                GameObject.FindGameObjectWithTag("unlockRoomButton").GetComponent<Button>().onClick.AddListener(AddRewards);
                GameObject.FindGameObjectWithTag("unlockRoomButton").GetComponent<Button>().onClick.AddListener(UnlockRoom);
                GameObject.Find("GoldUIDispatcherChest").GetComponent<Text>().text = dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].chestsList[0].GoldInTheChest.ToString();
                GameObject.Find("PanelBackground").SetActive(false);
            }
        }

        //--------FIGHT---------//
        if (dungeonLoader.roomType == "fight")
        {
            roomIsLocked = true;

            DungeonLoader.Instance.LogT.AddLogLine("Fight room !");

            if (!dungeonLoader.isUIinstantiated)
            {
                dungeonLoader.isUIinstantiated = true;
                Instantiate(Resources.Load("UI_Interface/FightRoomUI"));
            }
            GameObject.FindGameObjectWithTag("unlockRoomButton").GetComponent<Button>().onClick.AddListener(UnlockRoom);

            //instantie pour chaque enemi dans la liste une icone
            for (int i = 0; i < dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].enemies; i++)
            {
                GameObject enemyUI;
                enemyUI = Instantiate(Resources.Load("UI_Interface/EnemiesPanelUI")) as GameObject;
                enemyUI.transform.SetParent(GameObject.FindGameObjectWithTag("EnemyPanel").transform, false);
                enemyUI.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].enemiesList[i].enemyIcon;
            }

        }

        //--------BOSS---------//
        if (dungeonLoader.roomType == "boss")
        {
            roomIsLocked = true;

            DungeonLoader.Instance.LogT.AddLogLine("DEBUG ! NO BOSS ROOM ALLOWED");

            if (!dungeonLoader.isUIinstantiated)
            {
                dungeonLoader.isUIinstantiated = true;
                Instantiate(Resources.Load("UI_Interface/BossRoomUI"));
            }
            GameObject.FindGameObjectWithTag("unlockRoomButton").GetComponent<Button>().onClick.AddListener(UnlockRoom);

            //instantie l'icone de boss
            GameObject bossUI;
            bossUI = Instantiate(Resources.Load("UI_Interface/BossPanelUI")) as GameObject;
            bossUI.transform.SetParent(GameObject.FindGameObjectWithTag("EnemyPanel").transform, false);
            bossUI.transform.GetChild(0).GetComponent<Image>().sprite = dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].bossList[0].bossIcon;

            //instantie pour chaque enemi dans la liste une icone
            for (int i = 0; i < dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].enemies; i++)
            {

                GameObject enemyBossUI;
                enemyBossUI = Instantiate(Resources.Load("UI_Interface/EnemiesPanelUI")) as GameObject;
                enemyBossUI.transform.SetParent(GameObject.FindGameObjectWithTag("EnemyPanel").transform, false);
                enemyBossUI.transform.GetChild(0).GetComponent<Image>().sprite = dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].enemiesList[i].enemyIcon;
            }
        }
    }

    public void UnlockRoom()
    {
        roomIsLocked = false;
        GameObject.FindGameObjectWithTag("canvasInDungeon").SetActive(false);
    }

    public void AddRewards()
    {
        //ajoute un montant d or au joueur
        this.transform.GetComponent<CurrencyGestion>().IncreaseMoney(dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index - 1].chestsList[0].GoldInTheChest);
        DungeonLoader.Instance.LogT.AddLogLine("You have gain " + dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index - 1].chestsList[0].GoldInTheChest + " gold !");
        //save all the player money
        this.transform.GetComponent<CurrencyGestion>().SaveMoney();
    }

    public IEnumerator WaitForRoomToBeInstantiated()
    {
        yield return new WaitForSeconds(0.03f);

        //reset l'index du donjon
        dungeonLoader.index = 0;
        dungeonLoader.actualIndex = dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].number;

        //attribue le background de la salle
        DungeonLoader.Instance.Background = GameObject.FindGameObjectWithTag("backgroundOfRoom");
        DungeonLoader.Instance.Background.transform.GetComponent<Image>().sprite = dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].room.back;

        //montre en fonction de l'équipe que l'on a précédemment choisi les joueurs dans la salle.
        for (int i = 0; i < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedSizeOfTheTeam; i++)
        {

            DungeonLoader.Instance.CharacterUI = Instantiate(Resources.Load("UI_Interface/Character")) as GameObject;
            DungeonLoader.Instance.CharacterUI.transform.SetParent(GameObject.Find("PlayerPositions").transform, false);
            DungeonLoader.Instance.CharacterUI.transform.Find("CharacterBG").GetComponent<Image>().sprite = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].TempSprite;

            //setup the animator for the idle animation
            if (GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].hasAnimations)
            {
                DungeonLoader.Instance.CharacterUI.transform.Find("CharacterBG").GetComponent<Animator>().runtimeAnimatorController = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].persoAnimator;
            }


            DungeonLoader.Instance.CharacterUI.transform.localPosition = new Vector3(dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].room.playerPositions[i].x, dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].room.playerPositions[i].y, 0);
        }

        //instantiate the door
        loadDoor();

        //permet de vérifier le type de salle
        GetRoomType();

        dungeonLoader.index = dungeonLoader.dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[dungeonLoader.index].connectingTo;
    }

    //attend que la salle soit bien chargée
    public IEnumerator LoadWaitRoom()
    {
        yield return new WaitForSeconds(0.03f);
        dungeonLoader.loadOnceDoor = false;
    }

    //coroutine qui attend pour ne pas spammer le bouton de porte
    public IEnumerator WaitLagForClicking()
    {
        yield return new WaitForSeconds(0.03f);
        dungeonLoader.loadOnce3 = false;
    }

    /* Accessors Method */
    public static DungeonController Instance
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
    public bool RoomIsLocked
    {
        get
        {
            return roomIsLocked; 
        }
        set
        {
            roomIsLocked = value;
        }
    }
}
