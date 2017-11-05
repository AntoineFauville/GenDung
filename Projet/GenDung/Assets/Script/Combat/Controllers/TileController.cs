using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileController : MonoBehaviour {

    private int x;
    private int y;
    private Sprite defSprite;

    public void TileClicked()
    {
        if (Input.GetMouseButtonUp(0) && CombatController.Instance.placementDone == true )
        {
            DungeonController.Instance.GeneratePathTo(x, y);
        }
        else if (Input.GetMouseButtonUp(1) && CombatController.Instance.placementDone == true)
        {
            DungeonController.Instance.LaunchUnitAttack(x, y);
        }
        else
        {
            Debug.Log("You didn't place your character ? TOO BAD, you can't fight");
        }
    }

    public void start()
    {
        defSprite = this.GetComponent<Image>().sprite;
    }

    public void TileEnter()
    {
        if (DungeonController.Instance.Dungeon.Tiles[x,y].isWalkable == true)
        {
            this.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/HighLightGreen");
            Debug.Log("Pointer is entering a tile: " + x + "," + y);
            Debug.Log("Spatial position of this tile: " + this.transform.position);
            Debug.Log("Spatial LocalPosition of this tile: " + this.transform.localPosition);
        }
    }

    public void TileExit()
    {
        if (DungeonController.Instance.Dungeon.Tiles[x, y].isWalkable == true)
        {
            this.GetComponent<Image>().sprite = defSprite;
        }
    }

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
    /**/
}
