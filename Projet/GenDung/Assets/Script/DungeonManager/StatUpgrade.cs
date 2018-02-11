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

    void Update()
    {
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

        if (GoldCostUp <= GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.PlayerMoney)
        {
            GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.PlayerMoney -= GoldCostUp;

            GoldCostUp = 0;
            HealthCost = 0;
            PACost = 0;
            CACCost = 0;
            DistCost = 0;

            GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[0].Health_PV = TavernController.Instance.HealthTemp;
            GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[0].ActionPoints_PA = TavernController.Instance.ActionTemp;
            GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[0].CloseAttaqueValue = TavernController.Instance.CACTemp;
            GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[0].DistanceAttaqueValue = TavernController.Instance.DistTemp;
        }
    }

    //---------health---------//
    public void HealthUp() {
        TavernController.Instance.HealthTemp++;
        HealthCost += 3;
    }

    public void HealthDown()
    {
        if (TavernController.Instance.HealthTemp > 0 && HealthCost > 0)
        {
            TavernController.Instance.HealthTemp--;
            HealthCost -= 3;
        }
    }

    //---------PA---------//
    public void PAUp()
    {

        TavernController.Instance.ActionTemp++;
        PACost += 5;
    }

    public void PADown()
    {
        if (TavernController.Instance.ActionTemp > 0 && PACost > 0)
        {
            TavernController.Instance.ActionTemp--;
            PACost -= 5;
        }
    }

    //---------CAC ATTACK---------//
    public void CACUp()
    {
        TavernController.Instance.CACTemp++;
        CACCost += 2;
    }

    public void CACDown()
    {
        if (TavernController.Instance.CACTemp > 0 && CACCost > 0)
        {
            TavernController.Instance.CACTemp--;
            CACCost -= 2;
        }
    }

    //---------Distance ATTACK---------//
    public void DistUp()
    {
        TavernController.Instance.DistTemp++;
        DistCost += 2;
    }

    public void DistDown()
    {
        if (TavernController.Instance.DistTemp > 0 && DistCost > 0)
        {
            TavernController.Instance.DistTemp--;
            DistCost -= 2;
        }
    }
}
