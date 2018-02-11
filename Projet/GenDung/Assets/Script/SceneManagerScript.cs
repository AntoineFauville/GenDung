using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {

	public GameData gameData;

	public void ContinueGameLoadMap () {
        GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().FadeInOutAnim();
        SceneManager.LoadScene ("Map");
		GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().dungeonUnlockedIndex = gameData.DungeonIndexData;
	}

	public void NewGameLoadMap () {
        GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().FadeInOutAnim();
        SceneManager.LoadScene ("CharacterCreation");
		GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.DungeonIndexData = 1;
        DungeonLoader.Instance.dungeonUnlockedIndex = 1;
        TavernController.Instance.QuestStartOn = true;
        GameObject.Find("DontDestroyOnLoad").GetComponent<CurrencyGestion>().ResetMoney();
        // Reset SaveData 
    }

    public void LoadDungeon () {

        GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().FadeInOutAnim();
        SceneManager.LoadScene ("Dungeon");
	}
	public void LoadMarket () {
        GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().FadeInOutAnim();
        SceneManager.LoadScene ("Tavern");
	}
	public void LoadMainMenu () {
        GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().FadeInOutAnim();
        SceneManager.LoadScene ("MainMenu");
	}

    public void CloseGame ()
    {
        Debug.Log("yep we are in editor, doesnt work...");
        Application.Quit();
    }
}
