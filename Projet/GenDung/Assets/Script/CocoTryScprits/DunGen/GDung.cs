using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GDung {
    public GCell[,] map;
    public int[] entrCoord, exitCoord;


    public GDung(int x, int y)
    {
        entrCoord = new int[2];
        exitCoord = new int[2];
        map = new GCell[x,y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                SetCell(new GCell(), i, j);
            }
        }
    }

    public GCell GetCell(int x, int y)
    {
        return map[x,y];
    }
    public void SetCell(GCell gc, int x, int y)
    {
        map[x, y] = gc;
    }
    public void SetCell(string t, int x, int y)
    {
        map[x, y] = new GCell(t);
    }
    public void SetCell(int x, int y)
    {
        map[x, y] = new GCell();
    }
    public void SetCell(string t, bool v, int x, int y)
    {
        map[x, y] = new GCell(t, v);
    }
    public void SetEntry(int x, int y)
    {
        SetCell("entry",true, x, y);
        entrCoord[0] = x;
        entrCoord[1] = y;
    }
    public void SetExit(int x, int y)
    {
        exitCoord[0] = x;
        exitCoord[1] = y;
        SetCell("exit",true, x, y);
    }
    public void SetMonster(int x, int y, GMonster monster)
    {
        GCell tmp = new GCell("monster");
        tmp.AddTheart(monster);
        SetCell(tmp, x, y);
    }
    public void SetTreasure(int x, int y)
    {
        SetCell("treasure", true, x, y);
    }

    public void NullToBlank()
    {
        NullTo("blank");
    }
    public void NullTo(string newtype)
    {
        int x = GetWidth();
        int y = GetHeight();
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (String.IsNullOrEmpty(map[i, j].type))
                {
                    map[i, j].type = newtype;
                }
            }
        }
    }

    public void GenerateBorderWalls()
    {
        int x = GetWidth(), y = GetHeight();
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (i == 0 || i == (x - 1) || j == 0 || j == (y - 1))
                {
                    SetCell("wall", true, i, j);
                }
            }
        }
    }

    public void FillWithWalls()
    {
        int x = GetWidth(), y = GetHeight();
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (i == 0 || i == (x - 1) || j == 0 || j == (y - 1))
                {
                    SetCell("wall", true, i, j);
                }
                else
                {
                    SetCell("wall", false, i, j);
                }
            }
        }
    }

    //Every cell but the border walls
    public void AllUnvisited()
    {
        int x = GetWidth(), y = GetHeight();
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (i == 0 || i == (x - 1) || j == 0 || j == (y - 1))  //Border wall case
                {
                    GetCell(i, j).visited = true;
                }
                else
                {
                    GetCell(i, j).visited = false;
                }

            }
        }
    }

    public void AllBlankUnivisited()
    {
        int x = GetWidth(), y = GetHeight();
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                GetCell(i, j).visited = !(GetCell(i, j).IsBlank());
            }
        }
    }

    public int GetHeight()
    {
        return map.GetLength(1);
    }
    public int GetWidth()
    {
        return map.GetLength(0);
    }

    public int[] GetEntrCoord()
    {
        return entrCoord;
    }
    public int[] GetExitCoord()
    {
        return exitCoord;
    }
}
