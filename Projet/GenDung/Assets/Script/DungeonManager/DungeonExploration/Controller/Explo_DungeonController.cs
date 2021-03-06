﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_DungeonController : MonoBehaviour {

    GameObject dontDestroyOnLoad;
    MapController map;
    DungeonLoader dngLoader;
    ExploMap presetDungeon;
    GameData saveData;

    Explo_Data dungeonData;
    Explo_Dungeon dungeon = new Explo_Dungeon();

    public void Start()
    {
        dontDestroyOnLoad = GameObject.Find("DontDestroyOnLoad"); // Link to DontDestroyOnLoad Object.
        map = dontDestroyOnLoad.GetComponent<MapController>(); // reference to the MapController for DungeonIndex.
        dngLoader = dontDestroyOnLoad.GetComponent<DungeonLoader>(); // reference to the DungeonLoader for DungeonPreset.
        presetDungeon = dngLoader.exploDungeonList.explorationDungeons[map.dungeonIndex]; // Actual preset for the DungeonIndex.
        saveData = dontDestroyOnLoad.GetComponent<SavingSystem>().gameData; // reference to the GameData Object contain in SavingSystem.
        // Populating Explo_Data model with previously loaded preset.
        dungeonData = new Explo_Data(presetDungeon.fightRoomAmount, presetDungeon.enemyMax, presetDungeon.enemiesList, presetDungeon.roomsBackground, presetDungeon.treasureRoomAmount, presetDungeon.chestGoldRewardMax, presetDungeon.percentage, presetDungeon.movTiles, presetDungeon.eeTiles);
        dungeon.Data = dungeonData; // Linking the Explo_dungeon and the Explo_data Models.
        GatherPlayers();
    }

    public void GatherPlayers()
    {
        for (int i = 0; i < saveData.SavedSizeOfTheTeam; i++)
        {
            GameObject player_GO = GameObject.Find("BattleSystem/BattleSystem/Player " + i);

            Player createdPlayer = new Player(saveData.SavedCharacterList[i].Health_PV, saveData.SavedCharacterList[i].ActionPoints_PA, saveData.SavedCharacterList[i].Initiative, saveData.SavedCharacterList[i].Name, dungeonData, saveData.SavedCharacterList[i].SpellList, player_GO, saveData.SavedCharacterList[i].ICON);
            createdPlayer.InitializeVisual();
            dungeon.Data.Players.Add(createdPlayer);
            Debug.Log("Created Player : " + Dungeon.Data.Players[i].Name + " with " + Dungeon.Data.Players[i].Health + " HP and " + Dungeon.Data.Players[i].ActionPoint + " PA ");
        }

        for (int i = 0; i < dungeon.Data.Players.Count; i++)
        {
            if (!dungeon.Data.Players[i].Dead)
                Dungeon.Data.PlayersLeft++; // Setting the number of Players left to 4
        }
        Debug.Log(Dungeon.Data.PlayersLeft);
    }

    public Explo_Dungeon Dungeon
    {
        get
        {
            return dungeon;
        }

        set
        {
            dungeon = value;
        }
    }
}
