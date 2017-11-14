using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoeController : MonoBehaviour {

    private string foeName;
    private int foeID,foeHealth,foePA,foePM,foeAtk,foeMaxHealth;
    private Vector2 pos; // Actual tile position where the monster stand.
    private bool dead = false;
    private Image spriteMonster;
    private bool tileInRange;

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
        tileInRange = GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + pos.x + "_" + pos.y).GetComponent<TileController>().IsInRange;

        if (Input.GetMouseButtonUp(0) && CombatController.Instance.AttackMode && tileInRange)
        {
            if (foeHealth > 0)
            {
                CombatController.Instance.CleanRangeAfterAttack(GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + pos.x + "_" + pos.y).GetComponent<TileController>().S);
                CombatController.Instance.TargetUnit.Attack();
                foeHealth--;
                CombatController.Instance.UpdateUI(foeID);
                if(foeHealth == 0)
                {
                    dead = true;
                    CombatController.Instance.MonsterNmb--;
                    spriteMonster.enabled = false;
                    // Désactiver le DisplayUI lié à ce monstre.
                    GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("OrderOfBattle/OrderBattlePanel/UIDisplayMonster_" + foeID).gameObject.SetActive(false);
                    CombatController.Instance.CheckBattleDeath();
                }
            }
        }
        else if (!CombatController.Instance.AttackMode)
        {
            Debug.Log("Did you try to walk on this monster because I'm not sure it will kill without the attack mode");
        }
        else if (!tileInRange)
        {
            Debug.Log("Monster not in Range, forget about attacking him");
            Debug.Log("Monster is on the Tile: " + pos.x + "," + pos.y);
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
    public Vector2 Pos
    {
        get
        {
            return pos;
        }
        set
        {
            pos = value;
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
