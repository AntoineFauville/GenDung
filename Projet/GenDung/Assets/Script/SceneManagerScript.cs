using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {

	public void LoadMap () {
		SceneManager.LoadScene ("Map");
	}
	public void LoadDungeon () {
		SceneManager.LoadScene ("Dungeon");
	}
	public void LoadMarket () {
		SceneManager.LoadScene ("Market");
	}
	public void LoadMainMenu () {
		SceneManager.LoadScene ("MainMenu");
	}
}
