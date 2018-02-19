using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonListExploration", menuName = "ExploRelated/DungeonListExplo", order = 2)]
public class Explo_DungeonList : ScriptableObject
{
    public List<ExploMap> explorationDungeons = new List<ExploMap>(); 
}
