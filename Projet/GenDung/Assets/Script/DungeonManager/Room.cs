using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Room {

	public string 
	roomName = "New Room",
	roomID = "Input Number";

	public enum RoomType {empty,chest,fight,boss};
	public RoomType roomType;

	public int enemies = 0;
	public GameObject[] enemiesList;

	public int door = 0;
	public GameObject[] doorList;

	public int chests = 0;
	public GameObject[] chestsList;

	public string bossID = "0 Boss";
	public GameObject[] bossList;

	public int interactables = 0;
	public GameObject[] interactablesList;

	public Sprite backgroundOfTheRoom;
}
