﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExploTileController : MonoBehaviour {

    private int x, y, s = 99; // x et y pour la position de la tile; s pour le numéro du sort.

    private bool clicked = false; // vérifie si on vient de cliquer sur la tile.
    private bool checkMouse = false;
    private ExploUnitController playerOnTile;
    private Sprite[] sprites;

    public void Start()
    {

        /*if (CheckSpawnType())
            GridController.Instance.Grid.Tiles[x, y].state = Tile.TileState.Spawn; */

        StartCoroutine(WaitBeforeCleanUp(0f)); // Look for White Tiles at the beginning 

        sprites = Resources.LoadAll<Sprite>("Sprites/Explo_Map");
    }

    public void TileClicked()
    {
        if (SceneManager.GetActiveScene().name != "ExploEditor") 
        {
            if (Explo_GridController.Instance.Grid.ExploTiles[x,y].Type != Explo_Tile.Explo_TileType.Wall)
            {
                Debug.Log("Checking this tile");
                MoveTo();
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Adding Movement Tiles");
                ExploEditorController.Instance.AddMovementTiles(x, y);
                UpdateTileUI();
            }
            else if (Input.GetMouseButtonUp(1))
            {
                Debug.Log("Removing Movement Tiles");
                ExploEditorController.Instance.RemoveMovementTiles(x, y);
                UpdateTileUI();
            }

            if (Input.GetMouseButtonUp(0) && Input.GetKey(KeyCode.LeftShift))
            {
                Debug.Log("Adding EE Tiles");
                ExploEditorController.Instance.AddEETiles(x, y);
                UpdateTileUI();
            }
            else if (Input.GetMouseButtonUp(1) && Input.GetKey(KeyCode.LeftShift))
            {
                Debug.Log("Removing EE Tiles");
                ExploEditorController.Instance.RemoveEETiles(x, y);
                UpdateTileUI();
            }
        }
        StartCoroutine(WaitAfterClick());

    }

    public void MoveTo()
    {
        if (Explo_GridController.Instance.Grid.ExploTiles[x,y].Type != Explo_Tile.Explo_TileType.Wall)
        {
            Debug.Log("Moving this tile");

            Explo_GridController.Instance.WorldPosTemp = this.transform.position;
            Explo_GridController.Instance.GeneratePathTo(x, y);
        }
        else
        {
            Debug.Log("Can't walk on a wall");
        }

    }

    public bool CheckSpawnType()
    {
        if (Explo_GridController.Instance.Grid.ExploTiles[x, y].isStarterTile)
            return true;
        else
            return false;
    }

    public void UpdateTileUI()
    {
        switch (Explo_GridController.Instance.Grid.ExploTiles[x, y].Type)
        {
            case Explo_Tile.Explo_TileType.Wall:
                this.GetComponent<Image>().sprite = sprites[0];
                break;
            case Explo_Tile.Explo_TileType.Empty:
                this.GetComponent<Image>().sprite = sprites[1];
                break;
            case Explo_Tile.Explo_TileType.Fight:
                this.GetComponent<Image>().sprite = sprites[4];
                break;
            case Explo_Tile.Explo_TileType.Treasure:
                this.GetComponent<Image>().sprite = sprites[6];
                break;
            case Explo_Tile.Explo_TileType.Entrance:
                if(SceneManager.GetActiveScene().name == "ExploEditor")
                    this.GetComponent<Image>().sprite = sprites[2];
                else
                    this.GetComponent<Image>().sprite = sprites[1];
                break;
            case Explo_Tile.Explo_TileType.Exit:
                this.GetComponent<Image>().sprite = sprites[5];
                break;
            case Explo_Tile.Explo_TileType.OtterKingdom:
                this.GetComponent<Image>().sprite = sprites[2];
                break;
            case Explo_Tile.Explo_TileType.Trap:
                this.GetComponent<Image>().sprite = sprites[0];
                break;
            default:
                this.GetComponent<Image>().sprite = sprites[3];
                break;
        }
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
    public ExploUnitController PlayerOnTile
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
