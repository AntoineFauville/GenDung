using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorRoomList : EditorWindow {

	public RoomList roomList;
	private int viewIndex = 1;

	private GameObject door;

	private bool hasEnemies;

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
			if (roomList.RoomOfTheDungeon.Count > 0) {
				GUILayout.BeginHorizontal ();
				viewIndex = Mathf.Clamp (EditorGUILayout.IntField ("Current Room", viewIndex, GUILayout.ExpandWidth (false)), 1, roomList.RoomOfTheDungeon.Count);

				EditorGUILayout.LabelField ("of   " + roomList.RoomOfTheDungeon.Count.ToString () + "  Room", "", GUILayout.ExpandWidth (false));
				GUILayout.EndHorizontal ();

				//-----------------Name, Type, ID -------------//
				roomList.RoomOfTheDungeon [viewIndex - 1].roomName = EditorGUILayout.TextField ("Room Name", roomList.RoomOfTheDungeon [viewIndex - 1].roomName as string);
				roomList.RoomOfTheDungeon [viewIndex - 1].roomID = EditorGUILayout.TextField ("Room ID", roomList.RoomOfTheDungeon [viewIndex - 1].roomID as string);
				roomList.RoomOfTheDungeon [viewIndex - 1].roomType = (Room.RoomType)EditorGUILayout.EnumPopup ("Room Type", roomList.RoomOfTheDungeon [viewIndex - 1].roomType);

				GUILayout.Space (20);



				//----------------- General Attribute of a room -------------//
				//door module//
				roomList.RoomOfTheDungeon [viewIndex - 1].door = EditorGUILayout.IntField ("Amount of door", roomList.RoomOfTheDungeon [viewIndex - 1].door);

				if (roomList.RoomOfTheDungeon [viewIndex - 1].doorList.Length != roomList.RoomOfTheDungeon [viewIndex - 1].door) {
					roomList.RoomOfTheDungeon [viewIndex - 1].doorList = new GameObject[roomList.RoomOfTheDungeon [viewIndex - 1].door];
				}

				for (int i = 0; i < roomList.RoomOfTheDungeon [viewIndex - 1].door; i++) {
					roomList.RoomOfTheDungeon [viewIndex - 1].doorList[i] = EditorGUILayout.ObjectField ("Door",door ,typeof(GameObject),true) as GameObject;
				}

				//interactables module//
				roomList.RoomOfTheDungeon [viewIndex - 1].interactables = EditorGUILayout.IntField ("Amount of interactables", roomList.RoomOfTheDungeon [viewIndex - 1].interactables);


				//----------------- Chest Room -------------//
				if (roomList.RoomOfTheDungeon [viewIndex - 1].roomType == Room.RoomType.chest) 
				{
					roomList.RoomOfTheDungeon [viewIndex - 1].chests = EditorGUILayout.IntField ("Amount of chests", roomList.RoomOfTheDungeon [viewIndex - 1].chests);
				} 



				//----------------- Fight Room -------------//
				if (roomList.RoomOfTheDungeon [viewIndex - 1].roomType == Room.RoomType.fight) 
				{
					roomList.RoomOfTheDungeon [viewIndex - 1].chests = EditorGUILayout.IntField ("Amount of chests", roomList.RoomOfTheDungeon [viewIndex - 1].chests);
					roomList.RoomOfTheDungeon [viewIndex - 1].enemies = EditorGUILayout.IntField ("Amount of enemies", roomList.RoomOfTheDungeon [viewIndex - 1].enemies);
				}


				//----------------- Boss Room -------------//
				if (roomList.RoomOfTheDungeon [viewIndex - 1].roomType == Room.RoomType.boss) 
				{
					roomList.RoomOfTheDungeon [viewIndex - 1].chests = EditorGUILayout.IntField ("Amount of chests", roomList.RoomOfTheDungeon [viewIndex - 1].chests);
					roomList.RoomOfTheDungeon [viewIndex - 1].enemies = EditorGUILayout.IntField ("Amount of enemies", roomList.RoomOfTheDungeon [viewIndex - 1].enemies);
					roomList.RoomOfTheDungeon [viewIndex - 1].bossID = EditorGUILayout.TextField ("Boss ID", roomList.RoomOfTheDungeon [viewIndex - 1].bossID as string);
				}

			} else {
				GUILayout.Label ("This Dungeon is Empty.");
			}
			if (GUI.changed) {
				EditorUtility.SetDirty (roomList);
			}
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
}
