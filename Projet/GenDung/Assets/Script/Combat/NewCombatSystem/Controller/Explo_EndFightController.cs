using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Explo_EndFightController : MonoBehaviour {

    public void EndBattleAllPlayerDead()
    {
        SceneManager.LoadScene("Map");
    }

    public void EndBattleAllMonsterDead()
    {
        if (SceneManager.GetActiveScene().name != "NewCombatTest")
        {
            GameObject.Find("ExploGridPrefab").GetComponent<Explo_Room_FightController>().CleanFinishedFightRoom();
        }
        else
        {
            SceneManager.LoadScene("Init");
        }
    }

    public void CleanUIBattleOrder(List<Foe> foeList, List<Player> playerList)
    {
        for (int i = 0; i < foeList.Count; i++)
        {
            Destroy(foeList[i].EntitiesUIOrder);
            Destroy(foeList[i].EntitiesGO);
        }
        for (int i = 0; i < playerList.Count; i++)
        {
            Destroy(playerList[i].EntitiesUIOrder);
        }
    }
}
