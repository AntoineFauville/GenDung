using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entities {

    // 
    int fightIndex;
    float maxHealth;
    float health;
    int initiative;
    int attack;
    int maxActionPoint;
    int actionPoint;
    bool dead;
    string name;
    SpellObject[] entitiesSpells = new SpellObject[3];
    List<Explo_Status> entitiesStatus = new List<Explo_Status>();
    //
    GameObject entitiesGO;
    Sprite entitiesSprite;
    GameObject entitiesUIOrder;
    RuntimeAnimatorController entitiesAnimator;
    Animator entitiesEffectAnimator;

    public Entities()
    {

    }

    public Entities(float _maxHealth,int _initiative, string _name, SpellObject[] _entitiesSpells, GameObject _entitiesGO, Sprite _entitiesSprite, GameObject _entitiesUIOrder, RuntimeAnimatorController _entitiesAnimator)
    {
        this.maxHealth = _maxHealth;
        health = maxHealth;
        this.initiative = _initiative;
        dead = false;
        this.name = _name;
        this.entitiesSpells = _entitiesSpells;
        this.entitiesGO = _entitiesGO;
        this.entitiesSprite = _entitiesSprite;
        this.entitiesUIOrder = _entitiesUIOrder;
        this.entitiesAnimator = _entitiesAnimator;
    }

    public void ChangeHealth(float value, bool crit) // Damage will be negative value and heal will be positive value.
    {
        int roundedHP;
        roundedHP = Mathf.RoundToInt(value);

        health += roundedHP;

        Debug.Log("I've been hit, I lost " + Mathf.Abs(roundedHP) + " HP");

        if (health <= 0)
        {
            health = 0; // Setting health to Zero to avoid negative value.
            dead = true; // Setting player as dead.
            DeathOfEntities();
        }
    }

    public virtual void ResetActionPoints()
    {

    }

    public virtual void ChangeActionPoints(int _points)
    {

    }

    public void CreateUI()
    {
        GameObject UiBattleDisplay;
        UiBattleDisplay = MonoBehaviour.Instantiate(Resources.Load("UI_Interface/UIBattleOrderDisplay"), GameObject.Find("OrderBattlePanel").transform) as GameObject;
        this.entitiesUIOrder = UiBattleDisplay;
    }

    public virtual void DeathOfEntities()
    {
        Debug.Log("I'm Actually Dead");
        //De-activate Button
        entitiesGO.transform.Find("Background").GetComponent<Button>().enabled = false;
        //Change color to Color.gray
        entitiesGO.transform.Find("Background").GetComponent<Image>().color = Color.gray;
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

    public GameObject EntitiesGO
    {
        get
        {
            return entitiesGO;
        }

        set
        {
            entitiesGO = value;
        }
    }

    public GameObject EntitiesUIOrder
    {
        get
        {
            return entitiesUIOrder;
        }
        set
        {
            entitiesUIOrder = value;
        }
    }

    public RuntimeAnimatorController EntitiesAnimator
    {
        get
        {
            return entitiesAnimator;
        }

        set
        {
            entitiesAnimator = value;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public Sprite EntitiesSprite
    {
        get
        {
            return entitiesSprite;
        }

        set
        {
            entitiesSprite = value;
        }
    }

    public int Initiative
    {
        get
        {
            return initiative;
        }

        set
        {
            initiative = value;
        }
    }

    public SpellObject[] EntitiesSpells
    {
        get
        {
            return entitiesSpells;
        }

        set
        {
            entitiesSpells = value;
        }
    }

    public int Attack
    {
        get
        {
            return attack;
        }

        set
        {
            attack = value;
        }
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

    public int FightIndex
    {
        get
        {
            return fightIndex;
        }
        set
        {
            fightIndex = value;
        }
    }

    public List<Explo_Status> EntitiesStatus
    {
        get
        {
            return entitiesStatus;
        }

        set
        {
            entitiesStatus = value;
        }
    }

    public Animator EntitiesEffectAnimator
    {
        get
        {
            return entitiesEffectAnimator;
        }

        set
        {
            entitiesEffectAnimator = value;
        }
    }
}
