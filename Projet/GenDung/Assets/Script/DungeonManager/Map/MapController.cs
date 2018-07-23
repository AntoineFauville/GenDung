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

        //---------- ecran de fin de donjon -----------//
        if (dungeonLoader.EndDungeon && !dungeonLoader.InstrantiateOnceEndDungeon)
        {
            //instantie l'écran de fin
            dungeonLoader.InstrantiateOnceEndDungeon = true;
            Instantiate(Resources.Load("UI_Interface/EndDungeonUI"));
            
            //save all the player money
            this.transform.GetComponent<CurrencyGestion>().SaveMoney();
        }

        #endregion

        dungeonOnTheMap = GameObject.Find("PanelScriptDungeonList").GetComponent<DungeonListOnMap>().dungeonOnTheMapList; // Ajouter par mes soins ^^
        
        //---------- Dungeon Unlocking Feature ------------//
        if (dungeonLoader.dungeonUnlockedIndex <= dungeonOnTheMap.Length)
        {
            for (int i = (dungeonOnTheMap.Length -1); i > (dungeonLoader.dungeonUnlockedIndex -1); i--)
            {
                //Met faux tous les donjons non débloqué
                dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Button>().enabled = false;
                dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Image>().enabled = false;
                dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Animator>().enabled = false;

                ////---- Grisé le donjon suivant------//
                //if (i == dungeonLoader.dungeonUnlockedIndex)
                //    if (i < dungeonOnTheMap.Length)
                //    {
                //        dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Button>().enabled = false;
                //        dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Button>().interactable = false;
                //        dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Image>().enabled = true;
                //        dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Button>().image.color = Color.grey;
                //        dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Animator>().enabled = false;
                //        dungeonOnTheMap[i].transform.Find("Road").GetComponent<Image>().enabled = true;
                //    }
            }
        }
    }

    public void ChangeDungeon(int hind)
    {
        dungeonIndex = hind;
        //ajoute au bouton actuel qui correspond à l'index sur la carte le fait de charger la salle donjon
        dungeonOnTheMap[dungeonIndex]
            .transform.Find("DungeonButton")
            .GetComponent<Button>()
            .onClick.AddListener(
                LoadSceneDungeon);
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
