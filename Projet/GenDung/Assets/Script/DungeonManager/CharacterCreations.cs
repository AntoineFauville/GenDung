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

		SizeOfTheTeam = GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedSizeOfTheTeam;

        //rempli la liste temporaire et du fichier de sauvegarde de theo et reinitialise ainsi le jeu 
        for (int i = 0; i < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList.Length; i++) {
            GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i] = charac[CharacterTempPlacement];
            tempCharac[i] = charac[CharacterTempPlacement];
        }

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
		GameObject.Find ("InfoCharaDispatch").GetComponent<Image> ().sprite = charac [indexMouseOverLeftPanel].TempSprite;
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
