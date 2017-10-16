using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour {

    public int x;
    public int y;

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
