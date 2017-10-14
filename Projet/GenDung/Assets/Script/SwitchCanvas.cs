using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchCanvas : MonoBehaviour {

	string 
	activeScene,
	previousScene;

	public bool
	didIInstantiate;

	public string[] 
	sceneNames;

	public GameObject[] 
	canvas;

	void Start () {
		activeScene = SceneManager.GetActiveScene ().name;

		previousScene = activeScene;

		//print (activeScene);
		print (SceneManager.GetActiveScene ().name);

		if (!didIInstantiate) {

			for (int i = 0; i < 4; i++) {

				if (activeScene == sceneNames [i]) {
					Instantiate (canvas [i]);
					didIInstantiate = true;
				}
			}
		}
	}

	void Update () {
		activeScene = SceneManager.GetActiveScene ().name;

		if (activeScene != previousScene) {

			didIInstantiate = false;

			if (!didIInstantiate) {

				DestroyImmediate(GameObject.FindGameObjectWithTag ("canvas"));

				for (int i = 0; i < 4; i++) {

					if (activeScene == sceneNames [i]) {
						print (activeScene);
						Instantiate (canvas [i]);
						didIInstantiate = true;
					}
				}

                /* Ajoute de manière dynamique mon DungeonController à l'objet (Pas de modification de scène nécessaire ;) )  */
               /* if (SceneManager.GetActiveScene().name == "Dungeon")
                {
                    this.gameObject.AddComponent<DungeonController>();
                    this.gameObject.AddComponent<MouseController>();
                }*/
                
            }

			if(activeScene == sceneNames [0]){
				DestroyImmediate (GameObject.Find("DontDestroyOnLoad"));
			}

			previousScene = activeScene;
		}

	}
}
