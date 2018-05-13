using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentSizeFitterChildren : MonoBehaviour {

	void Update () {

		if (this.transform.GetChild (0).GetComponent<Text> ().text != "") {
			this.GetComponent<RectTransform> ().sizeDelta = this.transform.GetChild (0).GetComponent<RectTransform> ().sizeDelta + new Vector2 (5, 0);
		} else {
			this.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0,0);
		}
	}
}
