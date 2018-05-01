using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Spell", menuName = "SpellRelated/Spell", order =1)]
public class SpellObject : ScriptableObject {
    
    [Header("Generals")]
    [Space(5)]

    public string spellName = "New Spell";
    public int spellID;

    public Sprite spellIcon;
    public SpellRangeObject range;

    public int spellCost = 1;
    public int spellDamage = 1;

    public enum SpellType { Enemy, Ally, Self }; //to define who you can click on to activate the spell
    public SpellType spellType;
    
    public enum SpellAnimator { Attack_1, Attack_2 , Attack_3 }; //launch player animation
    public SpellAnimator spellAnimator;

    public float SpellCastAnimationTime = 1; //time of casting player animation
    
    public enum SpellTargetType { EnemySingle, EnemyAll, PlayerSingle, PlayerAll }; // to know who it is affecting
    public SpellTargetType spellTargetType;

    public GameObject spellPrefab; // extra effect on the mob
    
    public enum SpellLogicType { Damage, Heal, Buff, Debuff }; // to how it affect the stats
    public SpellLogicType spellLogicType;
    
    public enum SpellOccurenceType { NoTurn = 0, One_Turn = 1, Two_Turn = 2, Three_Turn = 3 }; // to know the occurence
    public SpellOccurenceType spellOccurenceType;
    
    // to what's the extra effect of the spell
    public enum SpellTargetFeedbackAnimationType { None, Poisonned, Healed, Sheilded, TemporaryLifed, Cursed, ResistanceReduced, AvoidanceReduced, Spike }; 
    public SpellTargetFeedbackAnimationType spellTargetFeedbackAnimationType;

	public bool EffectAppearingDuringPlayerAnim;

	public enum SpellTargetEffectAppearing { None, Spike, Roots, ProjectileVic }; 
	public SpellTargetEffectAppearing spellTargetEffectAppearing;
}
