using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Exploration", menuName = "ExploRelated/Explo", order = 1)]
public class ExploMap : ScriptableObject {

	public string mapName = "New Map";
	[Space(10)]
	[Header("Propreties")]
	[Space(10)]

	[Range(1, 5)]
    public int fightRoomAmount;

	[Range(1, 5)]
    public int treasureRoomAmount;

	[Range(1, 3)]
	public int enemyMax = 1;

	public List<EnemyObject> enemiesList = new List<EnemyObject>();

	[Space(10)]
	[Header("Rooms in the dungeon")]
	[Space(10)]

    public List<RoomObject> rooms = new List<RoomObject>();

	[Space(10)]
	[Header("Rewards")]
	[Space(10)]

	[Range(1, 20)]
	public int endDungeonGoldRewardMax; //to finish the dungeon the amount max

	[Range(1, 5)]
	public int chestGoldRewardMax; //for getting a chest random amount max

	[Header("Chances of getting gold opening a treasure")]

	[Range(0, 100)]
	public int percentage = 50;

	[Space(10)]
	[Header("Don't touch, editor related")]
	[Space(10)]

    public List<Vector2> movTiles = new List<Vector2>();
    public List<Vector2> eeTiles = new List<Vector2>();
}
