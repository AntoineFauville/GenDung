using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;

public class DungeonLoader : MonoBehaviour {

	public string activeScene, previousScene, roomType; // just a checker to see what room is the actual room that we are using.

	public GameObject 
	roomPrefab,
	doorPrefab,
	chestRoomUIPrefab,
	fightRoomUIPrefab,
	bossRoomUIPrefab;

	GameObject 
	BG, 
	doorinstantiated, 
	InformationPanel,
	Nextdoor,
	Previousdoor;

	public RoomList[] roomListDungeon; // this are the dungeons, 

	public GameObject[] dungeonOnTheMap;

	public string[] sceneDungeonDependingOnList;

	int 
	index,
	dungeonIndex = 0;

	public bool
	loadOnceScene,
	loadOnce3,
	loadOnce2,
	loadOnceMapScene,
	loadOnceDoor,
	areWeWaitingToComeBack,
	roomIsLocked,
	isUIinstantiated;

	void FixedUpdate () {
		//check activate scene
		activeScene = SceneManager.GetActiveScene ().name;

		//-----------Dungeon gestion scene-------------//
		if (activeScene == "Dungeon"){

			//check if we did instanciate once the door
			BG = GameObject.FindGameObjectWithTag ("backgroundOfRoom");

			InformationPanel = GameObject.FindGameObjectWithTag ("informationPanel");

			if (InformationPanel != null && !roomIsLocked) {
				if (roomListDungeon [dungeonIndex].RoomOfTheDungeon[index].roomID < roomListDungeon [dungeonIndex].RoomOfTheDungeon.Count()){
					InformationPanel.GetComponent<Text> ().text = roomListDungeon [dungeonIndex].RoomOfTheDungeon [index-1].doorList [0].doorName;
				}
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
			dungeonIndex = 0;
			//asign to the buttons the loading scene onClick
			if (dungeonOnTheMap != null && !loadOnceScene) {
				for (int i = 0; i < dungeonOnTheMap.Length; i++) {
					
					dungeonOnTheMap [i].GetComponent<Button> ().onClick.AddListener (LoadSceneDungeon);
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
				//Debug.Log ("you have clicked once on LoadSceneDungeon Button");
				SceneManager.LoadScene (sceneDungeonDependingOnList[0]);
				previousScene = "DebugMap";
				loadOnceScene = true;
			}
		}
	}

	void LoadSceneMap () {
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
			//Debug.Log ("you have load the Room Once");
			Instantiate (roomPrefab);
			//index to say witch room are we using now
			index = 0;

			BG = GameObject.FindGameObjectWithTag ("backgroundOfRoom");

			BG.transform.GetComponent<Image> ().sprite = roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].back;

			//instantiate the door
			loadDoor();

			GetRoomType ();
			//Debug.Log ("you have fully load the Room ");
		}
	}
		
