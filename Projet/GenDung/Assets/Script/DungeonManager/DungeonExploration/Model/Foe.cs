using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Foe : Entities {

    int roomIndex;

	public Foe(float _maxHealth, int _initiative, int _attack, string _name, int _roomIndex,Explo_Data _entitiesData, GameObject _gameObject, Sprite _sprite, RuntimeAnimatorController _animator)
    {
        base.MaxHealth = _maxHealth;
        base.Health = MaxHealth;
        base.Initiative = _initiative;
        base.Attack = _attack;
        base.Dead = false;
        base.Name = _name;
        this.roomIndex = _roomIndex;
        base.EntitiesData = _entitiesData;
        base.EntitiesGO = _gameObject;
        base.EntitiesSprite = _sprite;
        base.EntitiesAnimator = _animator;
    }

    public override void DeathOfEntities()
    {
        // Change Value of foes for this combat.
        // Check if all foes are dead. if yes, Launch method for ending combat.

        base.DeathOfEntities(); // Need to check if it's an obligation or if it will work without calling it.
        this.EntitiesGO.transform.Find("Background").GetComponent<Animator>().Play("Death");
        EntitiesData.Rooms[roomIndex].MonstersAmount--;
        if (EntitiesData.Rooms[roomIndex].MonstersAmount <= 0)
            EntitiesGO.transform.parent.parent.parent.Find("ScriptBattle").GetComponent<BattleSystem>().EndBattleAllMonsterDead();
    }

    public void InitializeVisual()
    {
        base.EntitiesEffectAnimator = base.EntitiesGO.transform.Find("EffectLayer").GetComponent<Animator>();
        base.EntitiesGO.transform.Find("Background").GetComponent<Image>().sprite = EntitiesSprite;
        base.EntitiesEffectAnimator.Play("Effect_None");
        base.EntitiesGO.transform.Find("LifeControl/LifeBar").GetComponent<Image>().fillAmount = Health / MaxHealth;
        base.EntitiesGO.transform.Find("Shadow/Pastille2").GetComponent<Image>().enabled = false;
    }
}
