using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoeController : MonoBehaviour {

    private string foeName;
    private int foeID,foeHealth,foePA,foePM,foeAtk,foeMaxHealth, foeInitiative;
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
        // Indique la tile sur laquelle l'ennemi se trouve est occupé; (Penser à changer cette info quand les ennemis se déplaceront).
        
    }

    public void FoeClicked()
    {
        tileInRange = GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + pos.x + "_" + pos.y).GetComponent<TileController>().IsInRange;

        if (Input.GetMouseButtonUp(0) && CombatController.Instance.AttackMode /*&& tileInRange*/)
        {
            if (foeHealth > 0)
            {
                if (CombatController.Instance.TargetUnit.PlayerSpells[CombatController.Instance.ActualSpell].spellType == SpellObject.SpellType.CaC)
                    StartCoroutine(WaitForCaCAnimationEnd());
                else
                    StartCoroutine(WaitForAnimationEnd());
            }
        }
    }

    public void FoeDying()
    {
        dead = true;
        CombatController.Instance.MonsterNmb--;
        RemoveTileAsOccupied();
        //spriteMonster.enabled = false;
        // Désactiver le DisplayUI lié à ce monstre.
        //GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("OrderOfBattle/OrderBattlePanel/UIDisplayMonster_" + foeID).gameObject.SetActive(false)
        Destroy(GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("OrderOfBattle/OrderBattlePanel/UIDisplayMonster_" + foeID).gameObject);
        Destroy(GameObject.Find("Foe_" + foeID));
        CombatController.Instance.CheckBattleDeath();
    }

    public void SetTileAsOccupied() // Indique d'une Tile est Occupé par un Monstre.
    {
        GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + pos.x + "_" + pos.y).GetComponent<TileController>().Occupied = true;
        GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + pos.x + "_" + pos.y).GetComponent<TileController>().MonsterOnTile = this.gameObject.GetComponent<FoeController>();
    }
    public void RemoveTileAsOccupied()// Retire l'indication d'occupation d'une Tile.
    {
        GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + pos.x + "_" + pos.y).GetComponent<TileController>().Occupied = false;
        GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + pos.x + "_" + pos.y).GetComponent<TileController>().MonsterOnTile = null;
    }

    // |**| \**\ |**| /**/ 

    /* */
    public IEnumerator WaitForAnimationEnd()
    {
        CombatController.Instance.TargetUnit.Attack(CombatController.Instance.ActualSpell, Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
        //CombatController.Instance.CleanRangeAfterAttack();
        //yield return new WaitForSeconds(CombatController.Instance.TargetUnit.PlayerSpells[CombatController.Instance.ActualSpell].SpellCastAnimationTime);
        yield return new WaitForSeconds(CombatController.Instance.TargetUnit.PlayerSpells[CombatController.Instance.ActualSpell].SpellCastAnimationTime / 2);
        spriteMonster.GetComponent<Animator>().Play("DamageMonster");
        yield return new WaitForSeconds(CombatController.Instance.TargetUnit.PlayerSpells[CombatController.Instance.ActualSpell].SpellCastAnimationTime / 2);
        spriteMonster.GetComponent<Animator>().Play("IdleMonster");
        CombatController.Instance.SetTileSpellIndicator();
        foeHealth--;
        CombatController.Instance.UpdateUI(foeID);

        if (foeHealth == 0)
        {
            FoeDying();
        }
    }

    public IEnumerator WaitForCaCAnimationEnd()
    {
        CombatController.Instance.TargetUnit.Attack(CombatController.Instance.ActualSpell, Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
        //CombatController.Instance.CleanRangeAfterAttack();

        yield return new WaitForSeconds(CombatController.Instance.TargetUnit.PlayerSpells[CombatController.Instance.ActualSpell].SpellCastAnimationTime/2);
        spriteMonster.GetComponent<Animator>().Play("DamageMonster");
        yield return new WaitForSeconds(CombatController.Instance.TargetUnit.PlayerSpells[CombatController.Instance.ActualSpell].SpellCastAnimationTime / 2);
        spriteMonster.GetComponent<Animator>().Play("IdleMonster");
        CombatController.Instance.SetTileSpellIndicator();
        foeHealth--;
        CombatController.Instance.UpdateUI(foeID);

        if (foeHealth == 0)
        {
            FoeDying();
        }
    }
    /**/

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
    public int FoeInitiative
    {
        get
        {
            return foeInitiative;
        }
        set
        {
            foeInitiative = value;
        }
    }
    /*  */
}
