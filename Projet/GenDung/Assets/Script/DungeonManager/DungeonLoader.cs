using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class DungeonLoader : MonoBehaviour {

	string 
	activeScene;

	public GameObject roomPrefab,Nextdoor,Previousdoor;

	GameObject BG;

	public RoomList[] roomListDungeon;

	public GameObject[] dungeonOnTheMap;

	int index;

	public string map,dungeon;

	public bool
	didIInstantiateRoom,
	didICheckedButtons,
	didICheckDoors,
	loadOnce,
	loadOnce2,
	loadOnce3,
	loadOnce4,
	loadOnce5;

	void Update () {
		//check activate scene
		activeScene = SceneManager.GetActiveScene ().name;


		//-----------Map gestion scene-------------//
		if (activeScene == map){ 
			loadOnce5 = false;
			//get the buttons witch are dungeons entry
			dungeonOnTheMap = GameObject.FindGameObjectsWithTag ("DungeonButtonMap");

			//asign to the buttons the loading scene onClick
			dungeonOnTheMap [0].GetComponent<Button> ().onClick.AddListener(LoadSceneA);
			dungeonOnTheMap [1].GetComponent<Button> ().onClick.AddListener(LoadSceneA);
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
			if (!didICheckDoors) {
				Nextdoor = GameObject.FindGameObjectWithTag ("NextRoomDoor");
				didICheckDoors = true;
			}
			//asign the buttons to load next room
			Nextdoor.GetComponent<Button> ().onClick.AddListener(changeRoom);

		}		
	}

	//load the dungeon scene
	void LoadSceneA () {
		if (!loadOnce) {
			Debug.Log ("you have clicked once");
			SceneManager.LoadScene ("Dungeon");
			loadOnce = true;
		}
	}
	void LoadSceneB () {
		if (!loadOnce4) {
			Debug.Log ("you have clicked once");
			SceneManager.LoadScene ("Dungeon");
			loadOnce4 = true;
		}
	}
	void LoadSceneMap () {
		if (!loadOnce5) {
			
			loadOnce5 = true;

			SceneManager.LoadScene ("Map");

			didICheckedButtons = false;
			loadOnce = false;
			loadOnce2 = false;
			loadOnce3 = false;
			loadOnce4 = false;
			didICheckDoors = false;
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
			foreach (Sprite backgroundOfTheRoom in roomListDungeon[0].RoomOfTheDungeon[index].backgroundOfTheRoom) {
				BG.transform.GetComponent<Image> ().sprite = roomListDungeon [0].RoomOfTheDungeon [index].backgroundOfTheRoom [0];
			}
			didIInstantiateRoom = true;
		}
	}
		
	void changeRoom () {
		if (!loadOnce3) {
			loadOnce3 = true;
			//switch index = switch room
			index++;
			//Debug.Log (index);

			if (index > roomListDungeon [0].RoomOfTheDungeon.Count - 1) {
				LoadSceneMap ();
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
	}

	IEnumerator waitLagForClicking () {
		yield return new WaitForSeconds (0.1f);
		loadOnce3 = false;
	}
}
