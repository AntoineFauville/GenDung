﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_Data
{
    // [Fight]
    int fightRoomAmount;
    int maxAmountFoes;
    List<EnemyObject> ennemies; // Preset for Foes
    // [Treasure]
    int treasureRoomAmount;
    int maxChestGoldReward;
    int trapPercentage;
    // [Grid]
    List<Vector2> movTiles = new List<Vector2>(); // Movement Tiles
    List<Vector2> eeTiles = new List<Vector2>(); // entrance and exit Tiles
    //
    List<Foe> deadFoes = new List<Foe>();
    List<Player> players = new List<Player>();
    List<Explo_Room> rooms = new List<Explo_Room>();
    //

    public Explo_Data()
    {

    }

    public Explo_Data(int fightRoomAmount, int maxAmountFoes, List<EnemyObject> ennemies, int treasureRoomAmount, int maxChestGoldReward, int trapPercentage, List<Vector2> movTiles, List<Vector2> eeTiles)
    {
        this.fightRoomAmount = fightRoomAmount;
        this.maxAmountFoes = maxAmountFoes;
        this.ennemies = ennemies;
        this.treasureRoomAmount = treasureRoomAmount;
        this.maxChestGoldReward = maxChestGoldReward;
        this.trapPercentage = trapPercentage;
        this.movTiles = movTiles;
        this.eeTiles = eeTiles;
    }

    public int FightRoomAmount
    {
        get
        {
            return fightRoomAmount;
        }

        set
        {
            fightRoomAmount = value;
        }
    }

    public int MaxAmountFoes
    {
        get
        {
            return maxAmountFoes;
        }

        set
        {
            maxAmountFoes = value;
        }
    }

    public List<EnemyObject> Ennemies
    {
        get
        {
            return ennemies;
        }

        set
        {
            ennemies = value;
        }
    }

    public int TreasureRoomAmount
    {
        get
        {
            return treasureRoomAmount;
        }

        set
        {
            treasureRoomAmount = value;
        }
    }

    public int MaxChestGoldReward
    {
        get
        {
            return maxChestGoldReward;
        }

        set
        {
            maxChestGoldReward = value;
        }
    }

    public int TrapPercentage
    {
        get
        {
            return trapPercentage;
        }

        set
        {
            trapPercentage = value;
        }
    }

    public List<Vector2> MovTiles
    {
        get
        {
            return movTiles;
        }

        set
        {
            movTiles = value;
        }
    }

    public List<Vector2> EeTiles
    {
        get
        {
            return eeTiles;
        }

        set
        {
            eeTiles = value;
        }
    }
}