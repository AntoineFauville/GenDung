using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonLoader : MonoBehaviour {

	string 
	activeScene;

	public string map,dungeon;

	public bool
	didIInstantiate;

	// Use this for initialization
	void Start () {
		activeScene = SceneManager.GetActiveScene ().name;
	}
	
	// Update is called once per frame
	void Update () {
		if (activeScene == dungeon) {

		}
	}

	void changeDungeonIndex () {
		
	}

	void LoadRoom () {
		
	}
}
