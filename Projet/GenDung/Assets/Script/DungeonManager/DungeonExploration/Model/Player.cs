using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entities {

    int maxActionPoint;
    int actionPoint;

    public Player(float _maxHealth, int _maxActionsPoint)
    {
        base.MaxHealth = _maxHealth;
        base.Health = MaxHealth;
        base.Dead = false;
        maxActionPoint = _maxActionsPoint;
        actionPoint = maxActionPoint;
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