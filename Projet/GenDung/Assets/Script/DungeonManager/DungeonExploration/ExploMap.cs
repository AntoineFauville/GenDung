using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Exploration", menuName = "ExploRelated/Explo", order = 1)]
public class ExploMap : ScriptableObject {

	public string mapName = "New Map";
    public int fightRoomAmount = 1;
    public int treasureRoomAmount = 1;
    // Room Bitch Antoine Salope
    public List<Vector2> movTiles = new List<Vector2>();
    public List<Vector2> eeTiles = new List<Vector2>();
}
