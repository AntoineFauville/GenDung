using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorController : MonoBehaviour {

    private static EditorController instance;

    public RoomObject room;
    public Vector2 wall;

    public void Start()
    {
        CreateInstance();
        GameObject.FindGameObjectWithTag("backgroundOfRoom").transform.GetComponent<Image>().sprite = room.back;
    }

    void CreateInstance()
    {
        if (instance != null)
        {
            Debug.Log("There should never have two combat controllers.");
        }
        instance = this;
    }

    public void AddWall(int x, int y)
    {
        wall = new Vector2(x, y);

        if (!room.Walls.Contains(wall))
        {
            room.Walls.Add(wall);
            Debug.Log("Wall has been added : (" + x + "," + y + ")");
        }
        else
            Debug.Log("Non non non, Ce ne sont pas la loutre (" + x + "," + y + ") que vous recherchez ... ");
    }

    public void RemoveWall(int x, int y)
    {
         wall = new Vector2(x, y);

        if (room.Walls.Contains(wall))
        {
            room.Walls.Remove(wall);
            Debug.Log("Wall has been removed : (" + x + "," + y + ")");
        }
        else
            Debug.Log("Cette loutre (" + x + "," + y + ") n'as pas la Force en elle ... ");
    }

    public bool CheckWall(int x, int y)
    {
        Vector2 test = new Vector2(x, y);
        return room.Walls.Contains(test);
    }

    /* Accessors Methods */
    public static EditorController Instance
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
