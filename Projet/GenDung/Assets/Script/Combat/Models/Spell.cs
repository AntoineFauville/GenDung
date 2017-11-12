using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spell {

    public string spellName = "New Spell";

    public SpellObject spell;

    public enum SpellType {CaC,Distance};
    public SpellType spellType;

    public int spellCost = 0; 
		
	}
