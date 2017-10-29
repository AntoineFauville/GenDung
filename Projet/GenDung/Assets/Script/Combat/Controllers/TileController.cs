using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileController : MonoBehaviour {

    private int x;
    private int y;

    Sprite defSprite;

    public void TileCkicked()
    {
        if (Input.GetMouseButtonUp(0))
        {
            DungeonController.Instance.GeneratePathTo(x, y);
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            DungeonController.Instance.LaunchUnitAttack(x, y);
        }
    }

    public void start()
    {
        defSprite = this.GetComponent<Image>().sprite;
    }

    public void TileEnter()
    {
        this.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/HighLightGreen");
        //Debug.Log("Pointer is entering a tile: " + x + "," + y);
    }

    public void TileExit()
    {
        this.GetComponent<Image>().sprite = defSprite;
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
