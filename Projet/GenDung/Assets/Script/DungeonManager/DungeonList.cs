using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonList", menuName = "DungeonRelated/DungeonList", order = 1)]
public class DungeonList : ScriptableObject {

	public List<DungeonManager> myDungeons;
}
