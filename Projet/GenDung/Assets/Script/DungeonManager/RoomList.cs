using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomList", menuName = "DungeonRelated/RoomList", order = 1)]
public class RoomList : ScriptableObject {

	public List<Room> RoomOfTheDungeon;
}
