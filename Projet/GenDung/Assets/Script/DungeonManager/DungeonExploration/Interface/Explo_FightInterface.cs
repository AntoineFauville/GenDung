using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explo_FightInterface : MonoBehaviour {

    public static Explo_FightInterface instance;

    public static GameObject interface_NextPanel;
    public static GameObject interface_ScriptBattle;


    public static void Start()
    {
        interface_ScriptBattle = GameObject.Find("BattleSystem/ScriptBattle");
    }

}
