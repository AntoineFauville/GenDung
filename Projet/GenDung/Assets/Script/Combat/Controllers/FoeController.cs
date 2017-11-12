using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoeController : MonoBehaviour {

    private string foeName;
    public int foeHealth,foePA,foePM,foeAtk;

	public void SetDefaultSpawn(Vector3 pos)
    {
        this.transform.position = pos;
    }

    public void FoeClicked()
    {
        if (Input.GetMouseButtonUp(0))
            Debug.Log("Hello my name is : " +foeName+ ". I have got: " + foeHealth+ " PV, " + foePA + " PA, " + foePM + " PM, " + foeAtk + " ATK.");
    }

    /* Accessors Methods */
    public string FoeName
    {
        get
        {
            return foeName;
        }
        set
        {
            foeName = value;
        }
    }
    public int FoeHealth
    {
        get
        {
            return foeHealth;
        }
        set
        {
            foeHealth = value;
        }
    }
    public int FoePA
    {
        get
        {
            return foePA;
        }
        set
        {
            foePA = value;
        }
    }
    public int FoePM
    {
        get
        {
            return foePM;
        }
        set
        {
            foePM = value;
        }
    }
    public int FoeAtk
    {
        get
        {
            return foeAtk;
        }
        set
        {
            foeAtk = value;
        }
    }
    /*  */
}
