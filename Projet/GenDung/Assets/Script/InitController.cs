using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitController : MonoBehaviour {

	
        public void Start()
    {
#if UNITY_EDITOR

#else
        print("Explosion !!!!!!!!!!!!!!");
        GameObject.Find("CanvasInitDebug").SetActive(false);
        Instantiate(Resources.Load("UI_Interface/DontDestroyOnLoad")).name = "DontDestroyOnLoad";
        SceneManager.LoadScene ("MainMenu");
#endif
    }

    public void StartGame()
    {
        Instantiate(Resources.Load("UI_Interface/DontDestroyOnLoad")).name = "DontDestroyOnLoad";
        SceneManager.LoadScene("MainMenu");
    }

    public void StartEditor()
    {
        SceneManager.LoadScene("Editor");
    }

	public void StartExploEditor()
	{
		SceneManager.LoadScene("ExploEditor");
	}
}
