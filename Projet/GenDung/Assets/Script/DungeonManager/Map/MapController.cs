using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour {

    private static MapController instance;

    private DungeonLoader dungeonLoader;

    public int dungeonIndex = 1;//index pour le donjon.
    private GameObject[] dungeonOnTheMap;//list des boutons des donjons sur la carte
    private bool loadbutton; //pour ne charger qu'une fois la scene donjon

    void CreateInstance()
    {
        if (instance != null)
        {
            Debug.Log("There should never have two DungeonLoader.");
        }
        instance = this;
    }


    public void Start()
    {
        CreateInstance();
        dungeonLoader = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>();

    }

    public void Map()
    {
        dungeonLoader = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>();

        //reset la taverne
        DungeonLoader.Instance.DoOnceAllRelatedToUpgradeTavernPanel = false;

        //show on the map current money
        GameObject.Find("GoldUIDispatcherText").GetComponent<Text>().text = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.PlayerMoney.ToString();

        #region //----------ecran de fin de donjon-----------//

        //----------ecran de fin de donjon-----------//
        if (dungeonLoader.EndDungeon && !dungeonLoader.InstrantiateOnceEndDungeon)
        {
            //instantie l'écran de fin
            dungeonLoader.InstrantiateOnceEndDungeon = true;
            Instantiate(Resources.Load("UI_Interface/EndDungeonUI"));

            //montre le montant de gold que le donjon a donné
            //GameObject.Find("GoldDispatchEndUI").GetComponent<Text>().text = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonList.myDungeons[dungeonIndex].dungeon.dungeonGold.ToString();

            //verifie dans toutes les salles
			/*
            for (int i = 0; i < dungeonLoader.dungeonList.myDungeons[dungeonIndex].dungeon.RoomOfTheDungeon.Count; i++)
            {

                //si la salle est de type fight
                if (dungeonLoader.dungeonList.myDungeons[dungeonIndex].dungeon.RoomOfTheDungeon[i].roomType.ToString() == "fight")
                {

                    //prendre ses enfants et instantier une icone pour chaque + definir leur parent dans l'écran de fin
                    for (int l = 0; l < dungeonLoader.dungeonList.myDungeons[dungeonIndex].dungeon.RoomOfTheDungeon[i].enemies; l++)
                    {

                        GameObject enemyUI;
                        enemyUI = Instantiate(Resources.Load("UI_Interface/EnemiesPanelUI")) as GameObject;
                        enemyUI.transform.SetParent(GameObject.FindGameObjectWithTag("EnemyPanel").transform, false);
                        enemyUI.transform.GetChild(0).GetComponent<Image>().sprite = dungeonLoader.dungeonList.myDungeons[dungeonIndex].dungeon.RoomOfTheDungeon[i].enemiesList[l].enemyIcon; // ==> Enemy HERE 
                    }
                }

                //si la salle est de type boss
                if (dungeonLoader.dungeonList.myDungeons[dungeonIndex].dungeon.RoomOfTheDungeon[i].roomType.ToString() == "boss")
                {

                    //prendre ses enfants et instantier une icone pour chaque + definir leur parent dans l'écran de fin
                    for (int l = 0; l < dungeonLoader.dungeonList.myDungeons[dungeonIndex].dungeon.RoomOfTheDungeon[i].enemies; l++)
                    {

                        GameObject enemyUI;
                        enemyUI = Instantiate(Resources.Load("UI_Interface/EnemiesPanelUI")) as GameObject;
                        enemyUI.transform.SetParent(GameObject.FindGameObjectWithTag("EnemyPanel").transform, false);
                        enemyUI.transform.Find("Icon").GetComponent<Image>().sprite = dungeonLoader.dungeonList.myDungeons[dungeonIndex].dungeon.RoomOfTheDungeon[i].enemiesList[l].enemyIcon;
                    }

                    //vu que c'est un type boss il y a aussi le boss a instancier
                    GameObject bossUI;
                    bossUI = Instantiate(Resources.Load("UI_Interface/BossPanelUI")) as GameObject;
                    bossUI.transform.SetParent(GameObject.FindGameObjectWithTag("EnemyPanel").transform, false);
                    bossUI.transform.Find("Icon").GetComponent<Image>().sprite = dungeonLoader.dungeonList.myDungeons[dungeonIndex].dungeon.RoomOfTheDungeon[i].bossList[0].bossIcon;
                }
            }
            */

            //ajoute un montant d or au joueur
            //this.transform.GetComponent<CurrencyGestion>().IncreaseMoney( dungeonLoader.dungeonList.myDungeons[dungeonIndex].dungeon.dungeonGold);

            //save all the player money
            this.transform.GetComponent<CurrencyGestion>().SaveMoney();
        }

        #endregion

        dungeonOnTheMap = GameObject.Find("CanvasCarte(Clone)/Panel/Panel/PanelScriptDungeonList").GetComponent<DungeonListOnMap>().dungeonOnTheMapList; // Ajouter par mes soins ^^

        //assure que les salles sont bien unlock
       // DungeonController.Instance.RoomIsLocked = false;


        //----------Dungeon Unlocking Feature ------------//
        if (dungeonLoader.dungeonUnlockedIndex <= dungeonOnTheMap.Length)
        {
            for (int i = (dungeonOnTheMap.Length -1); i > (dungeonLoader.dungeonUnlockedIndex -1); i--)
            {
                //Met faux tous les donjons non débloqué
                dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Button>().enabled = false;
                dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Image>().enabled = false;
                dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Animator>().enabled = false;

                //---- Grisé le donjon suivant------//
                if (i == dungeonLoader.dungeonUnlockedIndex)
                    if (i < dungeonOnTheMap.Length)
                    {
                        dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Button>().enabled = false;
                        dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Button>().interactable = false;
                        dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Image>().enabled = true;
                        dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Button>().image.color = Color.grey;
                        dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Animator>().enabled = false;
                        dungeonOnTheMap[i].transform.Find("Road").GetComponent<Image>().enabled = true;
                    }
            }
        }
    }

    public void ChangeDungeon(int hind)
    {
        dungeonIndex = hind;
        //ajoute au bouton actuel qui correspond à l'index sur la carte le fait de charger la salle donjon
        dungeonOnTheMap[dungeonIndex].transform.Find("DungeonButton").GetComponent<Button>().onClick.AddListener(LoadSceneDungeon);
    }

    //load the dungeon scene
    public void LoadSceneDungeon()
    {
        if (!loadbutton)
        {
            loadbutton = true;

            dungeonLoader.FadeInOutAnim();

            dungeonLoader.InstrantiateOnceEndDungeon = false;
            SceneManager.LoadScene("Explo"); // "Dungeon"
        }
    }

    public void UnlockNextDungeon()
    {
		dungeonLoader = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>();

        if (dungeonLoader.dungeonUnlockedIndex < dungeonOnTheMap.Length)
        {
            dungeonLoader.dungeonUnlockedIndex++;
        }
    }

    public void DecreaseUnlockDungeonIndex()
    {
		dungeonLoader = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>();

        if (dungeonLoader.dungeonUnlockedIndex > 1)
        {
            dungeonLoader.dungeonUnlockedIndex--;
        }
    }

    public void ResetUnlockDungeonIndex()
    {
		dungeonLoader = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>();

        dungeonLoader.dungeonUnlockedIndex = 1;
    }

    /* Accessors Method */
    public static MapController Instance
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
    public int DungeonIndex
    {
        get
        {
            return dungeonIndex;
        }
        set
        {
            dungeonIndex = value;
        }
    }
    public bool Loadbutton
    {
        get
        {
            return loadbutton;
        }
        set
        {
            loadbutton = value;
        }
    }
}
