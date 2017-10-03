﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class DungeonLoader : MonoBehaviour {

	string activeScene; // just a checker to see what room is the actual room that we are using.

	public GameObject roomPrefab,doorPrefab,Nextdoor,Previousdoor; 

	GameObject BG;

	public RoomList[] roomListDungeon; // this are the dungeons, 

	public GameObject[] dungeonOnTheMap;

	public string[] sceneDungeonDependingOnList;

	int index;

	public string map,dungeon;

	public bool
	didIInstantiateRoom,
	didICheckedButtons,
	didICheckDoors,
	loadOnce,
	loadOnce3,
	loadOnce2,
	loadOnce5;

	void FixedUpdate () {
		//check activate scene
		activeScene = SceneManager.GetActiveScene ().name;


		//-----------Map gestion scene-------------//
		if (activeScene == map){ 
			loadOnce5 = false;
			//get the buttons witch are dungeons entry
			dungeonOnTheMap = GameObject.FindGameObjectsWithTag ("DungeonButtonMap");

			//asign to the buttons the loading scene onClick
			if (dungeonOnTheMap != null) {
				for (int i = 0; i < dungeonOnTheMap.Length; i++) {
					dungeonOnTheMap [i].GetComponent<Button> ().onClick.AddListener (LoadSceneDungeonDependingOnTheRoom);
				}
				//dungeonOnTheMap [1].GetComponent<Button> ().onClick.AddListener (LoadSceneDungeonDependingOnTheRoom);
			} else {
				Debug.Log ("I'm Null");
			}
		}


		//-----------Map gestion scene-------------//
		if (activeScene == dungeon){
			//check if we did instanciate once the door
			BG = GameObject.FindGameObjectWithTag ("backgroundOfRoom");
			if(!didIInstantiateRoom) {
				//asign background
				LoadRoom ();

			}
			//check if we assigned the door
		/*	if (!didICheckDoors) {
				Nextdoor = GameObject.FindGameObjectWithTag ("NextRoomDoor");
				Previousdoor = GameObject.FindGameObjectWithTag ("PreviousRoomDoor");
				didICheckDoors = true;
			}
			//asign the buttons to load next room
		//	Nextdoor.GetComponent<Button> ().onClick.AddListener(GoDeeperInTheDungeon);
*/
		}		
	}

	//load the dungeon scene
	void LoadSceneDungeonDependingOnTheRoom () {
		for (int i = 0; i < 1; i++) {
			if (!loadOnce) {
				Debug.Log ("you have clicked once");
				SceneManager.LoadScene (sceneDungeonDependingOnList[i]);
				loadOnce = true;
			}
		}
	}

	void LoadSceneMap () {
		if (!loadOnce5) {
			
			loadOnce5 = true;

			SceneManager.LoadScene ("Map");

			didICheckedButtons = false;
			loadOnce = false;
			loadOnce3 = false;
			loadOnce2 = false;
			//didICheckDoors = false;
			didIInstantiateRoom = false;
		}
	}

	//load first time room function
	void LoadRoom () {
		if (!loadOnce2) {
			loadOnce2 = true;
			Debug.Log ("you have loadRoomOnce");
			Instantiate (roomPrefab);
			//index to say witch room are we using now
			index = 0;
			BG = GameObject.FindGameObjectWithTag ("backgroundOfRoom");
			/*foreach (Sprite backgroundOfTheRoom in roomListDungeon[0].RoomOfTheDungeon[index].backgroundOfTheRoom) {
				BG.transform.GetComponent<Image> ().sprite = roomListDungeon [0].RoomOfTheDungeon [index].backgroundOfTheRoom [0];
			}*/
			BG.transform.GetComponent<Image> ().sprite = roomListDungeon [0].RoomOfTheDungeon [index].back;

			//instantiate the door
			loadDoor();

			didIInstantiateRoom = true;
		}
	}
		
	void GoDeeperInTheDungeon () {
		if (!loadOnce3) {
			loadOnce3 = true;
			//switch index = switch room
			//index++;
			//Debug.Log (index);
			if (index > roomListDungeon [0].RoomOfTheDungeon.Count - 1) {
				LoadSceneMap ();
			} else {
				//look throught all the stats and asign them to object in the scene depending on the tags
				foreach (Sprite backgroundOfTheRoom in roomListDungeon[0].RoomOfTheDungeon[index].backgroundOfTheRoom) {
					BG.transform.GetComponent<Image> ().sprite = roomListDungeon [0].RoomOfTheDungeon [index].backgroundOfTheRoom [0];
				}

				loadDoor();

				//Debug.Log (roomListDungeon [0].RoomOfTheDungeon.Count);
				//didICheckDoors = false;
				StartCoroutine ("waitLagForClicking");
			}
		}
	}

	/*void GoBackInTheDungeon () {
		if (!loadOnce3) {
			loadOnce3 = true;
			//switch index = switch room
			index--;
			//Debug.Log (index);
			if (index <= 0) {
				
			} else {
				//look throught all the stats and asign them to object in the scene depending on the tags
				foreach (Sprite backgroundOfTheRoom in roomListDungeon[0].RoomOfTheDungeon[index].backgroundOfTheRoom) {
					BG.transform.GetComponent<Image> ().sprite = roomListDungeon [0].RoomOfTheDungeon [index].backgroundOfTheRoom [0];
				}
				//Debug.Log (roomListDungeon [0].RoomOfTheDungeon.Count);
				didICheckDoors = false;
				StartCoroutine ("waitLagForClicking");
			}
		}
	}*/

	IEnumerator waitLagForClicking () {
		yield return new WaitForSeconds (0.1f);
		loadOnce3 = false;
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

		doorinstantiated.transform.localPosition = new Vector3 (roomListDungeon [0].RoomOfTheDungeon [index].doorList [0].coordinate.x, roomListDungeon [0].RoomOfTheDungeon [index].doorList [0].coordinate.y,0);

		index = roomListDungeon [0].RoomOfTheDungeon [index].doorList [0].connectingTo;

		print ("You loaded room : " + roomListDungeon [0].RoomOfTheDungeon [index].doorList [0].connectingTo);

		doorinstantiated.GetComponent<Button> ().onClick.AddListener(GoDeeperInTheDungeon);
	}
}