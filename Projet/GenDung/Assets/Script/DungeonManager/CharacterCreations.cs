using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterCreations : MonoBehaviour {

	int CharacterTempPlacement = 11;

	public int SizeOfTheTeam;
	int realCalculationSize;

	public bool clicked;

	public int indexMouseOverLeftPanel;
	public int indexMouseOverRightPanel;

    public bool didweChooseTheTeam;

	public Character[] charac;

    public Character[] tempCharac;

    public GameObject[] RightTeam;

	// Use this for initialization
	void Start () {
		GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader>().FadeInOutAnim ();

        //SetRightIndex(0);
        //SetLeftIndex(11);


        SizeOfTheTeam = GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedSizeOfTheTeam;

        //rempli la liste temporaire et du fichier de sauvegarde de theo et reinitialise ainsi le jeu 
        for (int i = 0; i < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList.Length; i++) {
            GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i] = charac[CharacterTempPlacement];
            tempCharac[i] = charac[CharacterTempPlacement];
        }

        //reset des stats des personnages
        //Toinoung
        charac[0].Health_PV = 100;
        charac[0].ActionPoints_PA = 3;

        //Raph P
        charac[1].Health_PV = 150;
        charac[1].ActionPoints_PA = 2;

        //Raph V
		charac[2].Health_PV = 100;
        charac[2].ActionPoints_PA = 3;

        //Robin
		charac[3].Health_PV = 100;
        charac[3].ActionPoints_PA = 2;

        //Coco
		charac[4].Health_PV = 100;
        charac[4].ActionPoints_PA = 3;

        //Victor
		charac[5].Health_PV = 100;
        charac[5].ActionPoints_PA = 3;

        //Arthur
        charac[6].Health_PV = 100;
        charac[6].ActionPoints_PA = 4;

        //Aline
        charac[7].Health_PV = 80;
        charac[7].ActionPoints_PA = 3;

        //Vincent
		charac[8].Health_PV = 100;
        charac[8].ActionPoints_PA = 2;

        //AlexViking
		charac[9].Health_PV = 100;
        charac[9].ActionPoints_PA = 3;

        //Jojo
		charac[10].Health_PV = 100;
        charac[10].ActionPoints_PA = 3;

        //Theo
		charac[11].Health_PV = 100;
        charac[11].ActionPoints_PA = 4;

        //GUI
		charac[12].Health_PV = 100;
        charac[12].ActionPoints_PA = 4;

        //Flo
		charac[13].Health_PV = 100;
        charac[13].ActionPoints_PA = 1;

        //Ben
		charac[14].Health_PV = 100;
        charac[14].ActionPoints_PA = 2;

        //Brodco
		charac[15].Health_PV = 100;
        charac[15].ActionPoints_PA = 3;

        //Mehdi
		charac[16].Health_PV = 100;
        charac[16].ActionPoints_PA = 1;

        //Brice
		charac[17].Health_PV = 100;
        charac[17].ActionPoints_PA = 1;
    }
	
	// Update is called once per frame
	void Update () {

		if (tempCharac [0] != charac [11] &&
		    tempCharac [1] != charac [11] &&
		    tempCharac [2] != charac [11] &&
			tempCharac [3] != charac [11])
		{
			didweChooseTheTeam = true;
		}


        if (didweChooseTheTeam)
        {
            GameObject.Find("LaunchButton").GetComponent<Button>().interactable = true;
            GameObject.Find("LaunchButton").GetComponent<Image>().color = Color.white;
        }
        else {
            GameObject.Find("LaunchButton").GetComponent<Button>().interactable = false;
            GameObject.Find("LaunchButton").GetComponent<Image>().color = Color.grey;
        }

        //pour etre sur qu'on ne dépasse pas
		if (SizeOfTheTeam > 4) {
			SizeOfTheTeam = 4;
		}
		if (SizeOfTheTeam < 1) {
			SizeOfTheTeam = 1;
		} 

        //bonne taille de liste
		realCalculationSize = 4 - SizeOfTheTeam;

        //active en fonction de la taille de la liste les personnages
		for (int i = 0; i < SizeOfTheTeam; i++) {
			RightTeam [i].SetActive (true);
		}

        // 3 est le maximum de joueur que l'on doit supprimer pour l'affichage
		int a = 3;

		for (int i = 0; i < realCalculationSize; i++) {
			RightTeam [a].SetActive (false);
			a--;
		}

        //renvoie la taille de la liste
		GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedSizeOfTheTeam = SizeOfTheTeam;

        //dispatch les infos sur l'écran
		//GameObject.Find ("InfoCharaDispatch").GetComponent<Image> ().sprite = charac [indexMouseOverLeftPanel].TempSprite;
        //if (charac[indexMouseOverLeftPanel].hasAnimations == true)
        //{
        //    GameObject.Find("InfoCharaDispatch").GetComponent<Animator>().runtimeAnimatorController = charac[indexMouseOverLeftPanel].persoAnimator;
        //}
		//GameObject.Find ("PV").GetComponent<Text> ().text = 	"Health :          " + charac [indexMouseOverLeftPanel].Health_PV.ToString();
		//GameObject.Find ("PA").GetComponent<Text> ().text = 	"Action Points :   " + charac [indexMouseOverLeftPanel].ActionPoints_PA.ToString();
		//GameObject.Find ("CAC").GetComponent<Text> ().text = 	"Close Attack :    " + charac [indexMouseOverLeftPanel].CloseAttaqueValue.ToString();
		//GameObject.Find ("DIST").GetComponent<Text> ().text = 	"Distance Attack : " + charac [indexMouseOverLeftPanel].DistanceAttaqueValue.ToString ();
		//GameObject.Find ("Story").GetComponent<Text> ().text = charac [indexMouseOverLeftPanel].story;
	}

	public void SetUpInfo(int indexOfTeamMember){
		GameObject.Find ("Panel"+indexOfTeamMember).transform.Find ("StatPanel/PV").GetComponent<Text> ().text = 	"Health :          " + charac [indexOfTeamMember].Health_PV.ToString();
		GameObject.Find ("Panel"+indexOfTeamMember).transform.Find ("StatPanel/PA").GetComponent<Text> ().text = 	"Action Points :   " + charac [indexOfTeamMember].ActionPoints_PA.ToString();
		GameObject.Find ("Panel"+indexOfTeamMember).transform.Find ("StatPanel/CAC").GetComponent<Text> ().text = 	"Close Attack :    " + charac [indexOfTeamMember].CloseAttaqueValue.ToString();
		GameObject.Find ("Panel"+indexOfTeamMember).transform.Find ("StatPanel/DIST").GetComponent<Text> ().text = 	"Distance Attack : " + charac [indexOfTeamMember].DistanceAttaqueValue.ToString ();
		GameObject.Find ("Panel"+indexOfTeamMember).transform.Find ("StoryPanel/Story").GetComponent<Text> ().text = charac [indexOfTeamMember].story;
	}

    //index de la liste qui correspond a celle de l'équipe right = équipe left = temporaire
	public void SetRightIndex (int a) {
		indexMouseOverRightPanel = a;
	}

	public void ClickedVoid (bool click){
		clicked = click;
	}

	public void SetLeftIndex (int b){
		if (clicked) {
			indexMouseOverLeftPanel = b;
			clicked = false;
		}

		if (tempCharac[0] == charac[indexMouseOverLeftPanel] ||
			tempCharac[1] == charac[indexMouseOverLeftPanel] ||
			tempCharac[2] == charac[indexMouseOverLeftPanel] ||
			tempCharac[3] == charac[indexMouseOverLeftPanel]
		) {
			print ("nop");
		} else {
			
			tempCharac [indexMouseOverRightPanel] = charac [indexMouseOverLeftPanel];

			if (charac[indexMouseOverLeftPanel].hasAnimations == true)
			{
				RightTeam[indexMouseOverRightPanel].transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = tempCharac[indexMouseOverRightPanel].persoAnimator;
				//didweChooseTheTeam = true;
			} else {
				RightTeam [indexMouseOverRightPanel].transform.GetChild (1).GetComponent<Animator> ().runtimeAnimatorController = null;
				RightTeam [indexMouseOverRightPanel].transform.GetChild (1).GetComponent<Image> ().sprite = tempCharac [indexMouseOverRightPanel].TempSprite;
			}
		}
    }

    public void GetBiggerTeam () {
		SizeOfTheTeam++;
	}

	public void GetSmallerTeam () {
		SizeOfTheTeam--;
	}

	public void LaunchGame () {

        //set the list to the list we choosed
        for (int i = 0; i < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList.Length; i++)
        {
            GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i] = tempCharac[i];
        }
        GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().FadeInOutAnim();
        SceneManager.LoadScene ("NewTavern");
	}
}
