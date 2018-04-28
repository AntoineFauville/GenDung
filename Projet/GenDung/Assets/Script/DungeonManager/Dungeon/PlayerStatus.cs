using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatus", menuName = "ExploRelated/PlayerStatus", order = 1)]
public class PlayerStatus : ScriptableObject {

	public enum SpellTargetFeedbackTempType { None, Poisonned, Healed, Sheilded, TemporaryLifed, Cursed, ResistanceReduced, AvoidanceReduced }; 
	public SpellTargetFeedbackTempType spellTargetFeedbackAnimationType;

    public string statusName;
    public int statusDamage;
    public int statusTurnTotal;
}
