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
		GameObject.Find ("DontDestroyOnLoad").GetComponent<DungeonLoader> ().dungeonUnlockedIndex = 1;
        GameObject.Find("DontDestroyOnLoad").GetComponent<CurrencyGestion>().restartResetGame();

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
}
