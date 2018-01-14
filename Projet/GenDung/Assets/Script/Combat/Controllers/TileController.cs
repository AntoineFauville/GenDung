using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileController : MonoBehaviour {

    private int x, y, s = 99; // x et y pour la position de la tile; s pour le numéro du sort.

    private bool clicked = false; // vérifie si on vient de cliquer sur la tile.
    private bool occupied = false; // Vérifie la présence d'un personnage sur la case.
    private bool isInRange = false; // vérifie si la case est à portée d'attaque;
    private bool checkMouse = false;

    private FoeController monsterOnTile;
    private UnitController playerOnTile;

    public void Start()
    {
        if (CheckSpawnType())
            GridController.Instance.Grid.Tiles[x, y].state = Tile.TileState.Spawn;

        StartCoroutine(WaitBeforeCleanUp(0f)); // Look for White Tiles at the beginning 
    }

    public void TileClicked()
    {
        if (SceneManager.GetActiveScene().name != "Editor")
        {

            if (Input.GetMouseButtonUp(0) && PreCombatController.Instance.PlacementDone && PreCombatController.Instance.CombatStarted && CombatController.Instance.ActualCombatState == CombatController.combatState.Movement && !clicked && GridController.Instance.Grid.Tiles[x, y].Type == Tile.TileType.Floor && GridController.Instance.Grid.Tiles[x,y].state == Tile.TileState.Movement &&  CombatController.Instance.Turn == CombatController.turnType.Player)
            {
                MoveTo();
            }
            else if (Input.GetMouseButtonUp(0) && PreCombatController.Instance.PlacementDone && PreCombatController.Instance.CombatStarted && CombatController.Instance.ActualCombatState == CombatController.combatState.Attack && isInRange && !clicked && CombatController.Instance.Turn == CombatController.turnType.Player)
            {
                if (monsterOnTile != null && GridController.Instance.Grid.Tiles[x, y].Type == Tile.TileType.Occupied && CombatController.Instance.TargetUnit.CheckPA())
                {
                    Debug.Log("Ennemy on Tile");
                    monsterOnTile.FoeClicked();                }
                else
                {
                    Debug.Log("No Ennemy");
                    CombatController.Instance.TargetUnit.Attack(s, x, y);
                }

                CombatController.Instance.SetTileSpellIndicator();
                CombatController.Instance.SetMovementRangeOnGrid();
            }
            else if (Input.GetMouseButtonUp(0) && PreCombatController.Instance.PlacementDone && PreCombatController.Instance.CombatStarted && CombatController.Instance.ActualCombatState == CombatController.combatState.Attack && !isInRange && !clicked)
            {
                CombatController.Instance.CleanRangeAfterAttack();
                CombatController.Instance.ActualCombatState = CombatController.combatState.Movement;
            }
            else if (!PreCombatController.Instance.PlacementDone) // Check si le placement Pré-Combat du personnage est deja fait.
            {
                if (Input.GetMouseButtonUp(0) && CheckSpawnType()) // Vérifie le click gauche ainsi que le fait que la Tile doit être de type Spawn Point.
                {
                    PreCombatController.Instance.ConfirmCharaPosition(x, y);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
                {
                    Debug.Log("Ctrl and shift are hold and Mouse click 0 detected");
                    EditorController.Instance.AddMovementRange(x, y);
                    this.GetComponent<Image>().color = Color.green;
                }
                else if (Input.GetKey(KeyCode.LeftControl))
                {
                    Debug.Log("Ctrl is hold and Mouse 0 click detected");
                    EditorController.Instance.AddSpawn(x,y);
                    this.GetComponent<Image>().color = Color.cyan;
                }
                else if (Input.GetKey(KeyCode.LeftAlt))
                {
                    Debug.Log("Alt is hold and Mouse 0 click detected");
                    EditorController.Instance.AddMonsterSpawn(x, y);
                    this.GetComponent<Image>().color = Color.magenta;
                }
                else if (Input.GetKey(KeyCode.LeftShift))
                {
                    Debug.Log("Shift is hold and Mouse 0 click detected");
                    EditorController.Instance.AddSpellRange(x,y);
                }
                else
                {
                    EditorController.Instance.AddWall(x, y);
                    this.GetComponent<Image>().color = Color.red;
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
                {
                    Debug.Log("Ctrl and shift are hold and Mouse click 1 detected");
                    EditorController.Instance.RemoveMovementRange(x, y);
                    this.GetComponent<Image>().color = new Color(255, 255, 255, 0.1f);
                }
                else if (Input.GetKey(KeyCode.LeftControl))
                {
                    Debug.Log("Ctrl is hold and Mouse 1 click detected");
                    EditorController.Instance.RemoveSpawn(x, y);
                    this.GetComponent<Image>().color = new Color(255, 255, 255, 0.1f);
                }
                else if (Input.GetKey(KeyCode.LeftAlt))
                {
                    Debug.Log("Alt is hold and Mouse 1 click detected");
                    EditorController.Instance.RemoveMonsterSpawn(x, y);
                    this.GetComponent<Image>().color = new Color(255, 255, 255, 0.1f);
                }
                else if (Input.GetKey(KeyCode.LeftShift))
                {
                    Debug.Log("Shift is hold and Mouse 1 click detected");
                    EditorController.Instance.RemoveSpellRange(x, y);
                }
                else
                {
                    EditorController.Instance.RemoveWall(x, y);
                    this.GetComponent<Image>().color = new Color(255, 255, 255, 0.1f);
                }
            }

        }

        StartCoroutine(WaitAfterClick());

    }

    public void UpdateTileUI()
    {
        switch (GridController.Instance.Grid.Tiles[x, y].state)
        {
            case Tile.TileState.Neutral:
                this.GetComponent<Image>().color = new Color(255, 255, 255, 0); // Transparent
                break;
            case Tile.TileState.Movement:
                this.GetComponent<Image>().color = new Color(0, 255, 0, 0.4f); // green
                break;
            case Tile.TileState.Range:
                this.GetComponent<Image>().color = new Color(255, 0, 0, 0.4f); // red
                break;
            case Tile.TileState.Wall: // Editor
                this.GetComponent<Image>().color = new Color(255, 0, 0, 0.4f); // red
                break;
            case Tile.TileState.Spawn: // Editor && Play
                this.GetComponent<Image>().color = new Color(0, 255, 255, 0.4f); // cyan
                break;
            case Tile.TileState.MonsterSpawn: // Editor
                this.GetComponent<Image>().color = new Color(255, 0, 255, 0.4f); // magenta
                break;
        }
    }

    public void MoveTo()
    {
        if (/*!occupied && */GridController.Instance.Grid.Tiles[x, y].Type != Tile.TileType.Occupied)
        {
            GridController.Instance.WorldPosTemp = this.transform.position;
            GridController.Instance.GeneratePathTo(x, y);
        }
        else if (GridController.Instance.Grid.Tiles[x, y].Type == Tile.TileType.Occupied)
            Debug.Log("This tile has already someone on it, this isn't Tetris man!");
    }

    public bool CheckSpawnType()
    {
        if (GridController.Instance.Grid.Tiles[x, y].isStarterTile && PreCombatController.Instance.PlacementDone == false)
            return true;
        else
            return false;        
    }

    public void SetRange()
    {
        RemoveRange();
        if (GridController.Instance.Grid.Tiles[x, y].isWalkable)
            GridController.Instance.Grid.Tiles[x, y].state = Tile.TileState.Range;
    }

    public void RemoveRange()
    {
        GridController.Instance.Grid.Tiles[x, y].state = Tile.TileState.Neutral;
    }

    public void SetMovementRange()
    {
        RemoveRange();
        if (GridController.Instance.Grid.Tiles[x, y].Type == Tile.TileType.Floor) // On vérifie si isWalkable est vrai
            GridController.Instance.Grid.Tiles[x, y].state = Tile.TileState.Movement;
    }

    /* IEnumerator Methods */
    public IEnumerator WaitAfterClick()
    {
        clicked = true;
        yield return new WaitForSeconds(0.1f);
        clicked = false;
    }
    public IEnumerator WaitAfterMouseCheck()
    {
        checkMouse = true;
        yield return new WaitForSeconds(0.5f);
        checkMouse = false;
    }
    public IEnumerator WaitBeforeCleanUp(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        UpdateTileUI();
        StartCoroutine(WaitBeforeCleanUp(0.3f));
    }
    /* */

    /* Accessors Methods */
    public int X
    {
        get
        {
            return x;
        }

        set
        {
            x = value;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }

        set
        {
            y = value;
        }
    }
    public int S
    {
        get
        {
            return s;
        }
        set
        {
            s = value;
        }
    }
    public bool Occupied
    {
        get
        {
            return occupied;
        }
        set
        {
            occupied = value;
        }
    }
    public bool IsInRange
    {
        get
        {
            return isInRange;
        }
        set
        {
            isInRange = value;
        }
    }
    public FoeController MonsterOnTile
    {
        get
        {
            return monsterOnTile;
        }
        set
        {
            monsterOnTile = value;
        }
    }
    public UnitController PlayerOnTile
    {
        get
        {
            return playerOnTile;
        }
        set
        {
            playerOnTile = value;
        }
    }
    /**/
}
