using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_Data : MonoBehaviour {

    // For Every Character
        // Health
    // Gold
    // Defeated Ennemy 

    public int exploHealth;
    public int exploGold = 0;
    public List<Object> exploEnnemy = new List<Object>(); 

	void Start ()
    {

	}
	
    public void ModifyGold(int value)
    {
        exploGold += value;
    }

    public void SendToSave()
    {
        GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.PlayerMoney += exploGold;
    }
}
