using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PrefabCell;
    public int x;
    public int y;

//    public Vector2 offset;
    public Vector2 PositionInitiale;

    void Start()
    {
        Vector2 offset;
        offset.x = 0.35f;
        offset.y = 0.35f;
        Vector2 position;
        position = PositionInitiale;
        Renderer rend;  //yo
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                GameObject CellGameObject;

                Cell cell = new Cell();

                CellGameObject = InstantiatePrefab(position);

                CellGameObject.name = cell.ID + "_" + j + "_" + i;
                rend = CellGameObject.GetComponent<Renderer>();  
                rend.material.color = Color.red; 
                position.x += offset.x;
            }
            position.x = PositionInitiale.x;
            position.y += offset.y;
        }
    }

    private GameObject InstantiatePrefab(Vector2 pos)
    {
        GameObject bob;
        bob = Instantiate(PrefabCell, pos, Quaternion.Euler(0, 0, 0));
        return bob;
    }
}
