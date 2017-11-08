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
