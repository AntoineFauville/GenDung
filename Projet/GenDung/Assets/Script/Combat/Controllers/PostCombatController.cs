using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostCombatController : MonoBehaviour {
//
//    private static PostCombatController instance;
//    private List<GameObject> spellCanvasInstantiated = new List<GameObject>();
//
//    void CreateInstance()
//    {
//        if (instance != null)
//        {
//            Debug.Log("There should never have two PostCombatControllers.");
//        }
//        instance = this;
//    }
//
//    void Start ()
//    {
//        CreateInstance();
//	}
//
//    public void CleanEndBattle()
//    {
//        // Clean Battle Display : 'UIDisplayPlayer_x' and 'UIDisplayMonster_x'
//		for (int m = 0; m < PreCombatController.Instance.MonsterAmount; m++)
//        {
//            if (GameObject.Find("CanvasUIDungeon").transform.Find("OrderOfBattle/OrderBattlePanel/UIDisplayFoe_" + m) != null)
//                Destroy(GameObject.Find("CanvasUIDungeon").transform.Find("OrderOfBattle/OrderBattlePanel/UIDisplayFoe_" + m).gameObject);
//
//            if (GameObject.Find("Foe_" + m) != null)
//                Destroy(GameObject.Find("Foe_" + m).gameObject);
//        }
//
//        for (int j = 0; j < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedSizeOfTheTeam; j++)
//        {
//            if (GameObject.Find("CanvasUIDungeon").transform.Find("OrderOfBattle/OrderBattlePanel/UIDisplayCharacter_" + j) != null)
//                Destroy(GameObject.Find("CanvasUIDungeon").transform.Find("OrderOfBattle/OrderBattlePanel/UIDisplayCharacter_" + j).gameObject);
//
//            if (GameObject.Find("Character_" + j) != null)
//                Destroy(GameObject.Find("Character_" + j).gameObject);
//
//        }
//        // Clean 'SpellCanvas(Clone)' from Hierarchy
//
//        for (int s = 0; s < spellCanvasInstantiated.Count; s++)
//        {
//            Destroy(spellCanvasInstantiated[s]);
//        }
//    }
//
//    public void EndBattle()
//    {
//        //CleanEndBattle();
//        //CombatUIController.Instance.SwitchStartVisual();
//
//		GameObject.Find("FightRoomUI").transform.Find("ScriptManagerFightRoomUI").GetComponent<CombatGestion>().FinishedCombat();
//    }
//
//    /* Accessors Methods */
//    public static PostCombatController Instance
//    {
//        get
//        {
//            return instance;
//        }
//
//        set
//        {
//            instance = value;
//        }
//    }
//    public List<GameObject> SpellCanvasInstantiated
//    {
//        get
//        {
//            return spellCanvasInstantiated;
//        }
//        set
//        {
//            spellCanvasInstantiated = value;
//        }
//    }
}
