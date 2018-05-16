using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entities {

    // 
    float maxHealth;
    float health;
    int initiative;
    bool dead;
    string name;
    //
    GameObject entitiesGO;
    Sprite entitiesSprite;
    GameObject entitiesUIOrder;
    RuntimeAnimatorController entitiesAnimator;

    public Entities()
    {

    }

    public Entities(float _maxHealth,int _initiative, string _name, GameObject _entitiesGO, Sprite _entitiesSprite, GameObject _entitiesUIOrder, RuntimeAnimatorController _entitiesAnimator)
    {
        this.maxHealth = _maxHealth;
        health = maxHealth;
        this.initiative = _initiative;
        dead = false;
        this.name = _name;
        this.entitiesGO = _entitiesGO;
        this.entitiesSprite = _entitiesSprite;
        this.entitiesUIOrder = _entitiesUIOrder;
        this.entitiesAnimator = _entitiesAnimator;
    }

    public void ChangeHealth(float value) // Damage will be negative value and heal will be positive value.
    {
        health += value;

        if (health <= 0)
        {
            health = 0; // Setting health to Zero to avoid negative value.
            dead = true; // Setting player as dead.
        }

        // Here, we should call a Update on UI for Health and/or Death of an Entities.
    }

    public void CreateUI()
    {
        GameObject UiBattleDisplay;
        UiBattleDisplay = MonoBehaviour.Instantiate(Resources.Load("UI_Interface/UIBattleOrderDisplay"), GameObject.Find("OrderBattlePanel").transform) as GameObject;
        this.entitiesUIOrder = UiBattleDisplay;
    }

    public virtual void DeathOfEntities()
    {
        //De-activate Button
        entitiesGO.GetComponent<Button>().enabled = false;
        //Change color to Color.gray
        entitiesGO.GetComponent<Image>().color = Color.gray;
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
}
