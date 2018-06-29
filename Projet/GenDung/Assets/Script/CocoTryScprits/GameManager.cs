using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PrefabCell;
    public int rows;
    public int colomns;

    public Vector2 offset;
    public Vector2 PositionInitiale;

    void Start()
    {
        Vector2 position;
        position = PositionInitiale;

        for (int y = 0; y < colomns; y++)
        {
            for (int x = 0; x < rows; x++)
            {
                GameObject CellGameObject;

                Cell cell = new Cell();

                CellGameObject = InstantiatePrefab(position);

                CellGameObject.name = cell.ID + "_" + x + "_" + y;

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
