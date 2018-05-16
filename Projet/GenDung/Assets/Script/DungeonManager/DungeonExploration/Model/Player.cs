using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entities {

    public Player(float _maxHealth, int _maxActionsPoint, int _initiative, string _name, SpellObject[] _entitiesSpells, GameObject _entitiesGO, Sprite _entitiesSprite)
    {
        base.MaxHealth = _maxHealth;
        base.Health = MaxHealth;
        base.Initiative = _initiative;
        base.Dead = false;
        base.Name = _name;
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

    public override void DeathOfEntities ()
    {
        // Indicate to Explo_Data that this player is dead for the actual dungeon.
        // Change Value of players for next combat.
        // Check if all players are dead. if yes, Launch method for ending combat.

        base.DeathOfEntities(); // Need to check if it's an obligation or if it will work without calling it.


    }

    public void InitializeVisual()
    {
        base.EntitiesGO.transform.Find("EffectLayer").GetComponent<Animator>().Play("Effect_None");
        base.EntitiesGO.transform.Find("LifeControl/LifeBar").GetComponent<Image>().fillAmount = Health / MaxHealth;
    }
}