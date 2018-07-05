using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explo_FightInterface : MonoBehaviour {

    public static Explo_FightInterface instance;

    public static GameObject interface_NextPanel;
    public static GameObject interface_ScriptBattle;


    public void Start()
    {
        CreateInstance();
        interface_ScriptBattle = GameObject.Find("BattleSystem/ScriptBattle");
    }

    void CreateInstance()
    {
        if (instance != null)
        {
            Debug.Log("There should never have two Explo_Interface controllers.");
        }
        instance = this;
    }
}
