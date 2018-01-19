using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class DungeonLoader : MonoBehaviour {

    private static DungeonLoader instance;

    private DungeonController dungeonController;
    private MapController mapController;
    private TavernController tavernController;

	public string 
	activeScene, //check active scene
	previousScene, //previous scene
	roomType; // just a checker to see what room is the actual room that we are using.

	Animator
	FadeInAnimator;

    GameObject[]
    FadeInOBjTemp;

	GameObject 
    a,
	background, //background de la salle
	doorinstantiated, //la porte instantiée
	characterUI; //instaniated character

	LogGestionTool
	logT;

	public RoomList[] 
	roomListDungeon; // this are the dungeons, 

    public int
    index, //index de la salle connecté à actualIndex.
    actualIndex; // index de la salle actuelle.
	

    //all int for upgrade temp
    public int
    healthTemp,
    ActionTemp,
    CACTemp,
    DistTemp;

    public int
    dungeonUnlockedIndex = 1,	//index pour le donjon unlocked doit etre 1 sinon 0 bonjons ne s'afficheront
    actualPlayer = 0;

    public bool
    loadOnce3, //lié au godeeperintodungeon vu que c'est un bouton ca a besoin de verifier que ca ne se fait qu'une fois
    loadOnce2,  //lié au loadRoom vu que c'est un bouton ca a besoin de verifier que ca ne se fait qu'une fois
    loadbutton, //pour ne charger qu'une fois la scene donjon
    loadbutton2,    //pour ne charger qu'une fois la scene map
    loadOnceDoor,   //pour ne charger qu'une fois la porte
    roomIsLocked,   //permet de verouiller une porte
    isUIinstantiated,   //verifier dans le donjon si l'interface a bien été instanciée
    doOnceCoroutine,    //lance la coroutine qu'une fois
    sceneLoaded,    //attendre que la scene est bien chargé
    InstrantiateOnceEndDungeon, //instancier une fois l'écran de fin de donjon
    EndDungeon,	//verifier si le donjon est fini ou pas
    InstantiateFade,
    InstantiatedCombatModule,
    QuestStartOn;

    private bool doOnceAllRelatedToUpgradeTavernPanel;

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

        if(activeScene == "MainMenu")
            Instantiate(Resources.Load("UI_Interface/CanvasMainMenu")); // Instantiate Canvas when we click on Button.
        
        dungeonController = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonController>(); // Séparation Gestion Dungeon du DungeonLoader
        mapController = GameObject.Find("DontDestroyOnLoad").GetComponent<MapController>(); // Séparation Gestion Map du DungeonLoader
        tavernController = GameObject.Find("DontDestroyOnLoad").GetComponent<TavernController>(); // Séparation Gestion Taverne du DungeonLoader

		//permet de vérifier ce qu'est la scene actuelle et d'attendre qu'elle aie fini de charger
		SceneManager.sceneLoaded += OnSceneLoaded;
        FadeInOutAnim();

		LogT = GameObject.Find ("DontDestroyOnLoad").GetComponent<LogGestionTool> ();

    }

	//permet de vérifier ce qu'est la scene actuelle et d'attendre qu'elle aie fini de charger
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
		sceneLoaded = true;
	}

	void FixedUpdate ()
    {
		//permet de savoir quel nom de scene
		activeScene = SceneManager.GetActiveScene ().name;

        //attendre que la scene soit chargée
        if (sceneLoaded)
        {
            //-----------Main Menu gestion scene-------------//
            if (activeScene == "MainMenu")
            { 
                if (true)
                if (GameObject.Find("CanvasMainMenu(Clone)") == null)
                {
                    Instantiate(Resources.Load("UI_Interface/CanvasMainMenu")); // Instantiate Canvas when we click on Button.
                    sceneLoaded = false;
                }
            }
            //-----------Dungeon gestion scene-------------//
            if (activeScene == "Dungeon" && GameObject.Find("CanvasUIDungeon(Clone)") == null)
            {
                Instantiate(Resources.Load("UI_Interface/CanvasUIDungeon")); // Instantiate Canvas when we click on Button.
                sceneLoaded = false;

                dungeonController.Dungeon();
			}		

			//-----------Map gestion scene-------------//
			if (activeScene == "Map" && GameObject.Find("CanvasCarte(Clone)") == null)
            {
                Instantiate(Resources.Load("UI_Interface/CanvasCarte")); // Instantiate Canvas when we click on Button.
                sceneLoaded = false;

                mapController.Map();
            }

            //--------Taverne--------//
            if (activeScene == "Tavern" && GameObject.Find("CanvasTavern(Clone)") == null)
            {
                Instantiate(Resources.Load("UI_Interface/CanvasTavern")); // Instantiate Canvas when we click on Button.
                sceneLoaded = false;

                tavernController.Tavern();
            }
        }
        else
        {
			//systeme de vérification pour voir si la scene a bien charger
			//au sinon lance une coroutine qui attend peu et reinitialise les données
			if (!doOnceCoroutine)
            {
				doOnceCoroutine = true;
				StartCoroutine ("WaitLoading");
			}
		}
	}

	//charge la scene map
	public void LoadSceneMap ()
    {
		if (!loadbutton2) {
			loadbutton2 = true;

			FadeInOutAnim();

			SceneManager.LoadScene ("Map");
			EndDungeon = true;
		}
	}

	public void FadeInOutAnim(){
        if (!InstantiateFade)
        {
            InstantiateFade = true;

            if (a == null) {
                a = Instantiate(Resources.Load("UI_Interface/CanvasFadeInOut")) as GameObject;

            }

            FadeInOBjTemp = GameObject.FindGameObjectsWithTag("FadeInFadeOut");
            for (int i = 0; i < FadeInOBjTemp.Length; i++)
            {
                FadeInOBjTemp[i].SetActive(false);
            }

            Instantiate(Resources.Load("UI_Interface/CanvasFadeInOut"));

            FadeInAnimator = GameObject.FindGameObjectWithTag("FadeInFadeOut").transform.GetChild(0).GetComponent<Animator>();

            FadeInAnimator.SetBool("Fade", true);
            StartCoroutine("FadeInOutCoroutine");
        }
	}

	//----------------------------IENUMERATOR---------------------------------//

	//attend que la scene soit bien chargée
	public IEnumerator WaitLoading(){
		yield return new WaitForSeconds (0.03f);

		//reinitialise la scene pour charger a nouveau lors du prochain donjon le LOADROOM
		if (activeScene == "Dungeon")
        {
			previousScene = "";
		}

		if (activeScene == "Map")
        {
			

			//reinitialise la scene pour charger a nouveau lors du prochain donjon le LOADROOM
			previousScene = "";

			//réinitialise les données
			loadOnce3 = false;
			loadOnce2 = false;
			loadbutton = false;
			isUIinstantiated = false;
			loadbutton2 = false;

            //dungeonOnTheMap = GameObject.FindGameObjectWithTag("CanvasCarte(Clone)/Panel/Panel/PanelScriptDungeonList").GetComponent<DungeonListOnMap>().dungeonOnTheMapList;

        }

		//relance la recherche des données dans le fixedUpdate
		sceneLoaded = false;
		doOnceCoroutine = false;
	}

	public IEnumerator FadeInOutCoroutine()
    {
		yield return new WaitForSeconds (0.4f);
		FadeInAnimator.SetBool ("Fade",false);
        InstantiateFade = false;
    }

    /* Accessors Method */
    public static DungeonLoader Instance
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
    public bool DoOnceAllRelatedToUpgradeTavernPanel
    {
        get
        {
            return doOnceAllRelatedToUpgradeTavernPanel;
        }
        set
        {
            doOnceAllRelatedToUpgradeTavernPanel = value;
        }
    }
    public GameObject CharacterUI
    {
        get
        {
            return characterUI;
        }
        set
        {
            characterUI = value;
        }
    }
    public GameObject Background
    {
        get
        {
            return background;
        }
        set
        {
            background = value;
        }
    }
    public LogGestionTool LogT
    {
        get
        {
            return logT;
        }
        set
        {
            logT = value;
        }
    }
    public GameObject Doorinstantiated
    {
        get
        {
            return doorinstantiated;
        }
        set
        {
            doorinstantiated = value;
        }
    }
}
