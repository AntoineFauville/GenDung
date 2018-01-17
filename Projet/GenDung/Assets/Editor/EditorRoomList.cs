﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class EditorRoomList : EditorWindow {

	public RoomList roomList;
//	public RoomRepository roomRepository;
	private int viewIndex = 1;

	[MenuItem("Window/Dungeon Editor")]
	public static void Init () {
		GetWindow (typeof(EditorRoomList));
	}

	private void OnEnable()
	{
		if (EditorPrefs.HasKey ("RoomListPath")) {
			string objectPath = EditorPrefs.GetString ("RoomListPath");
			roomList = AssetDatabase.LoadAssetAtPath (objectPath, typeof(RoomList)) as RoomList;
		}
		//if (EditorPrefs.HasKey ("RoomRepositoryPath")) {
		//	string objectPath2 = EditorPrefs.GetString ("RoomRepositoryPath");
		//	roomRepository = AssetDatabase.LoadAssetAtPath (objectPath2, typeof(RoomRepository)) as RoomRepository;
		//}
	}

	private void OnGUI () {
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Dungeon Editor", EditorStyles.boldLabel);
		GUILayout.EndHorizontal ();

		GUILayout.Space (20);

		GUILayout.BeginHorizontal ();
		if (roomList != null) {
			if (GUILayout.Button (" Show Dungeon Asset Location ", GUILayout.ExpandWidth (false))) {
				EditorUtility.FocusProjectWindow ();
				Selection.activeObject = roomList;
			}
			if (GUILayout.Button (" Open Dungeon ", GUILayout.ExpandWidth (false))) {
				LoadRoomList ();
			}


		}
		GUILayout.EndHorizontal ();

		GUILayout.BeginHorizontal ();
		//GUILayout.Label ("If you want to save or load room from repository : ");
		//if (GUILayout.Button (" Open Dungeon Repository ", GUILayout.ExpandWidth (false))) {
		//	LoadRepository ();
		//}
		GUILayout.EndHorizontal ();

		if (roomList == null) {
			GUILayout.BeginHorizontal ();
			GUILayout.Space (10);
			if (GUILayout.Button (" Open Dungeon ", GUILayout.ExpandWidth (false))) {
				LoadRoomList ();
			}
			GUILayout.EndHorizontal ();
		}

		GUILayout.Space (20);

		if (roomList != null) {
			GUILayout.BeginHorizontal ();

			GUILayout.Space (10);

			if (GUILayout.Button ("Prev", GUILayout.ExpandWidth (false))) {
				if (viewIndex > 1)
					viewIndex--;
			}
			GUILayout.Space (5);
			if (GUILayout.Button ("Next", GUILayout.ExpandWidth (false))) {
				if (viewIndex < roomList.RoomOfTheDungeon.Count) {
					viewIndex++;
				}
			}

			GUILayout.Space (60);

			if (GUILayout.Button ("Add a Room", GUILayout.ExpandWidth (false))) {
				AddARoom ();
			}
			if (GUILayout.Button ("Delete a Room", GUILayout.ExpandWidth (false))) {
				DeleteRoom (viewIndex - 1);
			}

			GUILayout.EndHorizontal ();
			if (roomList.RoomOfTheDungeon == null)
				Debug.Log ("wtf");
			/*if (roomList.RoomOfTheDungeon.Count > 0) {
				GUILayout.BeginHorizontal ();
				viewIndex = Mathf.Clamp (EditorGUILayout.IntField ("Current Room", viewIndex, GUILayout.ExpandWidth (false)), 1, roomList.RoomOfTheDungeon.Count);

				EditorGUILayout.LabelField ("of   " + roomList.RoomOfTheDungeon.Count.ToString () + "  Room", "", GUILayout.ExpandWidth (false));
				GUILayout.EndHorizontal ();

				//-----------------Name, Type, ID -------------//
				roomList.RoomOfTheDungeon [viewIndex - 1].roomName = EditorGUILayout.TextField ("Room Name", roomList.RoomOfTheDungeon [viewIndex - 1].roomName as string);
				GUILayout.BeginHorizontal ();
				roomList.RoomOfTheDungeon [viewIndex - 1].roomID = EditorGUILayout.IntField ("Room ID", roomList.RoomOfTheDungeon [viewIndex - 1].roomID);
				/*if (GUILayout.Button ("Add a Room", GUILayout.ExpandWidth (false))) {
					SaveARoom ();
				}*/
				/*GUILayout.EndHorizontal ();
				roomList.RoomOfTheDungeon [viewIndex - 1].roomType = (Room.RoomType)EditorGUILayout.EnumPopup ("Room Type", roomList.RoomOfTheDungeon [viewIndex - 1].roomType);

				GUILayout.Space (20);



				//----------------- General Attribute of a room -------------//
				//door module//
				/*roomList.RoomOfTheDungeon [viewIndex - 1].doorList = EditorGUILayout.IntField ("Amount of door", roomList.RoomOfTheDungeon [viewIndex - 1].door);

				if (roomList.RoomOfTheDungeon [viewIndex - 1].doorList.Length != roomList.RoomOfTheDungeon [viewIndex - 1].door) {
					roomList.RoomOfTheDungeon [viewIndex - 1].doorList = new Door[roomList.RoomOfTheDungeon [viewIndex - 1].door];
				}

				for (int i = 0; i < roomList.RoomOfTheDungeon [viewIndex - 1].door; i++) {
					roomList.RoomOfTheDungeon [viewIndex - 1].doorList[i] = EditorGUILayout.ObjectField ("Door Prefab",roomList.RoomOfTheDungeon [viewIndex - 1].doorList[i] ,typeof(Door),true) as Door;
				}*/

				//roomList.RoomOfTheDungeon [viewIndex - 1].doorList[0] = EditorGUILayout.ObjectField ("Door",roomList.RoomOfTheDungeon [viewIndex - 1].doorList[0] ,typeof(GameObject),true) as GameObject;

				GUILayout.Space (10);

				//interactables module//
				//roomList.RoomOfTheDungeon [viewIndex - 1].interactables = EditorGUILayout.IntField ("Amount of interactables", roomList.RoomOfTheDungeon [viewIndex - 1].interactables);

				//if (roomList.RoomOfTheDungeon [viewIndex - 1].interactablesList.Length != roomList.RoomOfTheDungeon [viewIndex - 1].interactables) {
				//	roomList.RoomOfTheDungeon [viewIndex - 1].interactablesList = new GameObject[roomList.RoomOfTheDungeon [viewIndex - 1].interactables];
				//}

				//for (int i = 0; i < roomList.RoomOfTheDungeon [viewIndex - 1].interactables; i++) {
				//	roomList.RoomOfTheDungeon [viewIndex - 1].interactablesList[i] = EditorGUILayout.ObjectField ("Interactable Prefab",roomList.RoomOfTheDungeon [viewIndex - 1].interactablesList[i] ,typeof(GameObject),true) as GameObject;
				//}

				//roomList.RoomOfTheDungeon[viewIndex - 1].backgroundOfTheRoom[0].sprite = (Sprite)EditorGUILayout.ObjectField("Background", roomList.RoomOfTheDungeon[viewIndex - 1].backgroundOfTheRoom[0].sprite, typeof(Sprite), false);



				//----------------- Chest Room -------------//
				//if (roomList.RoomOfTheDungeon [viewIndex - 1].roomType == Room.RoomType.chest) 
				//{
				//	GUILayout.Space (10);
				/*	roomList.RoomOfTheDungeon [viewIndex - 1].chests = EditorGUILayout.IntField ("Amount of chests", roomList.RoomOfTheDungeon [viewIndex - 1].chests);

					if (roomList.RoomOfTheDungeon [viewIndex - 1].chestsList.Length != roomList.RoomOfTheDungeon [viewIndex - 1].chests) {
						roomList.RoomOfTheDungeon [viewIndex - 1].chestsList = new GameObject[roomList.RoomOfTheDungeon [viewIndex - 1].chests];
					}

					for (int i = 0; i < roomList.RoomOfTheDungeon [viewIndex - 1].chests; i++) {
						roomList.RoomOfTheDungeon [viewIndex - 1].chestsList[i] = EditorGUILayout.ObjectField ("Chest Prefab",roomList.RoomOfTheDungeon [viewIndex - 1].chestsList[i] ,typeof(GameObject),true) as GameObject;
					}
				} */



				//----------------- Fight Room -------------//
				/*if (roomList.RoomOfTheDungeon [viewIndex - 1].roomType == Room.RoomType.fight) 
				{
					GUILayout.Space (10);
					roomList.RoomOfTheDungeon [viewIndex - 1].chests = EditorGUILayout.IntField ("Amount of chests", roomList.RoomOfTheDungeon [viewIndex - 1].chests);
					if (roomList.RoomOfTheDungeon [viewIndex - 1].chestsList.Length != roomList.RoomOfTheDungeon [viewIndex - 1].chests) {
						roomList.RoomOfTheDungeon [viewIndex - 1].chestsList = new GameObject[roomList.RoomOfTheDungeon [viewIndex - 1].chests];
					}

					for (int i = 0; i < roomList.RoomOfTheDungeon [viewIndex - 1].chests; i++) {
						roomList.RoomOfTheDungeon [viewIndex - 1].chestsList[i] = EditorGUILayout.ObjectField ("Chest Prefab",roomList.RoomOfTheDungeon [viewIndex - 1].chestsList[i] ,typeof(GameObject),true) as GameObject;
					}
					GUILayout.Space (10);
				//	roomList.RoomOfTheDungeon [viewIndex - 1].enemies = EditorGUILayout.IntField ("Amount of enemies", roomList.RoomOfTheDungeon [viewIndex - 1].enemies);
				//	if (roomList.RoomOfTheDungeon [viewIndex - 1].enemiesList.Length != roomList.RoomOfTheDungeon [viewIndex - 1].enemies) {
				//		roomList.RoomOfTheDungeon [viewIndex - 1].enemiesList = new GameObject[roomList.RoomOfTheDungeon [viewIndex - 1].enemies];
				//	}

				//	for (int i = 0; i < roomList.RoomOfTheDungeon [viewIndex - 1].enemies; i++) {
				//		roomList.RoomOfTheDungeon [viewIndex - 1].enemiesList[i] = EditorGUILayout.ObjectField ("Enemy Prefab",roomList.RoomOfTheDungeon [viewIndex - 1].enemiesList[i] ,typeof(GameObject),true) as GameObject;
				//	}
				}*/



				//----------------- Boss Room -------------//
				/*if (roomList.RoomOfTheDungeon [viewIndex - 1].roomType == Room.RoomType.boss) 
				{
					GUILayout.Space (10);
					roomList.RoomOfTheDungeon [viewIndex - 1].chests = EditorGUILayout.IntField ("Amount of chests", roomList.RoomOfTheDungeon [viewIndex - 1].chests);
					if (roomList.RoomOfTheDungeon [viewIndex - 1].chestsList.Length != roomList.RoomOfTheDungeon [viewIndex - 1].chests) {
						roomList.RoomOfTheDungeon [viewIndex - 1].chestsList = new GameObject[roomList.RoomOfTheDungeon [viewIndex - 1].chests];
					}

					for (int i = 0; i < roomList.RoomOfTheDungeon [viewIndex - 1].chests; i++) {
						roomList.RoomOfTheDungeon [viewIndex - 1].chestsList[i] = EditorGUILayout.ObjectField ("Chest Prefab",roomList.RoomOfTheDungeon [viewIndex - 1].chestsList[i] ,typeof(GameObject),true) as GameObject;
					}
					GUILayout.Space (10);
					//roomList.RoomOfTheDungeon [viewIndex - 1].enemies = EditorGUILayout.IntField ("Amount of enemies", roomList.RoomOfTheDungeon [viewIndex - 1].enemies);
					//if (roomList.RoomOfTheDungeon [viewIndex - 1].enemiesList.Length != roomList.RoomOfTheDungeon [viewIndex - 1].enemies) {
					//	roomList.RoomOfTheDungeon [viewIndex - 1].enemiesList = new GameObject[roomList.RoomOfTheDungeon [viewIndex - 1].enemies];
					//}

					//for (int i = 0; i < roomList.RoomOfTheDungeon [viewIndex - 1].enemies; i++) {
					//	roomList.RoomOfTheDungeon [viewIndex - 1].enemiesList[i] = EditorGUILayout.ObjectField ("Enemy Prefab",roomList.RoomOfTheDungeon [viewIndex - 1].enemiesList[i] ,typeof(GameObject),true) as GameObject;
					//}
					GUILayout.Space (10);
					//roomList.RoomOfTheDungeon [viewIndex - 1].bossID = EditorGUILayout.TextField ("Boss ID", roomList.RoomOfTheDungeon [viewIndex - 1].bossID as string);

					//if (roomList.RoomOfTheDungeon [viewIndex - 1].bossList.Length != 1) {
					//	roomList.RoomOfTheDungeon [viewIndex - 1].bossList = new GameObject[1];
					//}
						
					//roomList.RoomOfTheDungeon [viewIndex - 1].bossList[0] = EditorGUILayout.ObjectField ("Boss Prefab",roomList.RoomOfTheDungeon [viewIndex - 1].bossList[0] ,typeof(GameObject),true) as GameObject;
				}*/

			/*} else {
				GUILayout.Label ("This Dungeon is Empty.");
			}
			if (GUI.changed) {
				EditorUtility.SetDirty (roomList);
			}*/
		}
	}

	void LoadRoomList(){
		string absPath = EditorUtility.OpenFilePanel ("Select Dungeon", "Assets/Script/DungeonManager", "");
		if (absPath.StartsWith(Application.dataPath)) 
		{
			string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
			roomList = AssetDatabase.LoadAssetAtPath (relPath, typeof(RoomList)) as RoomList;
			if (roomList.RoomOfTheDungeon == null)
				roomList.RoomOfTheDungeon = new List<Room>();
			if (roomList) {
				EditorPrefs.SetString("ObjectPath", relPath);
			}
		}
	}

	void AddARoom () {
		Room newRoom = new Room ();
		roomList.RoomOfTheDungeon.Add (newRoom);
		viewIndex = roomList.RoomOfTheDungeon.Count;
	}

	void DeleteRoom (int index)
	{
		roomList.RoomOfTheDungeon.RemoveAt (index);
	}

	/*void LoadRepository () {
		string absPath = EditorUtility.OpenFilePanel ("Select Dungeon Repository", "Assets/Script/DungeonManager", "");
		if (absPath.StartsWith(Application.dataPath)) 
		{
			string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
			roomRepository = AssetDatabase.LoadAssetAtPath (relPath, typeof(RoomRepository)) as RoomRepository;
			if (roomRepository.RoomRepositoryList == null)
				roomRepository.RoomRepositoryList = new List<Room>();
			if (roomRepository) {
				EditorPrefs.SetString("ObjectPath2", relPath);
			}
		}
	}*/

	/*void SaveARoom () {
		Room newSaveRoom = new Room ();
		/*if (roomRepository.RoomRepositoryList.Contains (newSaveRoom.roomID.Equals (roomList.RoomOfTheDungeon [viewIndex - 1].roomID))) {
			Debug.Log ("Room Allready Exist");	
		} else {
			roomRepository.RoomRepositoryList.Add (newSaveRoom);
			newSaveRoom.roomName = roomList.RoomOfTheDungeon [viewIndex - 1].roomName;
			newSaveRoom.roomID = roomList.RoomOfTheDungeon [viewIndex - 1].roomID;
			newSaveRoom.roomType = roomList.RoomOfTheDungeon [viewIndex - 1].roomType;
			newSaveRoom.backgroundOfTheRoom = roomList.RoomOfTheDungeon [viewIndex - 1].backgroundOfTheRoom;
			newSaveRoom.chests = roomList.RoomOfTheDungeon [viewIndex - 1].chests;
			newSaveRoom.door = roomList.RoomOfTheDungeon [viewIndex - 1].door;
			newSaveRoom.bossID = roomList.RoomOfTheDungeon [viewIndex - 1].bossID;
			newSaveRoom.interactables = roomList.RoomOfTheDungeon [viewIndex - 1].interactables;
			newSaveRoom.enemies = roomList.RoomOfTheDungeon [viewIndex - 1].enemies;
		//}
	}*/

	/*void LoadRoom () {
		//roomRepository.RoomRepositoryList[0].
	}*/
}
