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

	public int enemies = 0;
	public Enemy[] enemiesList;

	public int door = 0;
	public Door[] doorList;

	public int chests = 0;
	public GameObject[] chestsList;

	public Boss[] bossList;

	public int interactables = 0;
	public GameObject[] interactablesList;
}
