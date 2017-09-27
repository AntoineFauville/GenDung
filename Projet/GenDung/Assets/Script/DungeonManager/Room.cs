using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Room {

	public string roomName;
	public enum RoomType {empty,chest,fight,boss};
	public RoomType roomType;

	public Sprite backgroundOfTheRoom;
}
