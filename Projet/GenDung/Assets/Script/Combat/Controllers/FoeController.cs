using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FoeController : MonoBehaviour {

    private string foeName;
    private int tileX, tileY, targetTileX, targetTileY;
    private int foeID,foeHealth,foePA,foePM,foeAtk,foeMaxHealth, foeInitiative;
    public float remainingMovement = 99, remainingAction = 5;
    private Vector2 pos; // Actual tile position where the monster stand.
    private List<Node> currentPath = null; // Liste des noeuds pour le PathFinding.
    private bool dead = false;
    private Image spriteMonster;
    private bool tileInRange;

    public void Start()
    {
        spriteMonster = this.transform.Find("Cube/Image").GetComponent<Image>();
        remainingMovement = foePM;
        remainingAction = foePA;
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name != "Editor" && CombatController.Instance.CombatStarted && CombatController.Instance.Turn == CombatController.turnType.IA) // On vérifie que la scene n'est pas l'editeur et que le placement pré-combat a été réalisé.
        {
            AdvancePathing();
            transform.position = Vector3.Lerp(transform.position, GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).transform.position, 5f * Time.deltaTime);
        }
    }

    public void SetTargetIntel(int _x, int _y)
    {
        targetTileX = _x;
        targetTileY = _y;

        int diffX = (targetTileX - tileX);
        int diffY = (targetTileY - tileY);

        if (diffX > diffY)
        {
            if (diffY > 0)
            {
                targetTileY--;
            }
            else
            {
                targetTileY++;
            }
        }
        else if (diffY > diffX)
        {
            if (diffX > 0)
            {
                targetTileX--;
            }
            else
            {
                targetTileX++;
            }
        }

        Debug.Log("Target Tile for Foe Movement: (" + targetTileX + "," + targetTileY + ')');
    } 

    public void CalculatePath()
    {
        GridController.Instance.WorldPosTemp = GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + targetTileX + "_" + targetTileY).transform.position;
        GridController.Instance.GeneratePathTo(targetTileX, targetTileY);
    }

    public void AdvancePathing() // Méthode de déplacement du personnage.
    {

        if (currentPath == null) // Cas où le chemin est null, on arrete l'éxécution de la méthode.
        {
            return;
        }

        if (remainingMovement <= 0) // on vérifie si le personnage a encore des PM.
        {
            //Debug.Log("Not enough movement point left, wait for the next turn");
            return;
        }

        remainingMovement -= GridController.Instance.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y); // on retire le coût du déplacement par case.

        tileX = currentPath[1].x;
        tileY = currentPath[1].y;

        GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).GetComponent<TileController>().Occupied = true;
        GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + currentPath[0].x + "_" + currentPath[0].y).GetComponent<TileController>().Occupied = false;

        if (CombatController.Instance.CombatStarted)
            GameObject.Find("DontDestroyOnLoad").GetComponent<BuffIndicatorGestion>().GetBuffIndicator(0, 3, Mathf.RoundToInt(24), 0f);

        StartCoroutine(WaitBeforeNextMovement()); // Coroutine pour faire patienter le joueur et donné une meilleure impression de déplacement.

        currentPath.RemoveAt(0); // on retire la case précedente de la liste.

        if (currentPath.Count == 1) // on vérifie si il ne reste pas que la case de destination.
        {
            currentPath = null;
            CombatController.Instance.SetMovementRangeOnGrid();
        }
    }

    public void SetDefaultSpawn(Vector3 pos)
    {
        this.transform.position = pos;
        // Indique la tile sur laquelle l'ennemi se trouve est occupé; (Penser à changer cette info quand les ennemis se déplaceront). 
    }

    public void FoeClicked()
    {
        tileInRange = GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + pos.x + "_" + pos.y).GetComponent<TileController>().IsInRange;

        if (Input.GetMouseButtonUp(0) && CombatController.Instance.ActualCombatState == CombatController.combatState.Attack)
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
        CombatController.Instance.RemoveDeadCharacter(this.transform.parent.name);
        RemoveTileAsOccupied();
        //spriteMonster.enabled = false;
        // Désactiver le DisplayUI lié à ce monstre.
        //GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("OrderOfBattle/OrderBattlePanel/UIDisplayMonster_" + foeID).gameObject.SetActive(false)
        Destroy(GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("OrderOfBattle/OrderBattlePanel/UIDisplayFoe_" + foeID).gameObject);
        Destroy(GameObject.Find("Foe_" + foeID));
        CombatController.Instance.CheckBattleDeath();
    }

    public void SetTileAsOccupied() // Indique d'une Tile est Occupé par un Monstre.
    {
        GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + pos.x + "_" + pos.y).GetComponent<TileController>().Occupied = true;
        GridController.Instance.Grid.Tiles[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)].Type = Tile.TileType.Occupied;
        GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + pos.x + "_" + pos.y).GetComponent<TileController>().MonsterOnTile = this.gameObject.GetComponent<FoeController>();
    }
    public void RemoveTileAsOccupied()// Retire l'indication d'occupation d'une Tile.
    {
        GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + pos.x + "_" + pos.y).GetComponent<TileController>().Occupied = false;
        GridController.Instance.Grid.Tiles[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)].Type = Tile.TileType.Floor;
        GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + pos.x + "_" + pos.y).GetComponent<TileController>().MonsterOnTile = null;
    }

    public void ResetMove()
    {
        remainingMovement = foePM;
        //GameObject.Find("DontDestroyOnLoad").GetComponent<BuffIndicatorGestion>().GetBuffIndicator(0, 2, pm, 0f);
    }

    public void ResetAction()
    {
        remainingAction = foePA;
        //GameObject.Find("DontDestroyOnLoad").GetComponent<BuffIndicatorGestion>().GetBuffIndicator(0, 0, pm, 1f);
    }

    // |**| \**\ |**| /**/ 

    /* */
    public IEnumerator WaitBeforeNextMovement()
    {
        yield return new WaitForSecondsRealtime(1f);
        transform.position = Vector3.Lerp(transform.position, GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).transform.position, 5f * Time.deltaTime);
    }

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
    public int TileX
    {
        get
        {
            return tileX;
        }
        set
        {
            tileX = value;
        }
    }

    public int TileY
    {
        get
        {
            return tileY;
        }
        set
        {
            tileY = value;
        }
    }
    public List<Node> CurrentPath
    {
        get
        {
            return currentPath;
        }
        set
        {
            currentPath = value;
        }
    }
    /*  */
}
