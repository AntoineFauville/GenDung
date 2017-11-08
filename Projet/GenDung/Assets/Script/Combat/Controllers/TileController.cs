using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileController : MonoBehaviour {

    private int x;
    private int y;
    private Sprite defSprite;

    private bool clicked = false;

    public void TileClicked()
    {
        if (SceneManager.GetActiveScene().name != "Editor")
        {
            if (Input.GetMouseButtonUp(0) && CombatController.Instance.placementDone == true && clicked == false)
            {
                StartCoroutine(WaitAfterClick());
                DungeonController.Instance.WorldPosTemp = this.transform.position;
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
        else
        {
            if (Input.GetMouseButtonUp(0))
            Debug.Log("this is tile: (" + x +","+y+")");
            /*
            Insert Code here for linking to ScriptableObject.
            */
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
        }
    }

    public void TileExit()
    {
        if (DungeonController.Instance.Dungeon.Tiles[x, y].isWalkable == true)
        {
            this.GetComponent<Image>().sprite = defSprite;
        }
    }


    public IEnumerator WaitAfterClick()
    {
        clicked = true;
        yield return new WaitForSeconds(0.1f);
        clicked = false;
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
