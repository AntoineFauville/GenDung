using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorController : MonoBehaviour {

    private static EditorController instance;

    public RoomObject room;

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
        Debug.Log("this is tile: (" + x + "," + y + ")");
        Vector2 wall = new Vector2(x, y);

        if (!room.Walls.Contains(wall))
        {
            room.Walls.Add(wall);
            Debug.Log("Wall has been added");
        }
        else
            Debug.Log("Fuck your wall");
    }

    public void RemoveWall(int x, int y)
    {
        Vector2 wall = new Vector2(x, y);

        if (room.Walls.Contains(wall))
        {
            room.Walls.Remove(wall);
            Debug.Log("Wall has been removed");
        }
        else
            Debug.Log("Wall not in database");
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
