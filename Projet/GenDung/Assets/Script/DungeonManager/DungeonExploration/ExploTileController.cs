using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExploTileController : MonoBehaviour {

    private int x, y, s = 99; // x et y pour la position de la tile; s pour le numéro du sort.

    private bool clicked = false; // vérifie si on vient de cliquer sur la tile.
    private bool checkMouse = false;
    private ExploUnitController playerOnTile;

    public void Start()
    {

        /*if (CheckSpawnType())
            GridController.Instance.Grid.Tiles[x, y].state = Tile.TileState.Spawn; */

        StartCoroutine(WaitBeforeCleanUp(0f)); // Look for White Tiles at the beginning 
    }

    public void TileClicked()
    {
        if (SceneManager.GetActiveScene().name != "ExploEditor")
        {
            if (Explo_GridController.Instance.Grid.ExploTiles[x,y].Type != Explo_Tile.Explo_TileType.Wall)
            {
                MoveTo();
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Adding Movement Tiles");
                ExploEditorController.Instance.AddMovementTiles(x, y);
            }
            else if (Input.GetMouseButtonUp(1))
            {
                Debug.Log("Removing Movement Tiles");
                ExploEditorController.Instance.RemoveMovementTiles(x, y);
            }

            if (Input.GetMouseButtonUp(0) && Input.GetKey(KeyCode.LeftShift))
            {
                Debug.Log("Adding EE Tiles");
                ExploEditorController.Instance.AddEETiles(x, y);
            }
            else if (Input.GetMouseButtonUp(1) && Input.GetKey(KeyCode.LeftShift))
            {
                Debug.Log("Removing EE Tiles");
                ExploEditorController.Instance.RemoveEETiles(x, y);
            }
        }
        StartCoroutine(WaitAfterClick());

    }

    public void MoveTo()
    {
        Explo_GridController.Instance.WorldPosTemp = this.transform.position;
        Explo_GridController.Instance.GeneratePathTo(x, y);
    }

    public bool CheckSpawnType()
    {
        if (Explo_GridController.Instance.Grid.ExploTiles[x, y].isStarterTile)
            return true;
        else
            return false;
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
