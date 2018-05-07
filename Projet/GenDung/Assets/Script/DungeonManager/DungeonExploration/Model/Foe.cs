using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foe : Entities {

	public Foe(float _maxHealth)
    {
        base.MaxHealth = _maxHealth;
        base.Health = MaxHealth;
        base.Dead = false;
    }
}
