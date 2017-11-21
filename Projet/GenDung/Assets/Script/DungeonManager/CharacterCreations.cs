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

	public int indexMouseOverLeftPanel;
	public int indexMouseOverRightPanel;

	public Character[] charac;

    public Character[] tempCharac;

    public GameObject[] RightTeam;

	// Use this for initialization
	void Start () {
		GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader>().FadeInOutAnim ();

        SetRightIndex(0);
        SetLeftIndex(11);


        SizeOfTheTeam = GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedSizeOfTheTeam;

        //rempli la liste temporaire et du fichier de sauvegarde de theo et reinitialise ainsi le jeu 
        for (int i = 0; i < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList.Length; i++) {
            GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i] = charac[CharacterTempPlacement];
            tempCharac[i] = charac[CharacterTempPlacement];
        }

        //reset des stats des personnages
        //Toinoung
        charac[0].Health_PV = 4;
        charac[0].ActionPoints_PA = 4;
        charac[0].CloseAttaqueValue = 0;
        charac[0].DistanceAttaqueValue = 2;

        //Raph P
        charac[1].Health_PV = 5;
        charac[1].ActionPoints_PA = 1;
        charac[1].CloseAttaqueValue = 4;
        charac[1].DistanceAttaqueValue = 0;

        //Raph V
        charac[2].Health_PV = 4;
        charac[2].ActionPoints_PA = 3;
        charac[2].CloseAttaqueValue = 1;
        charac[2].DistanceAttaqueValue = 2;

        //Robin
        charac[3].Health_PV = 4;
        charac[3].ActionPoints_PA = 2;
        charac[3].CloseAttaqueValue = 0;
        charac[3].DistanceAttaqueValue = 4;

        //Coco
        charac[4].Health_PV = 6;
        charac[4].ActionPoints_PA = 1;
        charac[4].CloseAttaqueValue = 3;
        charac[4].DistanceAttaqueValue = 0;

        //Victor
        charac[5].Health_PV = 5;
        charac[5].ActionPoints_PA = 3;
        charac[5].CloseAttaqueValue = 2;
        charac[5].DistanceAttaqueValue = 0;

        //Arthur
        charac[6].Health_PV = 3;
        charac[6].ActionPoints_PA = 4;
        charac[6].CloseAttaqueValue = 0;
        charac[6].DistanceAttaqueValue = 3;

        //Aline
        charac[7].Health_PV = 2;
        charac[7].ActionPoints_PA = 3;
        charac[7].CloseAttaqueValue = 1;
        charac[7].DistanceAttaqueValue = 4;

        //Vincent
        charac[8].Health_PV = 4;
        charac[8].ActionPoints_PA = 2;
        charac[8].CloseAttaqueValue = 4;
        charac[8].DistanceAttaqueValue = 0;

        //AlexViking
        charac[9].Health_PV = 5;
        charac[9].ActionPoints_PA = 3;
        charac[9].CloseAttaqueValue = 2;
        charac[9].DistanceAttaqueValue = 0;

        //Jojo
        charac[10].Health_PV = 2;
        charac[10].ActionPoints_PA = 3;
        charac[10].CloseAttaqueValue = 0;
        charac[10].DistanceAttaqueValue = 5;

        //Theo
        charac[11].Health_PV = 3;
        charac[11].ActionPoints_PA = 4;
        charac[11].CloseAttaqueValue = 2;
        charac[11].DistanceAttaqueValue = 1;

        //GUI
        charac[12].Health_PV = 2;
        charac[12].ActionPoints_PA = 4;
        charac[12].CloseAttaqueValue = 4;
        charac[12].DistanceAttaqueValue = 0;

        //Flo
        charac[13].Health_PV = 5;
        charac[13].ActionPoints_PA = 1;
        charac[13].CloseAttaqueValue = 4;
        charac[13].DistanceAttaqueValue = 0;

        //Ben
        charac[14].Health_PV = 4;
        charac[14].ActionPoints_PA = 2;
        charac[14].CloseAttaqueValue = 4;
        charac[14].DistanceAttaqueValue = 0;

        //Brodco
        charac[15].Health_PV = 3;
        charac[15].ActionPoints_PA = 3;
        charac[15].CloseAttaqueValue = 3;
        charac[15].DistanceAttaqueValue = 1;

        //Mehdi
        charac[16].Health_PV = 1;
        charac[16].ActionPoints_PA = 0;
        charac[16].CloseAttaqueValue = 1;
        charac[16].DistanceAttaqueValue = 0;

        //Brice
        charac[17].Health_PV = 6;
        charac[17].ActionPoints_PA = 1;
        charac[17].CloseAttaqueValue = 3;
        charac[17].DistanceAttaqueValue = 0;

    }
	
	// Update is called once per frame
	void Update () {

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
        GameObject.Find ("PV").GetComponent<Text> ().text = 	"Health :          " + charac [indexMouseOverLeftPanel].Health_PV.ToString();
		GameObject.Find ("PA").GetComponent<Text> ().text = 	"Action Points :   " + charac [indexMouseOverLeftPanel].ActionPoints_PA.ToString();
		GameObject.Find ("CAC").GetComponent<Text> ().text = 	"Close Attack :    " + charac [indexMouseOverLeftPanel].CloseAttaqueValue.ToString();
		GameObject.Find ("DIST").GetComponent<Text> ().text = 	"Distance Attack : " + charac [indexMouseOverLeftPanel].DistanceAttaqueValue.ToString ();
		GameObject.Find ("Story").GetComponent<Text> ().text = charac [indexMouseOverLeftPanel].story;
	}

    //index de la liste qui correspond a celle de l'équipe right = équipe left = temporaire
	public void SetRightIndex (int a) {
		indexMouseOverRightPanel = a;
	}

	public void SetLeftIndex (int b){
		indexMouseOverLeftPanel = b;

        tempCharac[indexMouseOverRightPanel] = charac[indexMouseOverLeftPanel];

        RightTeam [indexMouseOverRightPanel].transform.GetChild (1).GetComponent<Image> ().sprite = tempCharac [indexMouseOverRightPanel].TempSprite;

        if (charac[indexMouseOverLeftPanel].hasAnimations == true)
        {
            RightTeam[indexMouseOverRightPanel].transform.GetChild(1).GetComponent<Animator>().runtimeAnimatorController = tempCharac[indexMouseOverRightPanel].persoAnimator;
        }
    }

    public void GetBiggerTeam () {
		SizeOfTheTeam++;
	}

	public void GetSmallerTeam () {
		SizeOfTheTeam--;
	}

	public void LaunchGame () {

        for (int i = 0; i < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList.Length; i++)
        {
            GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i] = tempCharac[i];
        }

        SceneManager.LoadScene ("Map");
		GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader>().FadeInOutAnim ();
	}
}
