using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplorationRange", menuName = "ExploRelated/ExplorationRange", order = 3)]
public class Explo_Range : ScriptableObject {

    public List<Vector2> exploTileRange = new List<Vector2>();
}
