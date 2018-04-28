using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonData", menuName = "ExploRelated/DungeonData", order = 1)]
public class DungeonData : ScriptableObject {

	public List<RoomData> RoomData = new List<RoomData>();

	public int amountOfFightRoomsInData;

	public List<TempCharacter> TempFighterObject = new List<TempCharacter>();

}