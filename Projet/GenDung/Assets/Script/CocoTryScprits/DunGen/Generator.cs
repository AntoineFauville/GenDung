﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Generator{
    int x, y;

    public Generator(int x , int y)
    {
        this.x = x;
        this.y = y;
    }


	public GDung DiggingFlowGenerating()
    {
        GDung lvl = new GDung(x, y);
        lvl.FillWithWalls();
        lvl.AllUnvisited();
        int xC = Random.Range(1, x-1);
        int yC = Random.Range(1, y-1);
        lvl.SetEntry(xC, yC);
        //On a un point de départ, plus qu'à creuser un donjon à partir de ce "Current"
        lvl = RecursiveDiggingFlow(lvl, xC, yC);
        return lvl;
    }

    //(xC,yC) étant les coordonnées du point actuel de propagation
    public GDung RecursiveDiggingFlow(GDung lvl, int xC, int yC)
    {
        //Pour cet algorithme, le bool "visited" de GCell signifie "mur indémurable ou déjà démuré"
        int[] tab;
        int[] deeptab;
        LinkedList<int> ways;
        int way;

       
        ways = new LinkedList<int>();
        //On va regarder les étapes suivantes possibles
        for (int i = 0; i < 4; i++)
        {
            tab = Moved(xC, yC, i);
            if (!lvl.GetCell(tab[0], tab[1]).visited)
            {
                ways.AddLast(i);
            }
        }

        //Il existe au moins une étape suivante possible, sinon cas de base de récursivité
        if (ways.Count != 0)
        {
            way = ways.ElementAt(Random.Range(0, ways.Count));
            tab = Moved(xC, yC, way);
            lvl.SetCell("blank", true, tab[0], tab[1]);
            xC = tab[0];
            yC = tab[1];
            //Voyons voir si on doit bloquer un des mur adjacent a ce nouveau "blank"
            for (int i = 0; i < 4; i++)
            {
                tab = Moved(xC, yC, i);
                //Si la case adjacente est donc un mur qui n'est pas de toute façon déjà marqué ('visité')
                if (!lvl.GetCell(tab[0], tab[1]).visited && lvl.GetCell(tab[0], tab[1]).IsWall())
                {
                    //Pour tout les voisins de ce mur, càd pour toute direction à partir de ce mur (sauf la case d'où on vient, direction ReverseDir)
                    for(int k = 0; k < 4; k++)
                    {
                        if(k != ReverseDir(i))
                        {
                            deeptab = Moved(tab[0], tab[1], k);
                            //Si c'est un blank, alors le mur actuellement observé ne peut plus être retiré, il est donc marqué
                            if (lvl.GetCell(deeptab[0], deeptab[1]).IsBlank() || lvl.GetCell(deeptab[0], deeptab[1]).IsType("entry"))
                            {
                                lvl.GetCell(tab[0], tab[1]).visited = true;
                                break; 
                            }
                        }
                    }
                }
            }
            //Et on continue la propagation
            return RecursiveDiggingFlow(lvl, xC, yC);

        }
        else
        {
            lvl.SetExit(xC, yC); //!!!
        }
        return lvl;
    }


    public int[] Moved(int x, int y, int dir)
    {
        int[] tab = { x, y };
        switch (dir)
        {
            case 0: tab[1]++; break;
            case 1: tab[0]++; break;
            case 2: tab[1]--; break;
            case 3: tab[0]--; break;
            default: throw new System.ArgumentException("Moved(int x, int y, int dir) only accept 0,1,2,3 for the dir parameter");
        }
        return tab;
    }

    public int ReverseDir(int dir)
    {
        return (dir + 2) % 4;
    }
}
