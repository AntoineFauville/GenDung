using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;

public class DungeonLoader : MonoBehaviour {

	public string activeScene, previousScene; // just a checker to see what room is the actual room that we are using.

	public GameObject 
	roomPrefab,
	doorPrefab,
	Nextdoor,
	Previousdoor; 

	GameObject BG;

	public RoomList[] roomListDungeon; // this are the dungeons, 

	public GameObject[] dungeonOnTheMap;

	public string[] sceneDungeonDependingOnList;

	int 
	index,
	dungeonIndex;

	public bool
	loadOnceScene,
	loadOnce3,
	loadOnce2,
	loadOnceMapScene,
	areWeWaitingToComeBack;

	void FixedUpdate () {
		//check activate scene
		activeScene = SceneManager.GetActiveScene ().name;

		//-----------Dungeon gestion scene-------------//
		if (activeScene == "Dungeon"){

			if (index > roomListDungeon [0].RoomOfTheDungeon.Count - 1) {
				LoadSceneMap ();
			}

			//check if we did instanciate once the door
			BG = GameObject.FindGameObjectWithTag ("backgroundOfRoom");

			GameObject InformationPanel;

			InformationPanel = GameObject.FindGameObjectWithTag ("informationPanel");

			if (InformationPanel != null) {

				InformationPanel.GetComponent<Text> ().text = roomListDungeon [0].RoomOfTheDungeon [index].doorList [0].doorName;
			}
			if(previousScene != activeScene && !areWeWaitingToComeBack) {
				LoadRoom ();
			}
		}		

		//-----------Map gestion scene-------------//
		if (activeScene == "Map") { 
			loadOnceMapScene = false;
			previousScene = null;

			//get the buttons witch are dungeons entry
			dungeonOnTheMap = GameObject.FindGameObjectsWithTag ("DungeonButtonMap");

			//asign to the buttons the loading scene onClick
			if (dungeonOnTheMap != null && !loadOnceScene) {
				for (int i = 1; i < roomListDungeon.Length; i++) {
					dungeonIndex = i-1;
					dungeonOnTheMap [dungeonIndex].GetComponent<Button> ().onClick.AddListener (LoadSceneDungeon);
					Debug.Log (dungeonIndex);
				}
			} else {
				Debug.Log ("dungeonOnTheMap is Null");
			}
		}
	}


	//load the dungeon scene
	void LoadSceneDungeon () {
		for (int i = 0; i < 1; i++) {
			if (!loadOnceScene) {
				Debug.Log ("you have clicked once on LoadSceneDungeon Button");
				SceneManager.LoadScene (sceneDungeonDependingOnList[dungeonIndex]);
				previousScene = "DebugMap";
				loadOnceScene = true;
			}
		}
	}

	void LoadSceneMap () {
		index = 0;
		if (!loadOnceMapScene) {
			areWeWaitingToComeBack = true;
			loadOnceMapScene = true;

			SceneManager.LoadScene ("Map");

			previousScene = "DebugDungeon";

			loadOnceScene = false;
			loadOnce3 = false;
			loadOnce2 = false;
			StartCoroutine ("waitLagToGetBackIntoDungeon");
		}
	}

	//load first time room function
	void LoadRoom () {
		
		if (!loadOnce2) {
			loadOnce2 = true;
			Debug.Log ("you have load the Room Once");
			Instantiate (roomPrefab);
			//index to say witch room are we using now
			index = 0;

			BG = GameObject.FindGameObjectWithTag ("backgroundOfRoom");
			BG.transform.GetComponent<Image> ().sprite = roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].back;

			//instantiate the door
			loadDoor();
			Debug.Log ("you have fully load the Room ");
		}
	}
		
	void GoDeeperInTheDungeon () {
		if (!loadOnce3) {
			loadOnce3 = true;

			//look throught all the stats and asign them to object in the scene depending on the tags
			BG.transform.GetComponent<Image> ().sprite = roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].back;

			loadDoor();

			StartCoroutine ("waitLagForClicking");
			}
		}

	IEnumerator waitLagForClicking () {
		yield return new WaitForSeconds (0.1f);
		loadOnce3 = false;
	}

	IEnumerator waitLagToGetBackIntoDungeon () {
		yield return new WaitForSeconds (0.3f);
		areWeWaitingToComeBack = false;
	}

	void loadDoor () {
		//initialise the door at each time we call it
		GameObject[] tempDoor;
		tempDoor = GameObject.FindGameObjectsWithTag ("door");

		if(tempDoor != null){
			for (int i = 0; i < tempDoor.Length; i++) {
				tempDoor [i].SetActive (false);
			}
		}

		GameObject doorinstantiated = Instantiate (doorPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;

		doorinstantiated.transform.SetParent (GameObject.FindGameObjectWithTag("Canvas").transform, false);

		doorinstantiated.transform.localPosition 
			= new Vector3 (
				roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].doorList [0].coordinate.x, 
				roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].doorList [0].coordinate.y,
				0);

		index = roomListDungeon [0].RoomOfTheDungeon [index].doorList [0].connectingTo;
		doorinstantiated.GetComponent<Button> ().onClick.AddListener(GoDeeperInTheDungeon);
	}
}
