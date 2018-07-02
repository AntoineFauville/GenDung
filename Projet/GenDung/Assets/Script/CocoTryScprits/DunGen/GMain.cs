using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GMain : MonoBehaviour {

    [SerializeField]private GameObject PrefabCell;
    [SerializeField]private Sprite bosstile;
    [SerializeField] private Sprite e1;
    [SerializeField] private Sprite e2;
    [SerializeField] private Sprite e3;
    [SerializeField] private Sprite e4;
    [SerializeField] private Sprite e5;
    [SerializeField] private Sprite treasuretile;
    public int x, y, maxSteps, minSteps, ramPerc, loopPerc, threatLvl, avgTreasure;

    public Vector2 PositionInitiale;

    void Start () {
        //GDung myLevel=GenerateSampleLevel();
        //Display(myLevel);

        /*
        public int x, y;
        public int maxSteps, minSteps; //Longueur max/min du plus court chemin allant de l'entrée au boss
        public int ramPerc; //% de ramification
        public int loopPerc; //% d'acceptation des boucles
        */

        Generator g = new Generator(x,y,maxSteps,minSteps,ramPerc,loopPerc);
        g.AddMonster("Monster1", 10);
        g.AddMonster("Monster2", 15);
        g.AddMonster("Monster3", 20);
        g.minMonsterPerCell = 2;
        g.maxMonsterPerCell = 5;
        g.groupingDeviation = 65;
        g.avgMonsterPerCell = 3;
        g.expThreatLvl = threatLvl;
        g.avgTreasure = avgTreasure;
        //Generator g = new Generator(x, y);
        GDung myLevel = g.Generate();
        
        Display(myLevel);
        Debug.Log("Min: " + g.minSteps + "   Max: " + g.maxSteps+ "     ram:"+g.ramPerc+ "        loop:"+g.loopPerc);
        Debug.Log("Actual: "+g.LgSwLength);
        g.PrintPath();

    }



    public void Display(GDung level)
    {
        int xL = level.GetWidth();
        int yL = level.GetHeight();
        Vector2 offset;
        offset.x = 0.34f;
        offset.y = 0.34f;
        Vector2 position;
        position = PositionInitiale;
        Renderer rend;
        SpriteRenderer rend2;
        for (int i = 0; i < xL; i++)
        {
            for (int j = 0; j < yL; j++)
            {
                GameObject CellGameObject;

                Cell cell = new Cell();

                CellGameObject = InstantiatePrefab(position);

                CellGameObject.name = cell.ID + "_" + i + "_" + j;
                rend = CellGameObject.GetComponent<Renderer>();
                rend2 = CellGameObject.GetComponent<SpriteRenderer>();

                switch (level.GetCell(i, j).type)
                {
                    case "wall": rend.material.color = Color.black;
                        break;
                    case "blank": rend.material.color = Color.white;
                        break;
                    case "entry": rend.material.color = new Color(0, 0.75f, 0, 1);
                        break;
                    case "exit": rend.material.color = new Color(0, 1, 0, 1);
                        rend2.sprite = bosstile;
                        break;
                    case "monster":
                        if (level.GetCell(i, j).contained.Count == 1)
                            rend2.sprite = e1;
                        else if (level.GetCell(i, j).contained.Count == 2)
                            rend2.sprite = e2;
                        else if (level.GetCell(i, j).contained.Count == 3)
                            rend2.sprite = e3;
                        else if (level.GetCell(i, j).contained.Count == 4)
                            rend2.sprite = e4;
                        else if (level.GetCell(i, j).contained.Count == 5)
                            rend2.sprite = e5;
                        else
                        {
                            rend2.sprite = e5;
                            rend.material.color = Color.red;
                        }
                        break;
                    case "treasure":
                        rend2.sprite = treasuretile;
                        //rend.material.color = Color.cyan;
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
