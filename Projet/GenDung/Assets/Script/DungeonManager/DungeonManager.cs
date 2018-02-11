using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonManager {

	public string dungeonName;
	public enum DungeonType {Normal,Heroique};
	public DungeonType dungeonType;
	public RoomList dungeon;
}