	void GoDeeperInTheDungeon () {
		if (!loadOnce3) {
			//print ("GoDeeper index : " + index);
			if (!roomIsLocked) {
				loadOnce3 = true;

				//reset for ui
				isUIinstantiated = false;

				//look throught all the stats and asign them to object in the scene depending on the tags
				//Debug.Log ("Im Index Bug B");
				if (roomListDungeon [dungeonIndex].RoomOfTheDungeon[index].roomID <= roomListDungeon [0].RoomOfTheDungeon.Count)
					BG.transform.GetComponent<Image> ().sprite = roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].back;
			
				loadDoor ();
				GetRoomType ();
				StartCoroutine ("waitLagForClicking");
				}
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

	void GetRoomType()
	{
		//Debug.Log ("Im Index Bug C");
		if (roomListDungeon [dungeonIndex].RoomOfTheDungeon[index-1].roomID <= roomListDungeon [0].RoomOfTheDungeon.Count)
			roomType = roomListDungeon [dungeonIndex].RoomOfTheDungeon [index-1].roomType.ToString(); 

		if (roomType == "chest") {
			roomIsLocked = true;
			GameObject.FindGameObjectWithTag("door").GetComponent<Button> ().onClick.AddListener (ChangeInformationPanel);

			if (!isUIinstantiated) {
				isUIinstantiated = true;
				Instantiate (chestRoomUIPrefab);
			}
			GameObject.FindGameObjectWithTag ("unlockRoomButton").GetComponent<Button> ().onClick.AddListener (UnlockRoom);
		}

		if (roomType == "fight") {
			roomIsLocked = true;
			GameObject.FindGameObjectWithTag("door").GetComponent<Button> ().onClick.AddListener (ChangeInformationPanel);

			if (!isUIinstantiated) {
				isUIinstantiated = true;
				Instantiate (fightRoomUIPrefab);
			}
			GameObject.FindGameObjectWithTag ("unlockRoomButton").GetComponent<Button> ().onClick.AddListener (UnlockRoom);
		}

		if (roomType == "boss") {
			roomIsLocked = true;
			GameObject.FindGameObjectWithTag("door").GetComponent<Button> ().onClick.AddListener (ChangeInformationPanel);

			if (!isUIinstantiated) {
				isUIinstantiated = true;
				Instantiate (bossRoomUIPrefab);
			}
			GameObject.FindGameObjectWithTag ("unlockRoomButton").GetComponent<Button> ().onClick.AddListener (UnlockRoom);
		}
	}

	void ChangeInformationPanel(){
		if (roomType == "chest") {
			//Debug.Log ("Door is locked");
			InformationPanel.GetComponent<Text> ().text = "Door is locked, find the chest first";
		}
		if (roomType == "fight") {
			//Debug.Log ("Door is locked");
			InformationPanel.GetComponent<Text> ().text = "Door is locked, you have to fight";
		}
		if (roomType == "boss") {
			//Debug.Log ("Door is locked");
			InformationPanel.GetComponent<Text> ().text = "Door is locked, you have to fight the boss";
		}
	}

	void loadDoor () {
		if (!loadOnceDoor) {
			loadOnceDoor = true;
			//initialise the door at each time we call it
			GameObject[] tempDoor;
			tempDoor = GameObject.FindGameObjectsWithTag ("door");

			if (tempDoor != null) {
				for (int i = 0; i < tempDoor.Length; i++) {
					tempDoor [i].SetActive (false);
				}
			}

			doorinstantiated = Instantiate (doorPrefab, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;

			doorinstantiated.transform.SetParent (GameObject.FindGameObjectWithTag ("Canvas").transform, false);


				doorinstantiated.transform.localPosition 
					= new Vector3 (
					roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].doorList [0].coordinate.x, 
					roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].doorList [0].coordinate.y,
					0);

			//Debug.Log ("Im Index Bug D");
			if (roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].roomID <= roomListDungeon [dungeonIndex].RoomOfTheDungeon.Count) {	
				
				index = roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].doorList [0].connectingTo - 1;
				print ("Actual Index : " + index);

				//Debug.Log ("Im Index Bug E");
				if (roomListDungeon [dungeonIndex].RoomOfTheDungeon [index-1].roomID < roomListDungeon [dungeonIndex].RoomOfTheDungeon.Count) {
					doorinstantiated.GetComponent<Button> ().onClick.AddListener (GoDeeperInTheDungeon);
				} else {
					doorinstantiated.GetComponent<Button> ().onClick.AddListener (IndexAddingLoadMap);
				}	
			}
			StartCoroutine ("loadWaitRoom");
		}
	}

	void IndexAddingLoadMap(){
		LoadSceneMap ();
	}

	IEnumerator loadWaitRoom(){
		yield return new WaitForSeconds (0.3f);
		loadOnceDoor = false;
	}

	public void UnlockRoom () {
		roomIsLocked = false;

		GameObject.FindGameObjectWithTag ("canvasInDungeon").SetActive (false);
		//print ("Unlock index : " + index);
	}

}
