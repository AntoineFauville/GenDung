using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class DungeonLoader : MonoBehaviour {

	string activeScene; // just a checker to see what room is the actual room that we are using.

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
	didIInstantiateRoom,
	didICheckedButtons,
	didICheckDoors,
	loadOnceScene,
	loadOnce3,
	loadOnce2,
	loadOnceMapScene;

	void FixedUpdate () {
		//check activate scene
		activeScene = SceneManager.GetActiveScene ().name;

		//-----------Map gestion scene-------------//
		if (activeScene == "Map"){ 
			loadOnceMapScene = false;
			//get the buttons witch are dungeons entry
			dungeonOnTheMap = GameObject.FindGameObjectsWithTag ("DungeonButtonMap");

			//asign to the buttons the loading scene onClick
			if (dungeonOnTheMap != null) {
				for (int i = 0; i < dungeonOnTheMap.Length; i++) {
					dungeonOnTheMap [i].GetComponent<Button> ().onClick.AddListener (LoadSceneDungeon);
				}
			} else {
				Debug.Log ("I'm Null");
			}
		}


		//-----------Dungeon gestion scene-------------//
		if (activeScene == "Dungeon"){
			//check if we did instanciate once the door
			BG = GameObject.FindGameObjectWithTag ("backgroundOfRoom");
			GameObject.FindGameObjectWithTag ("informationPanel").GetComponent<Text> ().text = roomListDungeon [0].RoomOfTheDungeon [index].doorList [0].doorName;
			if(!didIInstantiateRoom) {
				LoadRoom ();
			}
		}		
	}

	//load the dungeon scene
	void LoadSceneDungeon () {
		for (int i = 0; i < 1; i++) {
			if (!loadOnceScene) {
				Debug.Log ("you have clicked once on LoadSceneDungeon Button");
				SceneManager.LoadScene (sceneDungeonDependingOnList[i]);
				loadOnceScene = true;
			}
		}
	}

	void LoadSceneMap () {
		if (!loadOnceMapScene) {
			
			loadOnceMapScene = true;

			SceneManager.LoadScene ("Map");

			didICheckedButtons = false;
			loadOnceScene = false;
			loadOnce3 = false;
			loadOnce2 = false;
			didIInstantiateRoom = false;
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
			/*foreach (Sprite backgroundOfTheRoom in roomListDungeon[0].RoomOfTheDungeon[index].backgroundOfTheRoom) {
				BG.transform.GetComponent<Image> ().sprite = roomListDungeon [0].RoomOfTheDungeon [index].backgroundOfTheRoom [0];
			}*/
			BG.transform.GetComponent<Image> ().sprite = roomListDungeon [0].RoomOfTheDungeon [index].back;

			//instantiate the door
			loadDoor();

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
