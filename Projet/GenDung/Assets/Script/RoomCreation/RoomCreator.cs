using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class RoomCreator : MonoBehaviour
{
    public GameObject[] Props;

    public List<GameObject> PropsList = new List<GameObject>();

    private void Start()
    {
        PopulateRoom();
    }

    public void PopulateRoom()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject prop;
            prop = Props[Random.Range(0, Props.Length)];

            GameObject bob;

            bob = Instantiate(prop, new Vector3(Random.Range(-4, 4), Random.Range(-4, 4), 0), Quaternion.Euler(0, 0, 0));

            PropsList.Add(bob);
        }
    }

    public void CleanRoom()
    {
        PropsList.Clear();
    }
}

[CustomEditor(typeof(RoomCreator))]
public class RoomCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RoomCreator myScript = (RoomCreator)target;
        if (GUILayout.Button("PopulateRoom"))
        {
            myScript.CleanRoom();
            myScript.PopulateRoom();
        }
    }
}
