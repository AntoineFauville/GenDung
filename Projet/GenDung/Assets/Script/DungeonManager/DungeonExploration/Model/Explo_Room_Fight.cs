using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_Room_Fight : Explo_Room
{
    int monstersAmount;
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
        monstersAmount = foesAmount;

        for (int i = 0; i < foesAmount; i++)
        {
            EnemyObject selectedFoePreset = foesPreset[Random.Range(0, foesPreset.Count)];
            Foe createdFoe = new Foe(selectedFoePreset.health,selectedFoePreset.name, selectedFoePreset.enemyIcon);
            foesList.Add(createdFoe);
            Debug.Log("Foe Created: " + createdFoe.Name);
        }

        Debug.Log("Populating of foesList has finished for room number " + base.Index + ", Thanks for waiting");

    }

    public int MonstersAmount
    {
        get
        {
            return monstersAmount;
        }
        set
        {
            monstersAmount = value;
        }
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
