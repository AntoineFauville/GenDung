using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    public GameObject cursorPrefab;
    Vector3 currentFramePosition;

	
	void Update ()
    {
        currentFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentFramePosition.z = 0;

        UpdateCursor();	
	}

    public void UpdateCursor()
    {
        Tile tileUnderMouse = DungeonController.Instance.GetTileAtWorldCoord(currentFramePosition);
        if (tileUnderMouse != null && tileUnderMouse.Type != Tile.TileType.Wall)
        {
            cursorPrefab.SetActive(true);
            Vector3 cursorPosition = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
            cursorPrefab.transform.position = cursorPosition;
        }
        else
        {
            cursorPrefab.SetActive(false);
        }
    }
}
