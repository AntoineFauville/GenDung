﻿using System.Collections;
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

    private FoeController monsterOnTile;
    private UnitController playerOnTile;

    public void TileClicked()
    {
        if (SceneManager.GetActiveScene().name != "Editor")
        {
            if (Input.GetMouseButtonUp(0) && CombatController.Instance.PlacementDone && CombatController.Instance.CombatStarted && !CombatController.Instance.AttackMode && !clicked && !occupied)
            {
                MoveTo();
            }
            else if (Input.GetMouseButtonUp(0) && CombatController.Instance.PlacementDone && CombatController.Instance.CombatStarted && CombatController.Instance.AttackMode && isInRange)
            {
                CombatController.Instance.CleanRangeAfterAttack();
                if (monsterOnTile != null && occupied && CombatController.Instance.TargetUnit.CheckPA())
                    monsterOnTile.FoeClicked();
                else
                    CombatController.Instance.TargetUnit.Attack(s, x, y);
                UpdateTileUI();
                CombatController.Instance.SetTileSpellIndicator();
            }
            else if (Input.GetMouseButtonUp(0) && CombatController.Instance.PlacementDone && CombatController.Instance.CombatStarted && CombatController.Instance.AttackMode && !isInRange)
            {
                CombatController.Instance.CleanRangeAfterAttack();
                CombatController.Instance.AttackMode = false;
                UpdateTileUI();
            }
            else if (!CombatController.Instance.PlacementDone) // Check si le placement Pré-Combat du personnage est deja fait.
            {
                if (Input.GetMouseButtonUp(0) && CheckSpawnType()) // Vérifie le click gauche ainsi que le fait que la Tile doit être de type Spawn Point.
                {
                    CombatController.Instance.ConfirmCharaPosition(x, y);
                    UpdateTileUI();
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (Input.GetKey(KeyCode.LeftControl))
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
                if (Input.GetKey(KeyCode.LeftControl))
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
    }

    public void Start()
    {
        TileExit();

        if(CheckSpawnType())
            this.GetComponent<Image>().color = new Color(0, 255, 255, 0.4f);
    }

    public void TileEnter()
    {
        if (SceneManager.GetActiveScene().name == "Editor" && !EditorController.Instance.CheckWall(x,y))
            this.GetComponent<Image>().color = new Color(0, 255, 0, 0.4f); // green
        else if (CheckSpawnType() && !CombatController.Instance.CombatStarted)
            this.GetComponent<Image>().color = new Color(0, 255, 255, 0.4f); // cyan
        else
            this.GetComponent<Image>().color = new Color(255,255,0, 0.6f);
    }       

    public void TileExit()
    {
        UpdateTileUI();
    }

    public void UpdateTileUI()
    {
        if (SceneManager.GetActiveScene().name == "Editor" && EditorController.Instance.CheckWall(x, y))
            this.GetComponent<Image>().color = new Color(255, 0, 0, 0.4f); // red
        else if (SceneManager.GetActiveScene().name == "Editor" && EditorController.Instance.CheckSpawn(x, y))
            this.GetComponent<Image>().color = new Color(0, 255, 255, 0.4f);// cyan
        else if (CheckSpawnType() && !CombatController.Instance.CombatStarted)
            this.GetComponent<Image>().color = new Color(0, 255, 255, 0.4f);// cyan
        else if (SceneManager.GetActiveScene().name == "Editor" && EditorController.Instance.CheckMonsterSpawn(x, y))
            this.GetComponent<Image>().color = new Color(255, 0, 255, 0.4f); // magenta
        else if (isInRange && CombatController.Instance.AttackMode)
            this.GetComponent<Image>().color = new Color(255, 0, 0, 0.4f); // red
        else
            this.GetComponent<Image>().color = new Color(255, 255, 255, 0); // Transparent
    }

    public void MoveTo()
    {
        if (!occupied)
        {
            StartCoroutine(WaitAfterClick());
            DungeonController.Instance.WorldPosTemp = this.transform.position;
            DungeonController.Instance.GeneratePathTo(x, y);
        }
        else if (occupied)
            Debug.Log("This tile has already someone on it, this isn't Tetris man!");
    }

    public bool CheckSpawnType()
    {
        if (DungeonController.Instance.Dungeon.Tiles[x, y].isStarterTile && CombatController.Instance.PlacementDone == false)
            return true;
        else
            return false;        
    }

    public void SetRange()
    {
        RemoveRange();
        this.GetComponent<Image>().color = new Color(255, 0, 0, 0.4f); // red
    }

    public void RemoveRange()
    {
        this.GetComponent<Image>().color = new Color(255, 255, 255, 0);
    }

    /* IEnumerator Methods */
    public IEnumerator WaitAfterClick()
    {
        clicked = true;
        yield return new WaitForSeconds(0.1f);
        clicked = false;
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
