using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUpgrade : MonoBehaviour {

    public int 
        GoldCostUp = 0,
        HealthCost = 0,
        PACost = 0,
        CACCost = 0,
        DistCost = 0;
    

    void Update() {
        GameObject.Find("GoldCostUp").GetComponent<Text>().text = "Total Cost : " + GoldCostUp;
        GoldCostUp = HealthCost + PACost + CACCost + DistCost;
    }

    public void PAY() {
        if (GoldCostUp > GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.PlayerMoney) {
            Debug.Log("ouch not enought money bro");
        }

        if (GoldCostUp == GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.PlayerMoney)
        {
            Debug.Log("what are you trying to do bro");
        }

        if (GoldCostUp < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.PlayerMoney)
        {
            GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.PlayerMoney -= GoldCostUp;

            GoldCostUp = 0;

            GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[0].Health_PV = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().healthTemp;
            GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[0].ActionPoints_PA = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().ActionTemp;
            GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[0].CloseAttaqueValue = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().CACTemp;
            GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[0].DistanceAttaqueValue = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().DistTemp;
        }
    }

    //---------health---------//
    public void HealthUp() {
        GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().healthTemp++;
        HealthCost += 3;
    }

    public void HealthDown()
    {
        if (GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().healthTemp > 0 && HealthCost > 0)
        {
            GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().healthTemp--;
            HealthCost -= 3;
        }
    }

    //---------PA---------//
    public void PAUp()
    {
        
        GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().ActionTemp++;
        PACost += 5;
    }

    public void PADown()
    {
        if (GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().ActionTemp > 0 && PACost > 0)
        {
            GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().ActionTemp--;
            PACost -= 5;
        }
    }

    //---------CAC ATTACK---------//
    public void CACUp()
    {
        GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().CACTemp++;
        CACCost += 2;
    }

    public void CACDown()
    {
        if (GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().CACTemp > 0 && CACCost > 0)
        {
            GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().CACTemp--;
            CACCost -= 2;
        }
    }

    //---------Distance ATTACK---------//
    public void DistUp()
    {
        GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().DistTemp++;
        DistCost += 2;
    }

    public void DistDown()
    {
        if (GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().DistTemp > 0 && DistCost > 0)
        {
            GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().DistTemp--;
            DistCost -= 2;
        }
    }
}
