using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomRepository", menuName = "DungeonRelated/RoomRepository", order = 2)]
public class RoomRepository : ScriptableObject {

	public List<Room> RoomRepositoryList;
}
