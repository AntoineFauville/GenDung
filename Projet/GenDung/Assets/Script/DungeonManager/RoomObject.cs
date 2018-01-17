using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Room", menuName = "DungeonRelated/Room", order = 2)]
public class RoomObject : ScriptableObject {

	public string roomName;

	public int roomID;

	public int doorAmount;
	public Door[] doorList;

	public Sprite back;

	public Vector2[] playerPositions;

    public List<Vector2> Walls;

    public List<Vector2> SpawningPoints;

    public List<Vector2> MonsterSpawningPoints;
}
