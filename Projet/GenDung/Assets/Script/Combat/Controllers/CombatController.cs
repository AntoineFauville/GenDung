using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour {

    private static CombatController instance;
    public bool placementDone = false;

    void CreateInstance()
    {
        if (instance != null)
        {
            Debug.Log("There should never have two combat controllers.");
        }
        instance = this;
    }

    public void Start()
    {
        CreateInstance();
        Debug.Log("Combat will begin soon (if i want it to work soon) ");
    }

    /* Code de gestion du placement des personnages Pré-Combat*/



    /* Code de gestion de l'Initiative des personnages */



    /* Code de gestion du début de combat */



    /* Code de gestion de fin de combat */



    /* Accessors Methods */
    public static CombatController Instance
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

    public bool PlacementDone
    {
        get
        {
            return placementDone;
        }

        set
        {
            placementDone = value;
        }
    }
    /**/
}
