using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {

    private int tileX;
    private int tileY;
    private DungeonController dungeon;

    private List<Node> currentPath = null;

    int moveSpeed = 2;

	void Update ()
    {
	    if(currentPath != null)
        {
            int currNode = 0;

            while (currNode < currentPath.Count-1)
            {
                Vector3 start = dungeon.TileCoordToWorldCoord(currentPath[currNode].x, currentPath[currNode].y) + new Vector3(0, 0, -1f);
                Vector3 end = dungeon.TileCoordToWorldCoord(currentPath[currNode + 1].x, currentPath[currNode].y) + new Vector3(0, 0, -1f);

                Debug.DrawLine(start, end, Color.red);
                currNode++;
            }
        }	
	}

    public void MoveNextTile()
    {
        float remainingMovement = moveSpeed;

        while(remainingMovement > 0)
        {
            if (currentPath == null)
                return;

            remainingMovement -= dungeon.CostToEnterTile(currentPath[1].x, currentPath[1].y);

            tileX = currentPath[1].x;
            tileY = currentPath[1].y;

            transform.position = dungeon.TileCoordToWorldCoord(tileX, tileY);

            currentPath.RemoveAt(0);

            if (currentPath.Count == 1)
            {
                currentPath = null;
            }
        }
    }
}
