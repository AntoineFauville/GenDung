using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileController : MonoBehaviour {

    private int x;
    private int y;

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

        if (DungeonController.Instance.Dungeon.Tiles[x, y].isStarterTile)
            this.GetComponent<Image>().color = Color.cyan;
    }

    public void TileEnter()
    {
        if (SceneManager.GetActiveScene().name == "Editor" && !EditorController.Instance.CheckWall(x,y))
            this.GetComponent<Image>().color = Color.green;
        else
            this.GetComponent<Image>().color = new Color(255,255,0, 0.6f);
    }       

    public void TileExit()
    {
        if (SceneManager.GetActiveScene().name == "Editor" && EditorController.Instance.CheckWall(x,y))
            this.GetComponent<Image>().color = Color.red;
        else if (SceneManager.GetActiveScene().name == "Editor" && EditorController.Instance.CheckSpawn(x, y))
            this.GetComponent<Image>().color = Color.cyan;
        else if (SceneManager.GetActiveScene().name == "Editor" && EditorController.Instance.CheckMonsterSpawn(x, y))
            this.GetComponent<Image>().color = Color.magenta;
        else
            this.GetComponent<Image>().color = new Color(255, 255, 255, 0);
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
