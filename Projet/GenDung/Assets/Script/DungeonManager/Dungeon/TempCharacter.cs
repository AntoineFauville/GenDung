using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TempCharacter", menuName = "ExploRelated/TempCharacter", order = 1)]
public class TempCharacter : ScriptableObject {

	public float tempHealth;

	public bool died;

    public List<PlayerStatus> playerStatus = new List<PlayerStatus>();
}
