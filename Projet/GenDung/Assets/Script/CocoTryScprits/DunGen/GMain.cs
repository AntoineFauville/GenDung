using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GMain : MonoBehaviour {

    public GameObject PrefabCell;
    public int x;
    public int y;

    public Vector2 PositionInitiale;

    void Start () {
        //GDung myLevel=GenerateSampleLevel();
        //Display(myLevel);

        
        Generator g = new Generator(x,y);
        GDung myLevel = g.DiggingFlowGenerating();
        Display(myLevel);
      
    }



    public void Display(GDung level)
    {
        int xL = level.GetWidth();
        int yL = level.GetHeight();
        Vector2 offset;
        offset.x = 0.35f;
        offset.y = 0.35f;
        Vector2 position;
        position = PositionInitiale;
        Renderer rend;
        for (int i = 0; i < xL; i++)
        {
            for (int j = 0; j < yL; j++)
            {
                GameObject CellGameObject;

                Cell cell = new Cell();

                CellGameObject = InstantiatePrefab(position);

                CellGameObject.name = cell.ID + "_" + i + "_" + j;
                rend = CellGameObject.GetComponent<Renderer>();

                switch (level.GetCell(i, j).type)
                {
                    case "wall": rend.material.color = Color.black;
                        break;
                    case "blank": rend.material.color = Color.white;
                        break;
                    case "entry": rend.material.color = new Color(0, 0.55f, 0, 1);
                        break;
                    case "exit": rend.material.color = new Color(0, 1, 0, 1);
                        break;
                    default:
                        Debug.Log("Le type de (" + i + "," + j + ") est " + level.GetCell(i, j).type);
                        rend.material.color = Color.yellow;
                        break;
                }

        position.y += offset.y;
            }
            position.y = PositionInitiale.y;
            position.x += offset.x;
        }
    }   

    private GameObject InstantiatePrefab(Vector2 pos)
    {
        GameObject bob;
        bob = Instantiate(PrefabCell, pos, Quaternion.Euler(0, 0, 0));
        return bob;
    }

    public GDung GenerateSampleLevel()
    {
        GDung myLevel = new GDung(x, y);
        myLevel.GenerateBorderWalls();
        myLevel.NullToBlank();
        return myLevel;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
