using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMonster{
    public string name;
    public int threatlvl;

    private bool isTrap;

    public GMonster(string n, int t, bool trap)
    {
        name = n;
        threatlvl = t;
        isTrap = trap;
    }
}
