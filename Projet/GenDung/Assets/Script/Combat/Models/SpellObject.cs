using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "SpellRelated/Spell", order =1)]
public class SpellObject : ScriptableObject {

    public string spellName = "New Spell";

    public int spellID;

    public Sprite spellIcon;

    public SpellRangeObject range;

    public float SpellCastAnimationTime = 1;

    public enum SpellType { CaC, Distance, Self };
    public SpellType spellType;

    public GameObject spellPrefab;

    public enum SpellAnimator { Attack_1, Attack_2 , Attack_3 };
    public SpellAnimator spellAnimator;

    public int spellCost = 1;

	public int spellDamage = 1;
}
