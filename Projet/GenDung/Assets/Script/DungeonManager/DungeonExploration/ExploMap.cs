using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ExploMap {

	public string roomName = "New Map";

    public List<Vector2> movTiles = new List<Vector2>();
    public List<Vector2> eeTiles = new List<Vector2>();
}
