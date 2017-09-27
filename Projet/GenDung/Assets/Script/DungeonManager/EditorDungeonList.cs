using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorDungeonManager : EditorWindow {

	public DungeonList dungeonList;
	private int viewIndex = 1;

	[MenuItem("Window/Editor Dungeon List")]
	public static void Init () {
		GetWindow (typeof(EditorDungeonManager));
	}
		
	private void OnEnable()
	{
		if (EditorPrefs.HasKey ("DungeonListPath")) {
			string objectPath = EditorPrefs.GetString ("DungeonListPath");
			dungeonList = AssetDatabase.LoadAssetAtPath (objectPath, typeof(DungeonList)) as DungeonList;
		}
	}

	private void OnGUI () {
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Dungeon List Editor", EditorStyles.boldLabel);
		GUILayout.EndHorizontal ();

		GUILayout.Space(20);

		GUILayout.BeginHorizontal ();
		if(dungeonList != null){
			if (GUILayout.Button (" Show Dungeon List Asset Location ", GUILayout.ExpandWidth(false))) {
				EditorUtility.FocusProjectWindow ();
				Selection.activeObject = dungeonList;
			}
			if (GUILayout.Button (" Open Dungeon List ", GUILayout.ExpandWidth(false))) 
			{
				LoadList();
			}
		}

		GUILayout.EndHorizontal ();

		if (dungeonList == null) {
			GUILayout.BeginHorizontal ();
			GUILayout.Space(10);
			if (GUILayout.Button(" Open DungeonList List ", GUILayout.ExpandWidth(false))) 
			{
				LoadList();
			}
			GUILayout.EndHorizontal ();
		}

		GUILayout.Space(20);

		if (dungeonList != null) {
			GUILayout.BeginHorizontal ();

			GUILayout.Space (10);

			if (GUILayout.Button ("Prev", GUILayout.ExpandWidth (false))) {
				if (viewIndex > 1)
					viewIndex--;
			}
			GUILayout.Space (5);
			if (GUILayout.Button ("Next", GUILayout.ExpandWidth (false))) {
				if (viewIndex < dungeonList.myDungeons.Count) {
					viewIndex++;
				}
			}

			GUILayout.Space (60);

			if (GUILayout.Button ("Add a Dungeon", GUILayout.ExpandWidth (false))) {
				AddADungeon ();
			}
			if (GUILayout.Button ("Delete a Dungeon", GUILayout.ExpandWidth (false))) {
				DeleteDungeon (viewIndex - 1);
			}

			GUILayout.EndHorizontal ();
			if ( dungeonList.myDungeons == null)
				Debug.Log ("wtf");
			if ( dungeonList.myDungeons.Count > 0) {
				GUILayout.BeginHorizontal ();
				viewIndex = Mathf.Clamp (EditorGUILayout.IntField ("Current Item", viewIndex, GUILayout.ExpandWidth (false)), 1, dungeonList.myDungeons.Count);

				EditorGUILayout.LabelField ("of   " + dungeonList.myDungeons.Count.ToString () + "  Dungeons", "", GUILayout.ExpandWidth (false));
				GUILayout.EndHorizontal ();

				dungeonList.myDungeons [viewIndex - 1].dungeonName = EditorGUILayout.TextField ("Dungeon Name", dungeonList.myDungeons [viewIndex - 1].dungeonName as string);
				dungeonList.myDungeons [viewIndex - 1].dungeonType = (DungeonManager.DungeonType)EditorGUILayout.EnumPopup ("Dungeon Type", dungeonList.myDungeons [viewIndex - 1].dungeonType);
				//dungeonList.myDungeons [viewIndex - 1].RoomList = EditorGUILayout.ObjectField ("Room List", typeof(Object));

			} else {
				GUILayout.Label ("This World is Empty.");
			}
		}
	}

	void LoadList(){
		string absPath = EditorUtility.OpenFilePanel ("Select Dungeon List", "Assets/Script/DungeonManager", "");
		if (absPath.StartsWith(Application.dataPath)) 
		{
			string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
			dungeonList = AssetDatabase.LoadAssetAtPath (relPath, typeof(DungeonList)) as DungeonList;
			if (dungeonList.myDungeons == null)
				dungeonList.myDungeons = new List<DungeonManager>();
			if (dungeonList) {
				EditorPrefs.SetString("ObjectPath", relPath);
			}
		}
	} 

	void AddADungeon () {
		DungeonManager newDung = new DungeonManager ();
		dungeonList.myDungeons.Add (newDung);
		viewIndex = dungeonList.myDungeons.Count;
	}

	void DeleteDungeon (int index)
	{
		dungeonList.myDungeons.RemoveAt (index);
	}
}
