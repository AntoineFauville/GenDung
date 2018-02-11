using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Room {

	public string roomName = "New Room";

	public RoomObject room;

	public int number; // number of the room in the dungeon

	public enum RoomType {empty,chest,fight,boss};
	public RoomType roomType;

	public enum DoorType {regularDoor,LastDoor};
	public DoorType doorType;
	public int connectingTo = 0;

	public int enemies = 0;
    public EnemyObject[] enemiesList; 

	public int chests = 0;
	public chest[] chestsList;

	public Boss[] bossList;

	public int interactables = 0;
	public GameObject[] interactablesList;
}
