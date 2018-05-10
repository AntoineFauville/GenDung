using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TavernController : MonoBehaviour {

    private static TavernController instance;

    private DungeonLoader dungeonLoader;
    private bool questStartOn;

    //all int for upgrade temp
    private int healthTemp,
        actionTemp,
        cACTemp,
        distTemp;


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

    public void Tavern()
    {
        dungeonLoader = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>();

        //affiche le canvas Quest Start si c'est la premiere fois qu'on vient dans la taverne
        if (questStartOn)
        {
            questStartOn = false;

            Instantiate(Resources.Load("UI_Interface/CanvasQuestStart"));

        }

        //montre le nombre de gold que possede le joueur pour l'achat de upgrade
        //GameObject.Find("GoldTotalPlayerUp").GetComponent<Text>().text = "Your Gold : " + GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.PlayerMoney.ToString();

        //a faire ajouter l'index par rapport a la liste des personnages qu'on a selectionner
        //affiche l'image dans le panel en fonction du personnage selectionner avec le bouton
        //GameObject.Find("CharDisImage").GetComponent<Image>().sprite = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[0].TempSprite;
       // GameObject.Find("HistoryText").GetComponent<Text>().text = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[0].story;




        if (!DungeonLoader.Instance.DoOnceAllRelatedToUpgradeTavernPanel)
        {
			//ajoute autour du feu les membres de l'équipe
			for (int i = 0; i < GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedSizeOfTheTeam; i++) {
				GameObject.Find("PersoTeamTaverne" + i).GetComponent<Image>().sprite = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].TempSprite;
				// + animator
				GameObject.Find("PersoTeamTaverne" + i).GetComponent<Animator>().runtimeAnimatorController = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].persoAnimator;
			}
			/*
            //stoque les valeurs du fichier de sauvegarde au niveau de la vie etc pour les modifiers localement
            healthTemp = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[0].Health_PV;
            actionTemp = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[0].ActionPoints_PA;
            cACTemp = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[0].CloseAttaqueValue;
            distTemp = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[0].DistanceAttaqueValue;

            DungeonLoader.Instance.DoOnceAllRelatedToUpgradeTavernPanel = true;
            GameObject.Find("CanvasUpgradePanel").GetComponent<Canvas>().enabled = false;
            //pour chaque membre de l'équipe ajoute un nouveau bouton au menu pour passer d'un membre a un autre dans le panel upgrade
            for (int i = 0; i < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedSizeOfTheTeam; i++)
            {
                //ajoute un nouveau bouton en haut qui permet de naviguer entre les personnages
                DungeonLoader.Instance.CharacterUI = Instantiate(Resources.Load("UI_Interface/TeamMemberUp")) as GameObject;
                DungeonLoader.Instance.CharacterUI.transform.SetParent(GameObject.Find("TeamUpPanel").transform, false);
                DungeonLoader.Instance.CharacterUI.transform.Find("TeamSpriteImage").GetComponent<Image>().sprite = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].TempSprite;
            }

            //a ajouter pour chaque membre de l'équipe
           */
        }
		/*
        //stoque les valeurs du fichier de sauvegarde au niveau de la vie etc pour les modifiers localement
        //PV
        GameObject.Find("HealthText").GetComponent<Text>().text = "Character Health : " + healthTemp.ToString();

        //PA
        GameObject.Find("ActionText").GetComponent<Text>().text = "Character Action Points : " + actionTemp.ToString();

        //CAC
        GameObject.Find("AttackText").GetComponent<Text>().text = "Character Close Battle Attack : " + cACTemp.ToString();

        //Dist
        GameObject.Find("DistText").GetComponent<Text>().text = "Character Distance Attack : " + distTemp.ToString();
		*/
    }

    /* Accessors Method */
    public static TavernController Instance
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
    public bool QuestStartOn
    {
        get
        {
            return questStartOn;
        }
        set
        {
            questStartOn = value;
        }
    }
    public int HealthTemp
    {
        get
        {
            return healthTemp;
        }
        set
        {
            healthTemp = value;
        }
    }
    public int ActionTemp
    {
        get
        {
            return actionTemp;
        }
        set
        {
            actionTemp = value;
        }
    }
    public int CACTemp
    {
        get
        {
            return cACTemp;
        }
        set
        {
            cACTemp = value;
        }
    }
    public int DistTemp
    {
        get
        {
            return distTemp;
        }
        set
        {
            distTemp = value;
        }
    }
}
