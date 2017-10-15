using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Room", menuName = "DungeonRelated/Room", order = 2)]
public class RoomObject : ScriptableObject {

	public string roomName;

	public int roomID;

	public Sprite back;

	public ExceptionGrid[] exceptionGrid;
}
