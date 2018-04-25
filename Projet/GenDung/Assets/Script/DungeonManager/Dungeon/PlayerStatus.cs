using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatus", menuName = "ExploRelated/PlayerStatus", order = 1)]
public class PlayerStatus : ScriptableObject {

    public string statusName;
    public int statusDamage;
    public int statusTurnTotal;
    public int statusTurnLeft;
}
