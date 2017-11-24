using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementRange", menuName = "MovementRelated", order = 1)]
public class MovementRangeObject : ScriptableObject {

    public string movementRangeName;
    public int movementRangeID;
    public List<Vector2> movementRange;
}
