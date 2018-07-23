using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entities {

    Explo_EndFightController endFightCtrl;

    public Player(float _maxHealth, int _maxActionsPoint, int _initiative, string _name,Explo_Data _entitiesData, SpellObject[] _entitiesSpells, GameObject _entitiesGO, Sprite _entitiesSprite)
    {
        base.MaxHealth = _maxHealth;
        base.Health = MaxHealth;
        base.Initiative = _initiative;
        base.Dead = false;
        base.Name = _name;
        base.EntitiesData = _entitiesData;
        base.EntitiesSpells = _entitiesSpells;
        base.EntitiesGO = _entitiesGO;
        base.EntitiesSprite = _entitiesSprite;
        base.MaxActionPoint = _maxActionsPoint;
        base.ActionPoint = MaxActionPoint;
    }

    public override void ResetActionPoints()
    {
        Debug.Log("My Action Points has been fully restored");
        base.ActionPoint = this.MaxActionPoint;
    }

    public override void ChangeActionPoints(int _points)
    {
        base.ActionPoint -= _points;
    }

    public override void DeathOfEntities ()
    {
        base.Dead = true;
        EntitiesData.PlayersLeft--;
        if (EntitiesData.PlayersLeft <= 0)
        {
            endFightCtrl = EntitiesGO.transform.parent.parent.Find("ScriptBattle").GetComponent<Explo_EndFightController>();
            endFightCtrl.EndBattleAllPlayerDead();
        }

        base.DeathOfEntities(); // Need to check if it's an obligation or if it will work without calling it.


    }

    public void InitializeVisual()
    {
        base.EntitiesEffectAnimator = base.EntitiesGO.transform.Find("EffectLayer").GetComponent<Animator>();
        base.EntitiesEffectAnimator.Play("Effect_None");
        base.EntitiesGO.transform.Find("LifeControl/LifeBar").GetComponent<Image>().fillAmount = Health / MaxHealth;
    }
}