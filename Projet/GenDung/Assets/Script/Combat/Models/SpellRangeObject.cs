using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "SpellRelated/SpellRange", order = 2)]
public class SpellRangeObject : ScriptableObject {

    public string spellRangeName;

    public int spellRangeID;

    public List<Vector2> spellRange;
}
