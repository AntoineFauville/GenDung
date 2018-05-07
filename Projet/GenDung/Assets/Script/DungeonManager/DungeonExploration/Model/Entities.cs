using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entities {

    float maxHealth;
    float health;
    bool dead;

    public Entities()
    {

    }

    public Entities(float _maxHealth)
    {
        this.maxHealth = _maxHealth;
        health = maxHealth;
        dead = false;
    }

    public void ChangeHealth(float value) // Damage will be negative value and heal will be positive value.
    {
        health += value;

        if (health <= 0)
        {
            health = 0; // Setting health to Zero to avoid negative value.
            dead = true; // Setting player as dead.
        }
    }

    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }

        set
        {
            maxHealth = value;
        }
    }

    public float Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }

    public bool Dead
    {
        get
        {
            return dead;
        }

        set
        {
            dead = value;
        }
    }
}
