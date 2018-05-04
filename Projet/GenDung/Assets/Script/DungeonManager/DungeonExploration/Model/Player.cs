using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

    float maxHealth;
    float health;
    int maxActionPoint;
    int actionPoint;
    bool dead;

    public Player(float _maxHealth, int _maxActionPoint)
    {
        this.maxHealth = _maxHealth;
        this.maxActionPoint = _maxActionPoint;
        health = this.maxHealth;
        actionPoint = this.maxActionPoint;
        dead = false;
    }
}