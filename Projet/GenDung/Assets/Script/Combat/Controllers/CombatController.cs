using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour {

    private static CombatController instance;
    private bool placementDone = false;

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
    }

    /* Code de gestion du placement des personnages Pré-Combat*/

    public void PlaceCharacter()
    {
        placementDone = true;
    }

    /* Code de gestion de l'Initiative des personnages */



    /* Code de gestion du début de combat */

    public void CombatBeginning()
    {

    }

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
