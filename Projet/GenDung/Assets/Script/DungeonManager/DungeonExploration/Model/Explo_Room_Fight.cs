using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_Room_Fight : Explo_Room
{
    List<Foe> foesList = new List<Foe>();
    bool boss;
    bool overPowered;

    public Explo_Room_Fight(int _index, Explo_Dungeon _dungeon)
    {
        base.Index = _index;
        base.Dungeon = _dungeon;
    }

    public Explo_Room_Fight(bool _boss, bool _overPowered)
    {
        this.boss = _boss;
        this.overPowered = _overPowered;
    }

    public void PopulateFoesList(List<EnemyObject> foesPreset, int foesAmount)
    {
        Debug.Log("Populating of foesList for room number " + base.Index + " has started");
        base.MonstersAmount = foesAmount;

        for (int i = 0; i < foesAmount; i++)
        {
            EnemyObject selectedFoePreset = foesPreset[Random.Range(0, foesPreset.Count)];
            GameObject foe_GO = GameObject.Find("BattleSystem/BattleSystem/EnemyPanelPlacement/Enemy " + i);

            Foe createdFoe = new Foe(selectedFoePreset.health, selectedFoePreset.initiative, selectedFoePreset.atk, selectedFoePreset.name, Index, base.Dungeon.Data, foe_GO, selectedFoePreset.enemyIcon, selectedFoePreset.enemyAnimator);
            foesList.Add(createdFoe);
            Debug.Log("Foe Created: " + createdFoe.Name);
        }

        Debug.Log("Populating of foesList has finished for room number " + base.Index + ", Thanks for waiting");

    }

    public List<Foe> FoesList
    {
        get
        {
            return foesList;
        }

        set
        {
            foesList = value;
        }
    }
}
