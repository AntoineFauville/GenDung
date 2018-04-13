using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleImage : MonoBehaviour {

    public int size;

	void Start () {
        size = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedSizeOfTheTeam;
	}
	
	void Update () {

        size = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedSizeOfTheTeam;

        if (size == 1)
        {
            this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(140,149);
        }
        else if (size == 2)
        {
            this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(237, 149);
        }
        else if (size == 3)
        {
            this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(335, 149);
        }
        else if (size == 4)
        {
            this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(434, 149);
        }
        else
        {

        }	
	}
}
