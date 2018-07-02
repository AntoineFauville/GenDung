using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Generator{

    //Paramètres relatifs au layout, à la génération des murs, corridors, salles
    public int x, y;
    public int maxSteps, minSteps; //Longueur max/min du plus court chemin allant de l'entrée au boss
    public int ramPerc;  //A quel point on veut de la de ramification (valeur entre 0 et 100, les variations pertinentes étant entre 0 et 30)
    public int loopPerc; //A quel point on veut des boucles (valeur entre 0 et 100, les variations pertinentes étant entre 0 et 25)

    //Paramètres relatifs aux monstres, aux trésors,...
    public int expThreatLvl;  //Le niveau de difficulté approximatif du donjon en fonction du thread des monstres (le générateur peut légèrement le dépasser) /!\BOSS NON COMPRIT
    public LinkedList<GMonster> monsters;  //La liste des monstres pouvant être présent dans le donjon. Un monstre a un nom et un niveau de menace, et peut être ajouté avec AddMonster(string name, int threatlvl)
    public LinkedList<GMonster> traps;     //La liste des pièges pouvant être présent dans le donjon. 
   // public int trapPorc;      //(0-100) A quel point on veut des pièges dans le donjon
    public int avgMonsterPerCell;
    public int maxMonsterPerCell; //Nombre max de monstrer en une seule salle
    public int minMonsterPerCell;
    public int groupingDeviation; //(0-100) A quel point les ennemis se regroupent en nombre différent de celui de avgMonsterPerCell. A 0, toute les cases auront soit 0 monstres soit avgMonsterPerCell monstres
    public int avgTreasure; //Nombre moyen de trésor sur la map

    //Variables prenant de nouvelles valeurs à chaque génération de niveau, servant à titre indicatif pour l'utilisateur qui veut avoir des infos sur le niveau qu'il vient de générer
    public int LgSwLength; //"last generated shortest way length", la longueur du plus court chemin du dernier maze généré avec ce constructeur
    public LinkedList<int[]> shortestpath; //La description complète du plus court chemin. Chaque élément est un int[3] de la forme (x,y,profondeur). PrintPath() affiche ce chemin dans la console

    private int lastDir;
    private bool exitDone;
    private int currentTL;

    public Generator(int x , int y)
    {
        this.x = x;
        this.y = y;
        AutoMaxSteps();
        AutoMinSteps();
        ramPerc = 60;
        loopPerc = 25;
        ResetThreats();
        Init();        
    }

    public Generator(int x, int y, int maxSteps, int minSteps, int ramPerc, int loopPerc) 
    {
        this.x = x;
        this.y = y;

        if (maxSteps == -1)
            AutoMaxSteps();
        else
            this.maxSteps = maxSteps;

        if (minSteps == -1)
            AutoMinSteps();
        else
            this.minSteps = minSteps;

        this.ramPerc = ramPerc;
        this.loopPerc = loopPerc;
        ResetThreats();
        Init();
    }

    private void Init()
    {
        lastDir = -1;
        exitDone = false;
        currentTL = 0;
    }

    public void ResetThreats()
    {
        monsters = new LinkedList<GMonster>();
        traps = new LinkedList<GMonster>();
    }

    public void AddMonster(string name, int threatlvl)
    {
        monsters.AddFirst(new GMonster(name, threatlvl,false));
    }
    public void AddTrap(string name, int threatlvl)
    {
        traps.AddFirst(new GMonster(name, threatlvl, true));
    }

    public GDung Generate()
    {
        GDung dung = DiggingFlowGenerating();
        return ContentGenerating(dung);
    }

    public GDung ContentGenerating(GDung lvl)
    {
        lvl.AllBlankUnivisited();
        currentTL=0; //currentThreatLevel
        int count = 0;
        int rnd1, rnd2;
        int[] posiTmp;
        GCell cellTmp;

        //Ajoutons quelques monstres sur le plus court chemin
        while (count < (LgSwLength / 2) && currentTL < expThreatLvl)
        {
            rnd1 = Random.Range(1, shortestpath.Count - 2);
            posiTmp = shortestpath.ElementAt(rnd1);
            cellTmp = lvl.GetCell(posiTmp[0], posiTmp[1]);
            if (!cellTmp.visited)
            {
                cellTmp = GenerateMonsterCell(true);
                lvl.SetCell(cellTmp, posiTmp[0], posiTmp[1]);
                count++;
            }
            count++;
        }

        //Ajoutons des monstres supplémentaires
        count = 0;
        while (currentTL < expThreatLvl && count < 2000)
        {
            rnd1 = Random.Range(1, x - 1);
            rnd2 = Random.Range(1, y - 1);
            if (!lvl.GetCell(rnd1, rnd2).visited)
            {
                cellTmp = GenerateMonsterCell(false);
                lvl.SetCell(cellTmp, rnd1, rnd2);
            }
            count++;
        }
        
        return TreasuresGenerating(lvl);
    }

    public GDung TreasuresGenerating(GDung lvl)
    {
        int nbTreasures = 0;
        int[] tab;
        int focusTreasures=avgTreasure;
        if (ShouldI(30))
        {
            //je vais augmenter ce nombre
            if (ShouldI(50))
            {
                //de 15%
                if (ShouldI(70))
                    focusTreasures = (int)(avgTreasure * 1.15);
                //de 25%
                else
                    focusTreasures = (int)(avgTreasure * 1.25);
                //focusTreasures++;
            }
            //je vais diminuer ce nombre
            else
            {
                //de 15%
                if (ShouldI(70))
                    focusTreasures = (int)(avgTreasure * 0.85);
                //de 25%
                else
                    focusTreasures = (int)(avgTreasure * 0.75);
                //focusTreasures--;
            }
        }
        while (nbTreasures < focusTreasures)
        {
            tab = FindUnvisitedDeadEnd(lvl);
            if (tab[0] != 0)
            {
                lvl.SetTreasure(tab[0], tab[1]);
            }
            nbTreasures++;
        }
        return lvl;
    }

    public int[] FindUnvisitedDeadEnd(GDung lvl)
    {
        int attempt = 0;
        int tX=0, tY=0;
        int[] deeptab;
        int nbWalls;
        bool found = false;
        while(!found && attempt < 100)
        {
            Debug.Log("Attempt: " + attempt);
            tX = Random.Range(1, x-1);
            tY = Random.Range(1, y-1);
            if (!lvl.GetCell(tX, tY).visited)
            {
                nbWalls = 0;
                for(int k = 0; k < 4; k++)
                {
                    deeptab = Moved(tX, tY, k);
                    if (lvl.GetCell(deeptab[0], deeptab[1]).IsWall())
                        nbWalls++;
                }
                if (nbWalls == 3)
                {
                    found = true;
                    break;
                }
               
            }
            attempt++;
        }
        //Impossible de trouver un coin, tantpis on va juste mettre le trésor sur une case non visitée random
        
        attempt = 0;
        while(!found && attempt < 10)
        {
            tX = Random.Range(1, x - 1);
            tY = Random.Range(1, y - 1);
            if (!lvl.GetCell(tX, tY).visited)
            {
                found = true;
                break;
            }
            attempt++;
        }
        
        if (!found)
        {
            int[] result = { 0, 0 };
            return result;
        }
        else
        {
            int[] result = { tX, tY };
            return result;
        }
    }

    public GCell GenerateMonsterCell(bool main)
    {
        double multiplicator;
        GCell cellTmp = new GCell("monster", true);
        int groupingUp = 1;
        GMonster monsTmp = PickRandomMonster();
        cellTmp.AddTheart(monsTmp);
        int tmpTLAdded = monsTmp.threatlvl;
        while ( (groupingUp < minMonsterPerCell || PonderedShouldI(groupingUp)) && groupingUp < maxMonsterPerCell && currentTL < expThreatLvl)
        {
            monsTmp = PickRandomMonster();
            cellTmp.AddTheart(monsTmp);
            tmpTLAdded = tmpTLAdded + monsTmp.threatlvl;
            groupingUp++;
        }
        multiplicator = 1 + ((groupingUp-1) * 0.20);
        tmpTLAdded = (int)(tmpTLAdded * multiplicator);
        if (!main)
        {
            tmpTLAdded = (int)(tmpTLAdded * 0.90);
        }
        else
        {
            tmpTLAdded = (int)(tmpTLAdded * 1.20);
        }
        currentTL = currentTL + tmpTLAdded;
        return cellTmp;
    }

    public GMonster PickRandomMonster()
    {
        return monsters.ElementAt(Random.Range(0, monsters.Count - 1));
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
        LinkedList<int[]> path = LocalFindPath(lvl, lvl.entrCoord[0], lvl.entrCoord[1], lvl.exitCoord[0], lvl.exitCoord[1]);
        LgSwLength = path.ElementAt(path.Count - 1)[2];
        
        if (path.ElementAt(path.Count - 1)[2] < minSteps)
        {
            for(int ow = 0; ow < 60; ow++)
            {
                //Debug.Log("Boss too close from the entry, generating another dungeon...");
                lvl = DiggingFlowGeneratingSample();
                path = LocalFindPath(lvl, lvl.entrCoord[0], lvl.entrCoord[1], lvl.exitCoord[0], lvl.exitCoord[1]);
                LgSwLength = path.ElementAt(path.Count - 1)[2];
                if (path.ElementAt(path.Count - 1)[2] >= minSteps)
                    break;
                if (ow == 59)
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

        //Il existe au moins une étape suivante possible et on n'a pas dépassé le nombre de pas max, sinon cas de base de récursion
        if (ways.Count != 0 && nbSteps< (maxSteps+1))
        {
            way = ways.ElementAt(Random.Range(0, ways.Count));
            ways.Remove(way);
            tab = Moved(xC, yC, way);
            lvl.SetCell("blank", true, tab[0], tab[1]);
            xN = tab[0];
            yN = tab[1];

            if (!ShouldI(loopPerc) || HowTurned(lastDir,way)==-2)
            {
            //CAS OU LES BOUCLES SONT INTERDITES
                //Voyons voir si on doit bloquer un des mur adjacent a ce nouveau "blank"
                for (int i = 0; i < 4; i++)
                {
                    tab = Moved(xN, yN, i);
                    //Si la case adjacente est donc un mur qui n'est pas de toute façon déjà marqué ('visité')
                    if (!lvl.GetCell(tab[0], tab[1]).visited && lvl.GetCell(tab[0], tab[1]).IsWall())
                    {
                        //Pour tout les voisins de ce mur, càd pour toute direction à partir de ce mur (sauf la case d'où on vient, direction ReverseDir)
                        for (int k = 0; k < 4; k++)
                        {
                            if (k != ReverseDir(i))
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
            //FIN CAS OU LES BOUCLE SONT INTERDITES
            }
            else
            {
            //CAS OU LES BOUCLES SONT PERMISE
                //A-t-on tourné?
                int tur = HowTurned(lastDir, way);
                if (tur == 1 || tur == -1)
                {
                    deeptab = Moved(xC,yC,Turn(way, tur));
                    //On va bloqué le mur se trouvant dans la même rotation que la rotation que l'on vient de faire
                    lvl.GetCell(deeptab[0], deeptab[1]).visited = true;
                }
            //FIN CAS OU LES BOUCLES SONT PERMISE
            }

            //Et on continue la propagation
            lastDir = way;
            lvl = RecursiveDiggingFlow(lvl, xN, yN,nbSteps+1);

            //Mais doit-on ramifier?
            if(ShouldI(ramPerc))
            {
                lastDir = -1;
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
    
    //      0
    //    3 X 1
    //      2         
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

    //           -1 <-- 0 --> 1  (0=not turned)
    public int HowTurned(int oldDir, int newDir)
    {
        if (newDir == (oldDir + 1) % 4)
            return 1;
        else if (newDir == oldDir || oldDir == -1)
            return 0;
        else if ((oldDir == 0 && newDir == 4) || newDir == oldDir - 1)
            return -1;
        else
                return -2;          
    }

    public static int Turn(int oldDir, int turningWay)
    {
        if (turningWay == 1)
            return (oldDir + 1) % 4;
        else if(turningWay == 0)
            return oldDir;
        else if(turningWay == -1)
        {
            if (oldDir == 0)
                return 3;
            else
                return oldDir - 1;
        }
        else
            throw new System.Exception("Turn only accept 1, 0 and -1 as ValueSystem for turningWay parameter");
    }

    public static int ReverseDir(int dir)
    {
        return (dir + 2) % 4;
    }

    public bool PonderedShouldI(int groupingUp)
    {
        int perc;
        if(avgMonsterPerCell == groupingUp)
        {
            perc = (int)(0.4 * groupingDeviation);
            
        }
        else if(groupingUp>avgMonsterPerCell)
        {

            //Debug.Log("Wtffffffffffff  avg="+avgMonsterPerCell+"  and gorupingUp="+groupingUp);
            
            perc = (int)(0.5 * groupingDeviation) - ((groupingUp - avgMonsterPerCell)*((int) (110-groupingDeviation)/8));
            if (perc < 0)
                perc = 0;

        }
        else //On est inférieur à l'avg
        {
            perc = 100 - (int)(groupingDeviation * 0.4);
            //perc = 100;
        }

        bool resul= ShouldI(perc);
        Debug.Log("result: "+resul);
        return resul;
    }

    public static bool ShouldI(int rPerc)
    {

        int rnd = Random.Range(0, 100);
        if (rPerc >= rnd)
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

    public void AutoMaxSteps()
    {
        int mult = ((x - 1) * (y - 1));
        if (mult < 40)
            maxSteps = 15;
        else if (mult < 60)
            maxSteps = 23;
        else if (mult < 105)
            maxSteps = 25;
        else
            maxSteps = mult / 4;
    }
    public void AutoMinSteps()
    {
        int sum = x + y;
        if (sum < 8)
            minSteps = 1;
        else if (sum < 10)
            minSteps = 2;
        else if (sum < 11)
            minSteps = 3;
        else if (sum < 12)
            minSteps = 4;
        else if (sum < 15)
            minSteps = sum - 6;
        else if (sum < 35)
            minSteps = sum - 8;
        else if (sum < 50)
            minSteps = sum - 9;
        else if (sum < 100)
            minSteps = sum - 19;
        else
            minSteps = sum / 2;
    }

    public static LinkedList<int[]> FindPath(GDung lvl, int xFinish, int yFinish, int xDepart, int yDepart)
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
            //La longueur du plus court chemin est donc path.ElementAt(path.Count - 1)[2]
        }
        else
        {
            throw new System.Exception("No path between ("+xDepart+","+yDepart+") and ("+xFinish+","+yFinish+")");
        }
    }

    private LinkedList<int[]> LocalFindPath(GDung lvl, int xFinish, int yFinish, int xDepart, int yDepart)
    {
        LinkedList<int[]> path = FindPath(lvl, xFinish, yFinish, xDepart, yDepart);
        shortestpath = path;
        return path;
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

    public void PrintPath()
    {
        Debug.Log("               Shortest path:  ");
        string result="";
        int[] node;
        for(int i = 0; i < shortestpath.Count; i++)
        {
            node = shortestpath.ElementAt(i);
            result = result + node[2] + ":(" + node[0] + "," + node[1] + ") ";
        }
        Debug.Log(result);
    }


}
