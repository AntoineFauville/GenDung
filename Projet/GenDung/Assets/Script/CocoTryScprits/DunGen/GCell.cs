using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCell{
    public string type;  //wall, blank, entry, exit, monster, trap, treasure
    public bool visited;
    public LinkedList<GMonster> contained; //variable n'ayant d'utilité que si la GCell est de type 'monster' ou 'trap'


    public GCell()
    {
        type = null;
        visited = false;
        InitContained();
    }
    public GCell(string t)
    {
        type = t;
        visited = false;
        InitContained();
    }
    public GCell(bool v)
    {
        type = null;
        visited = v;
        InitContained();
    }
    public GCell(string t, bool v)
    {
        type = t;
        visited = v;
        InitContained();
    }

    public bool IsType(string t)
    {
        if (type == null)
        {
            if (t == null)
                return true;
            else
                return false;
        }
        return (type.Equals(t));
    }

    public void InitContained()
    {
        contained = new LinkedList<GMonster>();
    }
    public void AddTheart(GMonster gm)
    {
        contained.AddFirst(gm);
    }
    public void AddThreat(string name, int thLvl, bool istrap)
    {
        contained.AddFirst(new GMonster(name,thLvl,istrap));
    }
    public bool IsWall()
    {
        return IsType("wall");
    }
    public bool IsBlank()
    {
        return IsType("blank");
    }

}
