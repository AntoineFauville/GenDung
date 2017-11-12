using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoeController : MonoBehaviour {

    private string foeName;
    private int foeID,foeHealth,foePA,foePM,foeAtk,foeMaxHealth;

    private bool dead = false;
    private Image spriteMonster;

    public void Start()
    {
        spriteMonster = this.transform.Find("Cube/Image").GetComponent<Image>();
    }

	public void SetDefaultSpawn(Vector3 pos)
    {
        this.transform.position = pos;
    }

    public void FoeClicked()
    {
        if (Input.GetMouseButtonUp(0) && CombatController.Instance.AttackMode)
        {
            if (foeHealth > 0)
            {
                Debug.Log("Ouch, I lost 1 HP");
                foeHealth--;
                CombatController.Instance.UpdateUI(foeID);
                if(foeHealth == 0)
                {
                    dead = true;
                    CombatController.Instance.MonsterNmb--;
                    Debug.Log("This Monster Lost all is HP, Decreasing Monster Count in room");
                    spriteMonster.enabled = false;
                    CombatController.Instance.CheckBattleDeath();
                }
            }
            else if (foeHealth == 0)
            {
                Debug.Log("I'm Dead Man, Stop Clicking on me. That's Rude Man.");
            }

        }
    }

    /* Accessors Methods */
    public int FoeID
    {
        get
        {
            return foeID;
        }
        set
        {
            foeID = value;
        }
    }
    public string FoeName
    {
        get
        {
            return foeName;
        }
        set
        {
            foeName = value;
        }
    }
    public int FoeHealth
    {
        get
        {
            return foeHealth;
        }
        set
        {
            foeHealth = value;
        }
    }
    public int FoePA
    {
        get
        {
            return foePA;
        }
        set
        {
            foePA = value;
        }
    }
    public int FoePM
    {
        get
        {
            return foePM;
        }
        set
        {
            foePM = value;
        }
    }
    public int FoeAtk
    {
        get
        {
            return foeAtk;
        }
        set
        {
            foeAtk = value;
        }
    }
    public int FoeMaxHealth
    {
        get
        {
            return foeMaxHealth;
        }
        set
        {
            foeMaxHealth = value;
        }
    }
    /*  */
}
