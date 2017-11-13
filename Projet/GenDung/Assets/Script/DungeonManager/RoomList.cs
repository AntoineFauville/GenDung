using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomList", menuName = "DungeonRelated/RoomList", order = 1)]
public class RoomList : ScriptableObject {

	public List<Room> RoomOfTheDungeon;

    public int dungeonGold = 1;

    [Range(1, 4)]
    public int somethign;
    
    [Header("something")]
    [Space(10)]
    public int somethidddgn;


}
