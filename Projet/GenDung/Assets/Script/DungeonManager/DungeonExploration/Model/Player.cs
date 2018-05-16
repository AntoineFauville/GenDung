using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entities {

    int maxActionPoint;
    int actionPoint;

    public Player(float _maxHealth, int _maxActionsPoint, int _initiative, string _name, Sprite _entitiesSprite)
    {
        base.MaxHealth = _maxHealth;
        base.Health = MaxHealth;
        base.Initiative = _initiative;
        base.Dead = false;
        base.Name = _name;
        base.EntitiesSprite = _entitiesSprite;
        maxActionPoint = _maxActionsPoint;
        actionPoint = maxActionPoint;
    }

    public override void DeathOfEntities ()
    {
        // Indicate to Explo_Data that this player is dead for the actual dungeon.
        // Change Value of players for next combat.
        // Check if all players are dead. if yes, Launch method for ending combat.

        base.DeathOfEntities(); // Need to check if it's an obligation or if it will work without calling it.
    }

    public int MaxActionPoint
    {
        get
        {
            return maxActionPoint;
        }

        set
        {
            maxActionPoint = value;
        }
    }

    public int ActionPoint
    {
        get
        {
            return actionPoint;
        }

        set
        {
            actionPoint = value;
        }
    }
}