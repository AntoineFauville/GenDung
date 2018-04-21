using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogGestionTool : MonoBehaviour {

    /* TODO : */
    // Clean the log 

	private List<string> LogList = new List<string> ();

	string textDisplay = "";

	public int maxLines = 4;

	void Start () {
		StartCoroutine ("checkDisplay");
	}

	IEnumerator checkDisplay () {
		yield return new WaitForSeconds (0.1f);

		if (SceneManager.GetActiveScene ().name == "Explo") {
			//wait for loading the maps
			yield return new WaitForSeconds (0.2f);
			if (SceneManager.GetActiveScene ().name == "Explo") {
				//print ("hey");
				//GameObject.Find ("textDisplayLog").GetComponent<Text> ().text = textDisplay;
				GameObject.Find ("textDisplayLogExplo").GetComponent<Text> ().text = textDisplay;
			}
		}
		StartCoroutine ("checkDisplay");
	}

	public void AddLogLine (string Log){
	
		LogList.Add (Log);

		if (LogList.Count >= maxLines) {
			LogList.RemoveAt (0);
		}

		textDisplay = "";

		foreach (string log in LogList) 
		{
			textDisplay += "~ " + log;
			textDisplay += "\n";
		}
	}
}
