using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExploEditorController : MonoBehaviour {

    private static ExploEditorController instance;

    public ExploMap exploMap;
    private Vector2 movementTile;
    private Vector2 eeTile;

    public void Start()
    {
        CreateInstance();
    }

    void CreateInstance()
    {
        if (instance != null)
        {
            Debug.Log("There should never have two combat controllers.");
        }
        instance = this;
    }

    /* Walls Related Methods */
    public void AddMovementTiles(int x, int y)
    {
        movementTile = new Vector2(x, y);

        if (!exploMap.movTiles.Contains(movementTile))
        {
            exploMap.movTiles.Add(movementTile);
            Debug.Log("Movement Tiles has been added : (" + x + "," + y + ")");
        }
        else
            Debug.Log("Non non non, Ce ne sont pas la loutre (" + x + "," + y + ") que vous recherchez ... ");
    }

    public void RemoveMovementTiles(int x, int y)
    {
        movementTile = new Vector2(x, y);

        if (exploMap.movTiles.Contains(movementTile))
        {
            exploMap.movTiles.Remove(movementTile);
            Debug.Log("Wall has been removed : (" + x + "," + y + ")");
        }
        else
            Debug.Log("Cette loutre (" + x + "," + y + ") n'as pas la Force en elle ... ");
    }

    public bool CheckMovementTiles(int x, int y)
    {
        Vector2 test = new Vector2(x, y);
        return exploMap.movTiles.Contains(test);
    }
    /* */

    /* Walls Related Methods */
    public void AddEETiles(int x, int y)
    {
        eeTile = new Vector2(x, y);

        if (!exploMap.eeTiles.Contains(eeTile))
        {
            exploMap.eeTiles.Add(eeTile);
            Debug.Log("Movement Tiles has been added : (" + x + "," + y + ")");
        }
        else
            Debug.Log("Non non non, Ce ne sont pas la loutre (" + x + "," + y + ") que vous recherchez ... ");
    }

    public void RemoveEETiles(int x, int y)
    {
        eeTile = new Vector2(x, y);

        if (exploMap.eeTiles.Contains(eeTile))
        {
            exploMap.eeTiles.Remove(eeTile);
            Debug.Log("Wall has been removed : (" + x + "," + y + ")");
        }
        else
            Debug.Log("Cette loutre (" + x + "," + y + ") n'as pas la Force en elle ... ");
    }

    public bool CheckEETiles(int x, int y)
    {
        Vector2 test = new Vector2(x, y);
        return exploMap.eeTiles.Contains(test);
    }
    /* */

    /* Accessors Methods */
    public static ExploEditorController Instance
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
}
