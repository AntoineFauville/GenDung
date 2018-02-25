using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExploEditorGestion : MonoBehaviour {


	public void StartExplo()
	{
		SceneManager.LoadScene("Explo");
	}

	public void StartExploEditor()
	{
		SceneManager.LoadScene("ExploEditor");
	}
}
