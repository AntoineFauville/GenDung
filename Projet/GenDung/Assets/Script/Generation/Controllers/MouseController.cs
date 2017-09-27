using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    public GameObject cursorPrefab;
    Vector3 currentFramePosition;
    Sprite highlight = null;

    void Start()
    {
        GameObject cursor_go = new GameObject();
        cursor_go.transform.position = Vector3.zero;
        cursor_go.name = "Highlight";
        cursor_go.transform.localScale = new Vector3(2, 2, 2);
        highlight = Resources.Load<Sprite>("Sprites/HighLightGreen");
        SpriteRenderer cursor_sr = cursor_go.AddComponent<SpriteRenderer>();
        cursor_sr.sprite = highlight;
        cursor_sr.sortingLayerName = "TileUI";

        cursorPrefab = cursor_go;
    }
	
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
