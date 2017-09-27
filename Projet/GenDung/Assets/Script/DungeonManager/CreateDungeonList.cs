using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateDungeonList {

	[MenuItem("Assets/Create/Dungeon List")]
	public static DungeonList  Create()
	{
	DungeonList asset = ScriptableObject.CreateInstance<DungeonList>();

		AssetDatabase.CreateAsset(asset, "Assets/DungeonList.asset");
		AssetDatabase.SaveAssets();
		return asset;
	}
}
