using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLooseLife_Controller : MonoBehaviour {

	public void lifeTextUp (int amount, bool crit) {

		Transform t;
		t = this.transform.GetChild (0).transform.GetChild (3);

		GameObject lifeUp;

		lifeUp = Instantiate (Resources.Load ("UI_Interface/LifeShower"), t) as GameObject;

		lifeUp.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 0);

		lifeUp.GetComponent<LifeShower_Controller> ().ExtraScaleForCrit (crit);

		if (amount < 0) {
			lifeUp.transform.GetChild (0).GetComponent<Text> ().text = amount.ToString ();
		} else {
			lifeUp.transform.GetChild (0).GetComponent<Text> ().text = "+" + amount.ToString ();
		}
	}
}
