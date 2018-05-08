using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_DungeonController : MonoBehaviour {

    GameObject dontDestroyOnLoad;
    MapController map;
    DungeonLoader dngLoader;
    ExploMap presetDungeon;

    Explo_Data dungeonData;
    Explo_Dungeon dungeon = new Explo_Dungeon();

    public void Start()
    {
        dontDestroyOnLoad = GameObject.Find("DontDestroyOnLoad");
        map = dontDestroyOnLoad.GetComponent<MapController>();
        dngLoader = dontDestroyOnLoad.GetComponent<DungeonLoader>();
        presetDungeon = dngLoader.exploDungeonList.explorationDungeons[map.dungeonIndex];

        dungeonData = new Explo_Data(presetDungeon.fightRoomAmount, presetDungeon.enemyMax, presetDungeon.enemiesList, presetDungeon.treasureRoomAmount, presetDungeon.chestGoldRewardMax, presetDungeon.percentage, presetDungeon.movTiles, presetDungeon.eeTiles);
        dungeon.Data = dungeonData;

        Debug.Log("Dungeon has been created " + dungeonData.TrapPercentage);
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
