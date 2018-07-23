using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Explo_EndFightController : MonoBehaviour {

    public void EndBattleAllPlayerDead()
    {
        StartCoroutine(WaitBeforeEnd());
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
            StartCoroutine(WaitBeforeEnd());
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

    public IEnumerator WaitBeforeEnd()
    {
        yield return new WaitForSeconds(0.75f);
    }
}
