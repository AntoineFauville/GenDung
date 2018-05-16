using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Foe : Entities {

	public Foe(float _maxHealth, int _initiative, string _name,GameObject _gameObject, Sprite _sprite, RuntimeAnimatorController _animator)
    {
        base.MaxHealth = _maxHealth;
        base.Health = MaxHealth;
        base.Initiative = _initiative;
        base.Dead = false;
        base.Name = _name;
        base.EntitiesGO = _gameObject;
        base.EntitiesSprite = _sprite;
        base.EntitiesAnimator = _animator;
    }

    public override void DeathOfEntities()
    {
        // Change Value of foes for this combat.
        // Check if all foes are dead. if yes, Launch method for ending combat.

        base.DeathOfEntities(); // Need to check if it's an obligation or if it will work without calling it.
    }

    public void InitializeVisual()
    {
        //base.EntitiesGO.transform.SetParent(GameObject.Find("BattleSystem/BattleSystem/EnemyPanelPlacement").transform);
        //base.EntitiesGO.transform.Find("EnemyBackground").GetComponent<Image>().sprite = EntitiesSprite;
        //base.EntitiesGO.transform.Find("EffectLayer").GetComponent<Animator>().Play("Effect_None");
    }
}
