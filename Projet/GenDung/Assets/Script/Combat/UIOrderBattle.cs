using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOrderBattle : MonoBehaviour {

	public void Selected (bool sel){
		if (sel == true) {
			this.transform.parent.transform.localScale = new Vector3 (1.2f, 1.2f, 1f);
		} else {
			this.transform.parent.transform.localScale = new Vector3 (1f, 1f, 1f);
		}
	}
}
