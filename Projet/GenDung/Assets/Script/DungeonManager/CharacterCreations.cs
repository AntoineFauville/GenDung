using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterCreations : MonoBehaviour {

	int CharacterTempPlacement = 3;
	int CharacterTempPlacement2 = 1;

	public int SizeOfTheTeam;
	int realCalculationSize;

	public int indexMouseOverLeftPanel;
	public int indexMouseOverRightPanel;

	public Character[] charac;

	public GameObject[] RightTeam;

	// Use this for initialization
	void Start () {
		GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader>().FadeInOutAnim ();

		SizeOfTheTeam = GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedSizeOfTheTeam;

		//rempli la liste de theo et reinitialise ainsi le jeu
		for (int i = 0; i < GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedCharacterList.Length; i++) {
			GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedCharacterList [i] = charac [CharacterTempPlacement];
		}

	}
	
	// Update is called once per frame
	void Update () {

		if (SizeOfTheTeam > 4) {
			SizeOfTheTeam = 4;
		}
		if (SizeOfTheTeam < 1) {
			SizeOfTheTeam = 1;
		} 

		realCalculationSize = 4 - SizeOfTheTeam;

		for (int i = 0; i < SizeOfTheTeam; i++) {
			RightTeam [i].SetActive (true);
		}

		int a = 3;

		for (int i = 0; i < realCalculationSize; i++) {
			RightTeam [a].SetActive (false);
			a--;
		}

		GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedSizeOfTheTeam = SizeOfTheTeam;

		GameObject.Find ("InfoCharaDispatch").GetComponent<Image> ().sprite = charac [indexMouseOverLeftPanel].TempSprite;
		GameObject.Find ("PV").GetComponent<Text> ().text = 	"Health :          " + charac [indexMouseOverLeftPanel].Health_PV.ToString();
		GameObject.Find ("PA").GetComponent<Text> ().text = 	"Action Points :   " + charac [indexMouseOverLeftPanel].ActionPoints_PA.ToString();
		GameObject.Find ("CAC").GetComponent<Text> ().text = 	"Close Attack :    " + charac [indexMouseOverLeftPanel].CloseAttaqueValue.ToString();
		GameObject.Find ("DIST").GetComponent<Text> ().text = 	"Distance Attack : " + charac [indexMouseOverLeftPanel].DistanceAttaqueValue.ToString ();
		GameObject.Find ("Story").GetComponent<Text> ().text = charac [indexMouseOverLeftPanel].story;
	}

	public void SetRightIndex (int a) {
		indexMouseOverRightPanel = a;
	}

	public void SetLeftIndex (int b){
		indexMouseOverLeftPanel = b;
		GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedCharacterList [indexMouseOverRightPanel] = charac [indexMouseOverLeftPanel];
		RightTeam [indexMouseOverRightPanel].transform.GetChild (1).GetComponent<Image> ().sprite = charac [indexMouseOverLeftPanel].TempSprite;
	}

	public void GetBiggerTeam () {
		SizeOfTheTeam++;
	}

	public void GetSmallerTeam () {
		SizeOfTheTeam--;
	}

	public void LaunchGame () {
		SceneManager.LoadScene ("Map");
		GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader>().FadeInOutAnim ();
	}
}
