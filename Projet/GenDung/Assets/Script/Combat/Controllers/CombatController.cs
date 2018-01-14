using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CombatController : MonoBehaviour {

    private static CombatController instance;
    private bool spell1 = false, spell2 = false, spell3 = false;
    private int actualSpell = 99;
    private UnitController targetUnit;
    private FoeController targetFoe;
    private int monsterNmb, rndNmb;
    private MovementRangeObject movRange;
    private List<Vector2> movRangeList = new List<Vector2>();
    private int iniTurn = 0;
    private int turnCount = 0;
    private GameObject display;

    public enum combatState { Movement, Attack }
    private combatState actualCombatState;

    public enum turnType { Player,IA };
    private turnType turn;

    void CreateInstance()
    {
        if (instance != null)
        {
            Debug.Log("There should never have two combat controllers.");
        }
        instance = this;
    }

    public void Start()
    {
        CreateInstance();

        iniTurn = 0;
        turnCount = 0;

        if (SceneManager.GetActiveScene().name != "Editor")
        {
            GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().InstantiatedCombatModule = true;
        }
    }

    public void NextEntityTurn() //Fin de Tour Réel
    {
        GameObject.Find("ImageFondPassYourTurn").GetComponent<Animator>().enabled = false;
        GameObject.Find("ImageFondPassYourTurn").GetComponent<Image>().enabled = false;

        Debug.Log("End of Turn: " + turnCount);

        // Désactiver Display infos Joueur.

        display = GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("OrderOfBattle/OrderBattlePanel/UIDisplay" + PreCombatController.Instance.SortedGameobjectInit[iniTurn].transform.parent.name).gameObject as GameObject;
        display.transform.Find("BouleVerte").GetComponent<Image>().color = new Color(0, 255, 0, 0f);
        // Désactiver condition de déplacement du FoeTarget
        if (targetFoe != null)
            targetFoe.State = FoeController.foeState.Neutral;

        /* Detection tour character + reset */
        if (iniTurn >= (PreCombatController.Instance.SortedGameobjectInit.Count - 1))
        {
            iniTurn = 0;
            turnCount++;

            if (targetUnit != null)
            {
                targetUnit.ResetMove();
                targetUnit.ResetAction();
            }

            if (targetFoe != null)
            {
                for (int m = 0; m < PreCombatController.Instance.FoeData.enemiesList.Length; m++)
                {
                    try
                    {
                        targetFoe = GameObject.Find("Foe_" + m).transform.Find("Unit").GetComponent<FoeController>();
                        targetFoe.ResetMove();
                        targetFoe.ResetAction();
                    }
                    catch
                    {
                        Debug.Log("Monster number :" + m + "is dead");
                    } 
                }
            }
        }
        else
            iniTurn++;
        /* */

        display = GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("OrderOfBattle/OrderBattlePanel/UIDisplay" + PreCombatController.Instance.SortedGameobjectInit[iniTurn].transform.parent.name).gameObject as GameObject;
        display.transform.Find("BouleVerte").GetComponent<Image>().color = new Color(0, 255, 0, 1f);


        // Detection si Player ou Ennemi
        if (PreCombatController.Instance.SortedGameobjectInit[iniTurn].transform.parent.name.Contains("Foe"))
        {
            turn = turnType.IA;
            targetFoe = PreCombatController.Instance.SortedGameobjectInit[iniTurn].GetComponent<FoeController>();
            //targetFoe.State = FoeController.foeState.Movement; // Active le déplacement de l'ennemi


            CombatUIController.Instance.MonsterTurnButton();

            GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/ActualPlayerPanel").GetComponent<CanvasGroup>().alpha = 0.5f;

            if (targetUnit != null)
            {
                targetFoe.SetAttackTiles(targetUnit);
                targetFoe.SetTargetIntel(targetUnit.TileX, targetUnit.TileY);
            }
            else
            {
                UnitController tempTarget = GameObject.Find("Character_0").transform.Find("Unit").GetComponent<UnitController>();
                targetFoe.SetTargetIntel(tempTarget.TileX, tempTarget.TileY);
            }

            if (targetFoe.State == FoeController.foeState.Movement)
            {
                targetFoe.CalculatePath();
                StartCoroutine(WaitForEnemyEndTurn());
            }
                
        }
        else
        {
            turn = turnType.Player;
            targetUnit = PreCombatController.Instance.SortedGameobjectInit[iniTurn].GetComponent<UnitController>();
            StartCoroutine(WaitForEndTurn());

            CombatUIController.Instance.PlayerTurnButton();

            GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/ActualPlayerPanel").GetComponent<CanvasGroup>().alpha = 1f;
            /* */
        }

    }

    public void SpellUsable(float rmnPA) // TODO: A régler plus tard ( Complexification Système stats)
    {
        for (int i = 1; i < 4; i++)
        {
            if(rmnPA < targetUnit.PlayerSpells[i-1].spellCost)
            {
                GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/Spells/Panel/Button_Spell_"+ i).GetComponent<Button>().interactable = false;
                GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/Spells/Panel/Button_Spell_" + i).GetComponent<CanvasGroup>().alpha = 0.5f;
            }
        }
    }

    public void RemoveDeadCharacter(string s)
    {
        if (iniTurn == (PreCombatController.Instance.SortedGameobjectInit.Count - 1))
            iniTurn--;

        for (int i = 0; i < PreCombatController.Instance.SortedGameobjectInit.Count; i++)
        {
            Debug.Log(PreCombatController.Instance.SortedGameobjectInit[i].transform.parent.name);
            if (PreCombatController.Instance.SortedGameobjectInit[i].transform.parent.name == s)
            {
                Debug.Log(PreCombatController.Instance.SortedGameobjectInit[i].transform.parent.name + "Has been Removed");
                PreCombatController.Instance.SortedGameobjectInit.RemoveAt(i);
            }
            else
                Debug.Log("Sodomite Allemand!!!");
        }
    }

    /* Code de gestion du Mode Attaque ou Mode Déplacement */
    public void SetMovementRangeOnGrid()
    {
        if(targetUnit != null && targetUnit.remainingMovement != 0 && PreCombatController.Instance.CombatStarted)
        {
            movRange = Resources.Load<MovementRangeObject>("MovementRange/MovementRange_0" + targetUnit.remainingMovement);

            for (int m = 0; m < movRange.movementRange.Count; m++)
            {
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (movRange.movementRange[m].x + targetUnit.TileX) + "_" + (movRange.movementRange[m].y + targetUnit.TileY)).GetComponent<TileController>().SetMovementRange();
                movRangeList.Add(new Vector2((movRange.movementRange[m].x + targetUnit.TileX), (movRange.movementRange[m].y + targetUnit.TileY)));
            }
        }

    }

    public void RemoveMovementRangeOnGrid()
    {
        if (movRange != null)
        {
            for (int m = 0; m < movRange.movementRange.Count; m++)
            {
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (movRange.movementRange[m].x + targetUnit.TileX) + "_" + (movRange.movementRange[m].y + targetUnit.TileY)).GetComponent<TileController>().RemoveRange();
            }

            movRangeList = new List<Vector2>();
        }
    }

    public void CleanActualSpellRange()
    {
        if(actualSpell != 99)
        {
            for (int i = 0; i < targetUnit.PlayerSpells[actualSpell].range.spellRange.Count; i++)
            {
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[actualSpell].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[actualSpell].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().RemoveRange();
            }
        }

    }

    public void SwitchAttackModeFirst()
    {
        if (actualCombatState == combatState.Movement && !spell1 || actualCombatState == combatState.Attack && !spell1 || actualCombatState == combatState.Attack && spell2 || actualCombatState == combatState.Attack && spell3) // Active le spell 0 
        {
            CleanActualSpellRange();

            Debug.Log("Attack Mode has been selected, Spell N°1");
            actualCombatState = combatState.Attack;
            spell1 = true;
            spell2 = false;
            spell3 = false;
            actualSpell = 0;
            // Afficher la portée sur la grille (en Rouge).

            RemoveMovementRangeOnGrid();

            // Lié la ligne du dessus avec le code du système d'Initiative. 

            for (int i = 0; i < targetUnit.PlayerSpells[0].range.spellRange.Count; i++)
            {
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[0].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[0].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().SetRange();
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[0].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[0].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().IsInRange = true;
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[0].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[0].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().S = 0;
            }
        }
        else if (actualCombatState == combatState.Attack && spell1) // Désactive le spell 0 et clean la Range. 
        {
            actualCombatState = combatState.Movement;
            spell1 = false;
            actualSpell = 99;
            // Lié la ligne du dessus avec le code du système d'Initiative. 

            for (int i = 0; i < targetUnit.PlayerSpells[0].range.spellRange.Count; i++)
            {
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[0].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[0].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().RemoveRange();
            }
            SetMovementRangeOnGrid();
        }
    }

    public void SwitchAttackModeSecond()
    {
        if (actualCombatState == combatState.Movement && !spell2 || actualCombatState == combatState.Attack && !spell2 || actualCombatState == combatState.Attack && spell1 || actualCombatState == combatState.Attack && spell3) // Active le spell 1
        {
            CleanActualSpellRange();

            Debug.Log("Attack Mode has been selected, Spell N°2");
            actualCombatState = combatState.Attack;
            spell1 = false;
            spell2 = true;
            spell3 = false;
            actualSpell = 1;
            // Afficher la portée sur la grille (en Rouge).

            RemoveMovementRangeOnGrid();

            // On récupére le personnage dont c'est le tour.
            // Lié la ligne du dessus avec le code du système d'Initiative. 

            for (int i = 0; i < targetUnit.PlayerSpells[1].range.spellRange.Count; i++) 
            {
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[1].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[1].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().SetRange();
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[1].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[1].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().IsInRange = true;
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[1].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[1].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().S = 1;
            }
        }
        else if (actualCombatState == combatState.Attack && spell2) // Désactive le spell 1 et clean la Range. 
        {
            actualCombatState = combatState.Movement;
            spell2 = false;
            actualSpell = 99;
            // Lié la ligne du dessus avec le code du système d'Initiative. 

            for (int i = 0; i < targetUnit.PlayerSpells[1].range.spellRange.Count; i++)
            {
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[1].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[1].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().RemoveRange();
            }
            SetMovementRangeOnGrid();
        }
    }

    public void SwitchAttackModeThird()
    {
        if (actualCombatState == combatState.Movement && !spell3 || actualCombatState == combatState.Attack && !spell3 || actualCombatState == combatState.Attack && spell1 || actualCombatState == combatState.Attack && spell2) // Active le spell 2
        {
            CleanActualSpellRange();

            Debug.Log("Attack Mode has been selected, Spell N°3");
            actualCombatState = combatState.Attack;
            spell1 = false;
            spell2 = false;
            spell3 = true;
            actualSpell = 2;
            // Afficher la portée sur la grille (en Rouge).

            RemoveMovementRangeOnGrid();

            // On récupére le personnage dont c'est le tour.
            // Lié la ligne du dessus avec le code du système d'Initiative. 

            for (int i = 0; i < targetUnit.PlayerSpells[2].range.spellRange.Count; i++)
            {
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[2].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[2].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().SetRange();
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[2].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[2].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().IsInRange = true;
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[2].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[2].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().S = 2;
            }
        }
        else if (actualCombatState == combatState.Attack && spell3) // Désactive le spell 2 et clean la Range. 
        {
            actualCombatState = combatState.Movement;
            spell3 = false;
            actualSpell = 99;
            // On récupére le personnage dont c'est le tour.
            // Lié la ligne du dessus avec le code du système d'Initiative. 

            for (int i = 0; i < targetUnit.PlayerSpells[2].range.spellRange.Count; i++)
            {
                GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[2].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[2].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().RemoveRange();
            }
            SetMovementRangeOnGrid();
        }
    }

    public void CleanRangeAfterAttack()
    {
        // On récupére le personnage dont c'est le tour.
        // si on clique sur une case en dehors de la Range, voir pour faire correspondre le S avec le bon bouton.

        for (int i = 0; i < targetUnit.PlayerSpells[actualSpell].range.spellRange.Count; i++)
        {
            GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[actualSpell].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[actualSpell].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().RemoveRange();
            StartCoroutine(WaitForAttackToEnd(i));
            GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[actualSpell].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[actualSpell].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().RemoveRange();
        }

        spell1 = false;
        spell2 = false;
        spell3 = false;
    }

    public void SetTileSpellIndicator()
    {
        for (int i = 0; i < targetUnit.PlayerSpells[actualSpell].range.spellRange.Count; i++)
        {
            GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[actualSpell].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[actualSpell].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().S = 99;
        }
        spell1 = false;
        spell2 = false;
        spell3 = false;
        SetMovementRangeOnGrid();
        //actualSpell = 99;
    }

    /* Code de gestion du Combat */

    public void UpdateUI(int id)
    {
        GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("OrderOfBattle/OrderBattlePanel/UIDisplayFoe_"+id).transform.Find("PVOrderDisplay").GetComponent<Image>().fillAmount = ((float)GameObject.Find("Foe_"+id).transform.Find("Unit").GetComponent<FoeController>().FoeHealth / (float)GameObject.Find("Foe_" + id).transform.Find("Unit").GetComponent<FoeController>().FoeMaxHealth);
    }

    public void CheckBattleDeath()
    {
        if (monsterNmb <= 0)
        {
            Debug.Log("All Monsters are dead, Combat will finish soon");
            StartCoroutine(WaitBeforeEndBattle());
        }
    }
    /**/
    /*IEnumerator Methods*/

    public IEnumerator WaitForAttackToEnd(int i)
    {
        //Debug.Log("Launch Wait Before inRange Get back to False");
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + (targetUnit.PlayerSpells[actualSpell].range.spellRange[i].x + targetUnit.TileX) + "_" + (targetUnit.PlayerSpells[actualSpell].range.spellRange[i].y + targetUnit.TileY)).GetComponent<TileController>().IsInRange = false;
    }

    public IEnumerator WaitBeforeEndBattle()
    {
        yield return new WaitForSeconds(1f);
        PostCombatController.Instance.EndBattle();
    }

    public IEnumerator WaitForEndTurn()
    {
        Debug.Log("Simulating Foe Turn");
        GameObject.Find("YourTurnPanel/Panel").GetComponent<Animator>().Play("yourturngo");
        GameObject.Find("TextYourTurn").GetComponent<Text>().text = PreCombatController.Instance.SortedGameobjectInit[iniTurn].transform.parent.name;
        yield return new WaitForSeconds(2f);

        //ajout de l'animation de ton tour
        //GameObject.Find("YourTurnPanel/Panel").GetComponent<Animator>().Play("yourturngo");
        //GameObject.Find("TextYourTurn").GetComponent<Text>().text = "YOUR TURN";

        CombatController.Instance.SetMovementRangeOnGrid();
    }

    public IEnumerator WaitForEnemyEndTurn()
    {
        Debug.Log("Simulating Foe Turn");
        GameObject.Find("YourTurnPanel/Panel").GetComponent<Animator>().Play("yourturngo");
        GameObject.Find("TextYourTurn").GetComponent<Text>().text = PreCombatController.Instance.SortedGameobjectInit[iniTurn].transform.parent.name;
        yield return new WaitForSeconds(2f);

        //ajout de l'animation de ton tour
        //GameObject.Find("YourTurnPanel/Panel").GetComponent<Animator>().Play("yourturngo");
        //GameObject.Find("TextYourTurn").GetComponent<Text>().text = "YOUR TURN";

        CombatController.Instance.SetMovementRangeOnGrid();
        NextEntityTurn();
    }

    /* Accessors Methods */
    public static CombatController Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }
    public int MonsterNmb
    {
        get
        {
            return monsterNmb;
        }
        set
        {
            monsterNmb = value;
        }
    }
    public UnitController TargetUnit
    {
        get
        {
            return targetUnit;
        }
        set
        {
            targetUnit = value;
        }
    }

    public FoeController TargetFoe
    {
        get
        {
            return targetFoe;
        }
        set
        {
            targetFoe = value;
        }
    }
    public int ActualSpell
    {
        get
        {
            return actualSpell;
        }
        set
        {
            actualSpell = value;
        }
    }
    public combatState ActualCombatState
    {
        get
        {
            return actualCombatState;
        }
        set
        {
            actualCombatState = value;
        }
    }

    public turnType Turn
    {
        get
        {
            return turn;
        }
        set
        {
            turn = value;
        }
    }
    /**/
}