using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavingSystem : MonoBehaviour {

	bool waitToSaveAgain;

	public GameData gameData;

	// Update is called once per frame
	void Update () {
		if (!waitToSaveAgain) {
			StartCoroutine ("AutoSaving");
			waitToSaveAgain = true;
		}
	}

	public void SaveGame (){

		if (SceneManager.GetActiveScene ().name == "Map") {
			gameData.DungeonIndexData = GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().dungeonUnlockedIndex;
		}
	}

	//------IENUMERATOR------//
	IEnumerator AutoSaving(){
		yield return new WaitForSeconds (2f);
		SaveGame ();
		waitToSaveAgain = false;
		//Debug.Log ("GameSaved");
	}
}
