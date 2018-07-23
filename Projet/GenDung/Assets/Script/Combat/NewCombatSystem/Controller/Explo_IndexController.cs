using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_IndexController : MonoBehaviour {

    int fightIndex;
    Explo_FightController explo_Fight;

    public void Start()
    {
        explo_Fight = GameObject.Find("BattleSystem/ScriptBattle").GetComponent<Explo_FightController>();
    }

    public void SetIndexButton()
    {
        explo_Fight.SetTarget(fightIndex);
    }

    public int FightIndex
    {
        get
        {
            return fightIndex;
        }

        set
        {
            fightIndex = value;
        }
    }
}
