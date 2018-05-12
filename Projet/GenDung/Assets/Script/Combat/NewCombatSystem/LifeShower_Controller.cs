using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeShower_Controller : MonoBehaviour {

	float min;

	void Start(){
		min = 1.5f;
		this.GetComponent<CanvasGroup> ().alpha = 1.0f;
		this.GetComponent<RectTransform> ().localScale = new Vector3 (min,min,1.0f);

	}

	public void ExtraScaleForCrit(bool crit){
		if (crit) {
			min = 5.0f;
			this.GetComponent<RectTransform> ().localScale = new Vector3 (min,min,1.0f);
		}
	}

	void Update () {
		if (min > 1) {
			min -= 0.03f;
		} else {
			min = 1.0f;
		}

		this.GetComponent<RectTransform> ().localScale = new Vector3 (min,min,1.0f);

		this.GetComponent<RectTransform> ().anchoredPosition += new Vector2 (0,0.5f);

		this.GetComponent<CanvasGroup> ().alpha -= 0.01f;

		if (this.GetComponent<CanvasGroup> ().alpha <= 0) {
			this.gameObject.SetActive (false);
		}
	}
}
