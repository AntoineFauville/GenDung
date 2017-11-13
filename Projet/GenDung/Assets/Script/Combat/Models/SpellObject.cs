using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "SpellRelated/Spell", order =1)]
public class SpellObject : ScriptableObject {

    public string spellName = "New Spell";

    public int spellID;

    public SpellRangeObject range;

    public float SpellCastAnimationTime = 1;

    public enum SpellType { CaC, Distance };
    public SpellType spellType;

    public int spellCost = 1;
}
