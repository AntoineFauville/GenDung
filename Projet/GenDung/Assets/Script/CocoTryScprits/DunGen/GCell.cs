using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCell{
    public string type;  //wall, blank, entry, exit
    public bool visited;

    public GCell()
    {
        type = null;
        visited = false;
    }
    public GCell(string t)
    {
        type = t;
        visited = false;
    }
    public GCell(bool v)
    {
        type = null;
        visited = v;
    }
    public GCell(string t, bool v)
    {
        type = t;
        visited = v;
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
    public bool IsWall()
    {
        return IsType("wall");
    }
    public bool IsBlank()
    {
        return IsType("blank");
    }

}
