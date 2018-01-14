using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour {

    private DungeonLoader dungeonLoader;

    public void Start()
    {
        dungeonLoader = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>();
    }

    public void Loutre()
    {
        dungeonLoader = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>();

        //reset la taverne
        DungeonLoader.Instance.DoOnceAllRelatedToUpgradeTavernPanel = false;

        //show on the map current money
        GameObject.Find("GoldUIDispatcherText").GetComponent<Text>().text = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.PlayerMoney.ToString();


        //----------ecran de fin de donjon-----------//
        if (dungeonLoader.EndDungeon && !dungeonLoader.InstrantiateOnceEndDungeon)
        {

            //instantie l'écran de fin
            dungeonLoader.InstrantiateOnceEndDungeon = true;
            Instantiate(Resources.Load("UI_Interface/EndDungeonUI"));

            //montre le montant de gold que le donjon a donné
            GameObject.Find("GoldDispatchEndUI").GetComponent<Text>().text = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().roomListDungeon[dungeonLoader.dungeonIndex].dungeonGold.ToString();

            //verifie dans toutes les salles
            for (int i = 0; i < dungeonLoader.roomListDungeon[dungeonLoader.dungeonIndex].RoomOfTheDungeon.Count; i++)
            {

                //si la salle est de type fight
                if (dungeonLoader.roomListDungeon[dungeonLoader.dungeonIndex].RoomOfTheDungeon[i].roomType.ToString() == "fight")
                {

                    //prendre ses enfants et instantier une icone pour chaque + definir leur parent dans l'écran de fin
                    for (int l = 0; l < dungeonLoader.roomListDungeon[dungeonLoader.dungeonIndex].RoomOfTheDungeon[i].enemies; l++)
                    {

                        GameObject enemyUI;
                        enemyUI = Instantiate(Resources.Load("UI_Interface/EnemiesPanelUI")) as GameObject;
                        enemyUI.transform.SetParent(GameObject.FindGameObjectWithTag("EnemyPanel").transform, false);
                        enemyUI.transform.GetChild(0).GetComponent<Image>().sprite = dungeonLoader.roomListDungeon[dungeonLoader.dungeonIndex].RoomOfTheDungeon[i].enemiesList[l].enemyIcon; // ==> Enemy HERE 
                    }
                }

                //si la salle est de type boss
                if (dungeonLoader.roomListDungeon[dungeonLoader.dungeonIndex].RoomOfTheDungeon[i].roomType.ToString() == "boss")
                {

                    //prendre ses enfants et instantier une icone pour chaque + definir leur parent dans l'écran de fin
                    for (int l = 0; l < dungeonLoader.roomListDungeon[dungeonLoader.dungeonIndex].RoomOfTheDungeon[i].enemies; l++)
                    {

                        GameObject enemyUI;
                        enemyUI = Instantiate(Resources.Load("UI_Interface/EnemiesPanelUI")) as GameObject;
                        enemyUI.transform.SetParent(GameObject.FindGameObjectWithTag("EnemyPanel").transform, false);
                        enemyUI.transform.Find("Icon").GetComponent<Image>().sprite = dungeonLoader.roomListDungeon[dungeonLoader.dungeonIndex].RoomOfTheDungeon[i].enemiesList[l].enemyIcon;
                    }

                    //vu que c'est un type boss il y a aussi le boss a instancier
                    GameObject bossUI;
                    bossUI = Instantiate(Resources.Load("UI_Interface/BossPanelUI")) as GameObject;
                    bossUI.transform.SetParent(GameObject.FindGameObjectWithTag("EnemyPanel").transform, false);
                    bossUI.transform.Find("Icon").GetComponent<Image>().sprite = dungeonLoader.roomListDungeon[dungeonLoader.dungeonIndex].RoomOfTheDungeon[i].bossList[0].bossIcon;
                }
            }

            //ajoute un montant d or au joueur
            this.transform.GetComponent<CurrencyGestion>().IncreaseMoney(dungeonLoader.roomListDungeon[dungeonLoader.dungeonIndex].dungeonGold);

            //save all the player money
            this.transform.GetComponent<CurrencyGestion>().SaveMoney();
        }

        //va rechercher dans la liste de donjon dans le prefab de carte l'index qui permet de savoir en passant la souris dans quel donjon on va entrer
        dungeonLoader.dungeonIndex = GameObject.FindGameObjectWithTag("DungeonButtonMap").GetComponent<DungeonListOnMap>().indexLocal;

        //ajoute a tout les boutons sur la carte le fait de charger la salle donjon

        dungeonLoader.dungeonOnTheMap[dungeonLoader.dungeonIndex].transform.Find("DungeonButton").GetComponent<Button>().onClick.AddListener(dungeonLoader.LoadSceneDungeon);
        //assure que les salles sont bien unlock
        dungeonLoader.roomIsLocked = false;
        //reinitialise le systeme de check de salle
        dungeonLoader.previousScene = null;


        //----------Dungeon Unlocking Feature ------------//
        if (dungeonLoader.dungeonUnlockedIndex <= dungeonLoader.dungeonOnTheMap.Length)
        {

            for (int i = 0; i < dungeonLoader.dungeonOnTheMap.Length; i++)
            {
                //Met faux tous les donjons non débloqué
                dungeonLoader.dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Button>().enabled = false;
                dungeonLoader.dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Image>().enabled = false;
                dungeonLoader.dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Animator>().enabled = false;
                if (i > 0)
                {
                    dungeonLoader.dungeonOnTheMap[i].transform.Find("Road").GetComponent<Image>().enabled = false;
                }
            }
            for (int i = 0; i < dungeonLoader.dungeonUnlockedIndex; i++)
            {
                //Met vrai tous les donjons débloqué
                dungeonLoader.dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Button>().enabled = true;
                dungeonLoader.dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Button>().interactable = true;
                dungeonLoader.dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Image>().enabled = true;
                dungeonLoader.dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Button>().image.color = Color.white;
                dungeonLoader.dungeonOnTheMap[i].transform.Find("DungeonButton").GetComponent<Animator>().enabled = true;

                if (i > 0)
                {
                    dungeonLoader.dungeonOnTheMap[i].transform.Find("Road").GetComponent<Image>().enabled = true;
                }

                //---- Grisé le donjon suivant------//
                if (i + 1 < dungeonLoader.dungeonOnTheMap.Length)
                {
                    dungeonLoader.dungeonOnTheMap[i + 1].transform.Find("DungeonButton").GetComponent<Button>().enabled = false;
                    dungeonLoader.dungeonOnTheMap[i + 1].transform.Find("DungeonButton").GetComponent<Button>().interactable = false;
                    dungeonLoader.dungeonOnTheMap[i + 1].transform.Find("DungeonButton").GetComponent<Image>().enabled = true;
                    dungeonLoader.dungeonOnTheMap[i + 1].transform.Find("DungeonButton").GetComponent<Button>().image.color = Color.grey;
                    dungeonLoader.dungeonOnTheMap[i + 1].transform.Find("DungeonButton").GetComponent<Animator>().enabled = false;
                    dungeonLoader.dungeonOnTheMap[i + 1].transform.Find("Road").GetComponent<Image>().enabled = true;
                }
            }
        }
    }
}
