using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyGestion : MonoBehaviour {

    public int localMoney;

	// Use this for initialization
	void Start ()
    {
       localMoney = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.PlayerMoney;
    }
	
    public void IncreaseMoney(int addedMoney)
    {
        localMoney += addedMoney;
    }

    public void DecreaseMoney(int decreasedMoney)
    {
        localMoney -= decreasedMoney;
    }

    public void SaveMoney ()
    {
        GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.PlayerMoney = localMoney;
    }

    public void restartResetGame ()
    {
        localMoney = 0;
        GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.PlayerMoney = 0;
        DungeonLoader.Instance.dungeonUnlockedIndex = 1;
    }
}