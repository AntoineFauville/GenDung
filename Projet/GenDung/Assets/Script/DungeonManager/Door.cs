using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Door {

	public string doorName = "Imput a door name";

	public int connectingTo; // witch room is it connecting to.

	public Vector2 coordinate;

	public Sprite doorSprite;

}
