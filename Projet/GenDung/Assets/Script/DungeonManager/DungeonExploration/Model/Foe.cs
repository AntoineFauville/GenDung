using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foe : Entities {

	public Foe(float _maxHealth, string _name)
    {
        base.MaxHealth = _maxHealth;
        base.Health = MaxHealth;
        base.Dead = false;
        base.Name = _name;
    }

    public override void DeathOfEntities()
    {
        // Change Value of foes for this combat.
        // Check if all foes are dead. if yes, Launch method for ending combat.

        base.DeathOfEntities(); // Need to check if it's an obligation or if it will work without calling it.
    }


}
