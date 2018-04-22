using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "EnemyRelated/Enemy", order = 1)]
public class EnemyObject : ScriptableObject {

    public string
    enemyName = "New Name";

    public Sprite enemyIcon;
    public RuntimeAnimatorController enemyAnimator;
    public SpellRangeObject enemyRange;

	public bool hasAnimation;

    public int 
        health,
        pa,
        pm,
        atk,
        initiative;

	public int enemyGoldValue;
}
