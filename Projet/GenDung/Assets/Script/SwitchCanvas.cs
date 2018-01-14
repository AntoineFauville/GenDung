using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

	void Start ()
    {
		activeScene = SceneManager.GetActiveScene ().name;

		previousScene = activeScene;

		if (!didIInstantiate) {

			for (int i = 0; i < 4; i++) {

				if (activeScene == sceneNames [i]) {
					GameObject Can = canvas [i];
					Instantiate (Can);
					didIInstantiate = true;
				}
			}
		}
	}

	void Update ()
    {
		activeScene = SceneManager.GetActiveScene ().name;

		if (activeScene != previousScene)
        {
			didIInstantiate = false;

			if (!didIInstantiate)
            {
				DestroyImmediate(GameObject.FindGameObjectWithTag ("canvas"));
				for (int i = 0; i < 4; i++)
                {
					if (activeScene == sceneNames [i])
                    {
						print (activeScene);
						Instantiate (canvas [i]);
						didIInstantiate = true;
					}
				}
            }

			if(activeScene == sceneNames [0])
            {
				DestroyImmediate (GameObject.Find("DontDestroyOnLoad"));

				#if UNITY_EDITOR
				DestroyImmediate (GameObject.FindGameObjectWithTag("DebugCanvas"));

				Instantiate(Resources.Load("UI_Interface/DebugCanvas"));

				GameObject.Find("IncreaseUnlockIndexButton").GetComponent<Button> ().onClick.AddListener (GameObject.Find("DontDestroyOnLoad").GetComponent<MapController>().UnlockNextDungeon);
				GameObject.Find("DecreaseUnlockIndexButton").GetComponent<Button> ().onClick.AddListener (GameObject.Find("DontDestroyOnLoad").GetComponent<MapController>().DecreaseUnlockDungeonIndex);
				GameObject.Find("ResetUnlockIndexButton").GetComponent<Button> ().onClick.AddListener (GameObject.Find("DontDestroyOnLoad").GetComponent<MapController>().ResetUnlockDungeonIndex);
				#endif
			}
			previousScene = activeScene;
		}
	}
}
