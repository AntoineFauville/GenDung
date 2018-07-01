using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Generator{

    public int x, y, maxSteps, minSteps;
    public bool loop;
    public int ramPerc; //% de ramification


    private int lastTurn;
    private int lastDir;
    private bool exitDone;

    public Generator(int x , int y)
    {
        this.x = x;
        this.y = y;
        loop = true;
        //autoMaxStep();
        maxSteps = 25;
        minSteps = 10;
        ramPerc = 35;

        exitDone = false;
    }


	public GDung DiggingFlowGeneratingSample()
    {
        exitDone = false;
        GDung lvl = new GDung(x, y);
        lvl.FillWithWalls();
        lvl.AllUnvisited();
        int xC = Random.Range(1, x-1);
        int yC = Random.Range(1, y-1);
        lvl.SetEntry(xC, yC);
        //On a un point de départ, plus qu'à creuser un donjon à partir de ce "Current"

        lvl = RecursiveDiggingFlow(lvl, xC, yC,0);


        return lvl;
    }

    public GDung DiggingFlowGenerating()
    {
        GDung lvl = DiggingFlowGeneratingSample();
        LinkedList<int[]> path = FindPath(lvl, lvl.entrCoord[0], lvl.entrCoord[1], lvl.exitCoord[0], lvl.exitCoord[1]);
        //Debug.Log(path.ElementAt(path.Count - 1)[2]);

        if(path.ElementAt(path.Count - 1)[2] < minSteps)
        {
            for(int ow = 0; ow < 25; ow++)
            {
                //Debug.Log("Boss too close from the entry, generating another dungeon...");
                lvl = DiggingFlowGeneratingSample();
                path = FindPath(lvl, lvl.entrCoord[0], lvl.entrCoord[1], lvl.exitCoord[0], lvl.exitCoord[1]);
                //Debug.Log(path.ElementAt(path.Count - 1)[2]);
                if(path.ElementAt(path.Count - 1)[2] >= minSteps)
                    break;
                if (ow == 24)
                    throw new System.Exception("UNEXPECTED EXCEPTION");
            }
        }

        return lvl;
    }

    //(xC,yC) étant les coordonnées du point actuel de propagation
    public GDung RecursiveDiggingFlow(GDung lvl, int xC, int yC, int nbSteps)
    {
        //Pour cet algorithme, le bool "visited" de GCell signifie "mur indémurable ou déjà démuré"
        int[] tab;
        int[] deeptab;
        int xN, yN;
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

        //Il existe au moins une étape suivante possible et on n'a pas dépassé le nombre de pas max, sinon cas de base de récursivité
        if (ways.Count != 0 && nbSteps< maxSteps)
        {
            way = ways.ElementAt(Random.Range(0, ways.Count));
            ways.Remove(way);
            tab = Moved(xC, yC, way);
            lvl.SetCell("blank", true, tab[0], tab[1]);
            xN = tab[0];
            yN = tab[1];
            //Voyons voir si on doit bloquer un des mur adjacent a ce nouveau "blank"
            for (int i = 0; i < 4; i++)
            {
                tab = Moved(xN, yN, i);
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
            lvl = RecursiveDiggingFlow(lvl, xN, yN,nbSteps+1);

            //Mais doit-on ramifier?
            if(ShouldI())
            {
                //Debug.Log("yo mamen");
                lvl = RecursiveDiggingFlow(lvl, xC, yC, nbSteps);
            }


        }
        else
        {
            if (!exitDone)
            {
                lvl.SetExit(xC, yC);
                exitDone = true;
            }
        }
        return lvl;
    }

    public static int[] Moved(int x, int y, int dir)
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

    public bool ShouldI()
    {

        int rnd = Random.Range(0, 100);
        if (ramPerc >= rnd)
        {
            //Debug.Log("WIN " + rnd + " vs " + ramPerc);
            return true;
        }
        else
        {
            //Debug.Log("LOS " + rnd + " vs " + ramPerc);
            return false;
        }
    }

    public void autoMaxStep()
    {
        maxSteps = ((x - 1) * (y - 1)) / 2;
    }

    public static LinkedList<int[]> FindPath(GDung lvl, int xDepart, int yDepart, int xFinish, int yFinish)
    {
        LinkedList<int[]> path = new LinkedList<int[]>();

        LinkedList<int[]> coords = new LinkedList<int[]>();
        int[] newtab;
        int[] tab = { xFinish, yFinish, 0 };
        coords.AddLast(tab);
        bool found = false;

        int i = 0;
        int length;
        bool alreadyIn;
        //Pour chaque noeud de la liste
        while(i<coords.Count && !found){
            length = coords.ElementAt(i)[2] + 1;
            //Regardons les cases autour
            for(int k = 0; k < 4; k++)
            {
                tab = Moved(coords.ElementAt(i)[0], coords.ElementAt(i)[1], k);
                //Si ce n'est pas un mur
                if (!lvl.GetCell(tab[0], tab[1]).IsWall())
                {
                    alreadyIn = false;
                    //Est ce que cette coordonnée est déjà dans la liste?
                    for (int it = 0; it < coords.Count; it++)
                    {
                        if (coords.ElementAt(it)[0] == tab[0] && coords.ElementAt(it)[1] == tab[1])
                        {
                            if (coords.ElementAt(it)[2] <= length)
                            {
                                alreadyIn = true;
                                break;
                            }
                        }
                    }
                    //Sinon on l'ajoute
                    if (!alreadyIn)
                    {
                        newtab = new int[3];
                        newtab[0] = tab[0]; newtab[1] = tab[1]; newtab[2] = length;
                        coords.AddLast(newtab);
                        if(newtab[0]==xDepart && newtab[1] == yDepart)
                        {
                            found = true;
                        }
                    }
                }
            }
            i++;
        }

        if (found)
        {
            int id;
            tab = coords.ElementAt(coords.Count - 1);
            int lastX = tab[0]; 
            int lastY = tab[1];
            path.AddFirst(tab);
            coords.Remove(tab);

            while (path.ElementAt(0)[0] != xFinish || path.ElementAt(0)[1] != yFinish)
            {
                for(int it = 0; it < coords.Count; it++)
                {
                    id = LowestNeighbourId(coords, lastX, lastY);
                    tab = coords.ElementAt(id);
                    lastX = tab[0];
                    lastY = tab[1];
                    path.AddFirst(tab);
                    coords.Remove(tab);
                    if (tab[2] == 0)
                        break;
                }
            }
            return path;
        }
        else
        {
            throw new System.Exception("No path between ("+xDepart+","+yDepart+") and ("+xFinish+","+yFinish+")");
        }
    }

    //coords element format: (x,y,height)
    public static int LowestNeighbourId(LinkedList<int[]> coords, int xC, int yC)
    {

        int min = int.MaxValue;
        int id=-1;
        int[] tT;
        LinkedList<int> ids = new LinkedList<int>();
        //Regardons tout les voisins
        for (int k = 0; k < 4; k++)
        {
            tT=Moved(xC, yC, k);
            //Regardons tout les éléments de coords pour les comparer à chaque voisin
            for(int i = 0; i < coords.Count; i++)
            {
                //Un élément de coords correspond au (x,y)
                if(coords.ElementAt(i)[0]==tT[0] && coords.ElementAt(i)[1] == tT[1])
                {
                    //Et il est plus bas
                    if (coords.ElementAt(i)[2] < min)
                    {
                        min = coords.ElementAt(i)[2];
                        id = i;
                    }
                }
            }
        }
        return id;
    }

}
