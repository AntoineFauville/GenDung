using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using UnityEngine.EventSystems;

public class DungeonLoader : MonoBehaviour {

	public string activeScene, previousScene, roomType; // just a checker to see what room is the actual room that we are using.

	public GameObject 
	roomPrefab,
	doorPrefab,
	chestRoomUIPrefab,
	fightRoomUIPrefab,
	bossRoomUIPrefab,
	enemyPrefabUIICON,
	bossPrefabUIICON;

	GameObject 
	BG, 
	doorinstantiated, 
	InformationPanel,
	Nextdoor,
	Previousdoor;

	public RoomList[] roomListDungeon; // this are the dungeons, 

	public GameObject[] dungeonOnTheMap;

	int index;
	public int dungeonIndex;

	public bool
	loadOnce3,
	loadOnce2,
	loadbutton,
	loadbutton2,
	loadOnceDoor,
	roomIsLocked,
	isUIinstantiated,
	doOnceCoroutine,
	sceneLoaded;

	void Start () {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode){
		sceneLoaded = true;
	}


	void FixedUpdate () {

		activeScene = SceneManager.GetActiveScene ().name;

		if (!sceneLoaded) {
			//-----------Dungeon gestion scene-------------//
			if (activeScene == "Dungeon") {

				//check if we did instanciate once the door
				BG = GameObject.FindGameObjectWithTag ("backgroundOfRoom");

				/*if (InformationPanel != null && !roomIsLocked) {
					if (roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].roomID < roomListDungeon [dungeonIndex].RoomOfTheDungeon.Count ()) {
						InformationPanel.GetComponent<Text> ().text = roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].doorList [0].doorName;
					}
				}*/
				if (previousScene != activeScene) {
					LoadRoom ();
				}
			}		

			//-----------Map gestion scene-------------//
			if (activeScene == "Map") { 

				dungeonIndex = GameObject.FindGameObjectWithTag ("DungeonButtonMap").GetComponent<DungeonListOnMap> ().indexLocal;
				dungeonOnTheMap [dungeonIndex].GetComponent<Button> ().onClick.AddListener (LoadSceneDungeon);

				roomIsLocked = false;
				previousScene = null;

			}
		} else {
			if (!doOnceCoroutine) {
				doOnceCoroutine = true;
				StartCoroutine ("WaitLoading");
			}
		}
	}

	//load the dungeon scene
	void LoadSceneDungeon () {
		if (!loadbutton) {
			loadbutton = true;
			SceneManager.LoadScene ("Dungeon");
		}
	}

	void LoadSceneMap () {
		if (!loadbutton2) {
			loadbutton2 = true;
			SceneManager.LoadScene ("Map");
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
				if (roomListDungeon [dungeonIndex].RoomOfTheDungeon[index].roomID <= roomListDungeon [dungeonIndex].RoomOfTheDungeon.Count)
					BG.transform.GetComponent<Image> ().sprite = roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].back;
			
				loadDoor ();
				GetRoomType ();

				//if (roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].roomID < roomListDungeon [dungeonIndex].RoomOfTheDungeon.Count ()) {	
				//	print ("Actual Index : " + index);

					index = roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].doorList [0].connectingTo - 1;
				//}
				StartCoroutine ("waitLagForClicking");
				}
			}
		}

	IEnumerator waitLagForClicking () {
		yield return new WaitForSeconds (0.1f);
		loadOnce3 = false;
	}

	void GetRoomType()
	{
		//Debug.Log ("Im Index Bug C");
		if (roomListDungeon [dungeonIndex].RoomOfTheDungeon[index].roomID <= roomListDungeon [dungeonIndex].RoomOfTheDungeon.Count)
			roomType = roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].roomType.ToString(); 

		if (roomType == "chest") {
			roomIsLocked = true;
			//GameObject.FindGameObjectWithTag("door").GetComponent<Button> ().onClick.AddListener (ChangeInformationPanel);

			if (!isUIinstantiated) {
				isUIinstantiated = true;
				Instantiate (chestRoomUIPrefab);
			}
			GameObject.FindGameObjectWithTag ("unlockRoomButton").GetComponent<Button> ().onClick.AddListener (UnlockRoom);
		}

		if (roomType == "fight") {
			roomIsLocked = true;
			//GameObject.FindGameObjectWithTag("door").GetComponent<Button> ().onClick.AddListener (ChangeInformationPanel);

			if (!isUIinstantiated) {
				isUIinstantiated = true;
				Instantiate (fightRoomUIPrefab);
			}
			GameObject.FindGameObjectWithTag ("unlockRoomButton").GetComponent<Button> ().onClick.AddListener (UnlockRoom);

			for (int i = 0; i < roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].enemies; i++) {

				GameObject enemyUI;

				enemyUI = Instantiate (enemyPrefabUIICON);

				enemyUI.transform.SetParent (GameObject.FindGameObjectWithTag ("EnemyPanel").transform, false);

				enemyUI.transform.GetChild(0).GetComponent<Image> ().sprite = roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].enemiesList [i].enemyIcon;
			}

		}

		if (roomType == "boss") {
			roomIsLocked = true;
			//GameObject.FindGameObjectWithTag("door").GetComponent<Button> ().onClick.AddListener (ChangeInformationPanel);

			if (!isUIinstantiated) {
				isUIinstantiated = true;
				Instantiate (bossRoomUIPrefab);
			}
			GameObject.FindGameObjectWithTag ("unlockRoomButton").GetComponent<Button> ().onClick.AddListener (UnlockRoom);

			GameObject bossUI;

			bossUI = Instantiate (bossPrefabUIICON);
			bossUI.transform.SetParent (GameObject.FindGameObjectWithTag ("EnemyPanel").transform, false);

			bossUI.transform.GetChild(0).GetComponent<Image> ().sprite = roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].bossList [0].bossIcon;

			for (int i = 0; i < roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].enemies; i++) {

				GameObject enemyBossUI;

				enemyBossUI = Instantiate (enemyPrefabUIICON);
				enemyBossUI.transform.SetParent (GameObject.FindGameObjectWithTag ("EnemyPanel").transform, false);

				enemyBossUI.transform.GetChild(0).GetComponent<Image> ().sprite = roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].enemiesList [i].enemyIcon;
			}
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

			if (roomListDungeon [dungeonIndex].RoomOfTheDungeon [index].doorList [0].doorType.ToString () == "LastDoor") {
				Debug.Log ("hey im last door");
				doorinstantiated.GetComponent<Button> ().onClick.AddListener (IndexAddingLoadMap);
			} else {
				doorinstantiated.GetComponent<Button> ().onClick.AddListener (GoDeeperInTheDungeon);
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

	IEnumerator WaitLoading(){
		yield return new WaitForSeconds (0.05f);
		if (activeScene == "Dungeon") {
			//InformationPanel = GameObject.FindGameObjectWithTag ("informationPanel");
			previousScene = "DebugMap";
		}
		if (activeScene == "Map") {
			dungeonOnTheMap = GameObject.FindGameObjectWithTag ("DungeonButtonMap").GetComponent<DungeonListOnMap> ().dungeonOnTheMapList;

			previousScene = "DebugDungeon";

			loadOnce3 = false;
			loadOnce2 = false;
			loadbutton = false;
			isUIinstantiated = false;
			loadbutton2 = false;
		}
		sceneLoaded = false;
		doOnceCoroutine = false;
	}

	public void UnlockRoom () {
		roomIsLocked = false;
		GameObject.FindGameObjectWithTag ("canvasInDungeon").SetActive (false);
		//print ("Unlock index : " + index);
	}
}
