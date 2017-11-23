using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EditorController : MonoBehaviour {

    private static EditorController instance;

    public RoomObject room;
    public SpellObject spell;
    public MovementRangeObject movement;
    private Transform testTile;
    private Vector2 wall;
    private Vector2 spawn;
    private Vector2 monsterSpawn;
    private Vector2 spellRange;
    private Vector2 movementRange;
    private UnitController unit;

    public void Start()
    {
        CreateInstance();
        GameObject.FindGameObjectWithTag("backgroundOfRoom").transform.GetComponent<Image>().sprite = room.back;
        unit = GameObject.Find("Unit(Clone)").transform.Find("Unit").GetComponent<UnitController>();
        testTile = GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_7_6").transform;
        if(SceneManager.GetActiveScene().name == "Editor")
        {
            unit.TileX = 7;
            unit.TileY = 6;
            unit.SetDefaultSpawn(testTile.position);
        }
    }

    void CreateInstance()
    {
        if (instance != null)
        {
            Debug.Log("There should never have two combat controllers.");
        }
        instance = this;
    }

    /* Walls Related Methods */
    public void AddWall(int x, int y)
    {
        wall = new Vector2(x, y);

        if (!room.Walls.Contains(wall))
        {
            room.Walls.Add(wall);
            Debug.Log("Wall has been added : (" + x + "," + y + ")");
        }
        else
            Debug.Log("Non non non, Ce ne sont pas la loutre (" + x + "," + y + ") que vous recherchez ... ");
    }

    public void RemoveWall(int x, int y)
    {
         wall = new Vector2(x, y);

        if (room.Walls.Contains(wall))
        {
            room.Walls.Remove(wall);
            Debug.Log("Wall has been removed : (" + x + "," + y + ")");
        }
        else
            Debug.Log("Cette loutre (" + x + "," + y + ") n'as pas la Force en elle ... ");
    }

    public bool CheckWall(int x, int y)
    {
        Vector2 test = new Vector2(x, y);
        return room.Walls.Contains(test);
    }
    /* */

    /* Spawning Points Related Methods */
    public void AddSpawn(int x, int y)
    {
        spawn = new Vector2(x, y);

        if (!room.SpawningPoints.Contains(spawn))
        {
            room.SpawningPoints.Add(spawn);
            Debug.Log("Spawn has been added : (" + x + "," + y + ")");
        }
        else
            Debug.Log("Non non non, Ce ne sont pas le Spawn loutre (" + x + "," + y + ") que vous recherchez ... ");
    }

    public void RemoveSpawn(int x, int y)
    {
        spawn = new Vector2(x, y);

        if (room.SpawningPoints.Contains(spawn))
        {
            room.SpawningPoints.Remove(spawn);
            Debug.Log("Spawn has been removed : (" + x + "," + y + ")");
        }
        else
            Debug.Log("Ce Spawn de loutre (" + x + "," + y + ") n'as pas la Force en lui ... ");
    }

    public bool CheckSpawn(int x, int y)
    {
        Vector2 test = new Vector2(x, y);
        return room.SpawningPoints.Contains(test);
    }
    /* */

    /* Ennemies Spawning Points Related Methods */
    public void AddMonsterSpawn(int x, int y)
    {
        monsterSpawn = new Vector2(x, y);

        if (!room.MonsterSpawningPoints.Contains(monsterSpawn))
        {
            room.MonsterSpawningPoints.Add(monsterSpawn);
            Debug.Log("Monster Spawn has been added : (" + x + "," + y + ")");
        }
        else
            Debug.Log("Non non non, Ce ne sont pas le Spawn de Castor (" + x + "," + y + ") que vous recherchez ... ");
    }

    public void RemoveMonsterSpawn(int x, int y)
    {
        monsterSpawn = new Vector2(x, y);

        if (room.MonsterSpawningPoints.Contains(monsterSpawn))
        {
            room.MonsterSpawningPoints.Remove(monsterSpawn);
            Debug.Log("Monster Spawn has been removed : (" + x + "," + y + ")");
        }
        else
            Debug.Log("Ce Spawn de Castor (" + x + "," + y + ") n'as pas la Force en lui ... ");
    }

    public bool CheckMonsterSpawn(int x, int y)
    {
        Vector2 test = new Vector2(x, y);
        return room.MonsterSpawningPoints.Contains(test);
    }
    /* */

    /* Spells Related Methods */
    public void AddSpellRange(int x, int y)
    {
        spellRange = new Vector2((x - unit.TileX), (y - unit.TileY));

        if (!spell.range.spellRange.Contains(spellRange))
        {
            spell.range.spellRange.Add(spellRange);
            Debug.Log("Spell Range has been added : (" + x + "," + y + ")");
        }
        else
            Debug.Log("Non non non, Cette Tile (" + x + "," + y + ") fait deja partie de la Range ... ");
    }

    public void RemoveSpellRange(int x, int y)
    {
        spellRange = new Vector2((x - unit.TileX), (y - unit.TileY));

        if (spell.range.spellRange.Contains(spellRange))
        {
            spell.range.spellRange.Remove(spellRange);
            Debug.Log("Spell Range has been removed : (" + x + "," + y + ")");
        }
        else
            Debug.Log("Cette Tile (" + x + "," + y + ") ne fait pas partie de la Range du sort ... ");
    }

    public bool CheckSpellRange(int x, int y)
    {
        Vector2 test = new Vector2((x - unit.TileX), (y - unit.TileY));
        return spell.range.spellRange.Contains(test);
    }
    /* */

    /* Movement Related Methods */
    public void AddMovementRange(int x, int y)
    {
        movementRange = new Vector2((x - unit.TileX), (y - unit.TileY));

        if (!movement.movementRange.Contains(movementRange))
        {
            movement.movementRange.Add(movementRange);
            Debug.Log("Movement Range has been added : (" + x + "," + y + ")");
        }
        else
            Debug.Log("Non non non, Cette Tile (" + x + "," + y + ") fait deja partie de la Range ... ");
    }

    public void RemoveMovementRange(int x, int y)
    {
        movementRange = new Vector2((x - unit.TileX), (y - unit.TileY));

        if (movement.movementRange.Contains(movementRange))
        {
            movement.movementRange.Remove(movementRange);
            Debug.Log("Movement Range has been removed : (" + x + "," + y + ")");
        }
        else
            Debug.Log("Cette Tile (" + x + "," + y + ") ne fait pas partie de la Range  ... ");
    }

    public bool CheckMovementRange(int x, int y)
    {
        Vector2 test = new Vector2((x - unit.TileX), (y - unit.TileY));
        return movement.movementRange.Contains(test);
    }
    /* */
    /* Accessors Methods */
    public static EditorController Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }
}
