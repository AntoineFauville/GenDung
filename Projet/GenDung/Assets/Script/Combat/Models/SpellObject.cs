using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "SpellRelated/Spell", order =1)]
public class SpellObject : ScriptableObject {

    public string spellName;

    public int spellID;

    public SpellRangeObject range;
}
