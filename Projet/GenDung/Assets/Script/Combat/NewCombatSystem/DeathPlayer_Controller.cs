using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlayer_Controller : MonoBehaviour {

	public GameObject[] Parts;

	public void Start() {
		for (int i = 0; i < Parts.Length; i++) {
			Parts [i].GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Kinematic;

			Parts [i].GetComponent<SpringJoint2D> ().enabled = false;
		}
	}

	public void death(){
		StartCoroutine (deathAnim());
	}

	IEnumerator deathAnim(){

		for (int i = 0; i < Parts.Length; i++) {
			Parts [i].GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;

			Parts [i].GetComponent<SpringJoint2D> ().enabled = true;
		}

		yield return new WaitForSeconds (5.0f);

		for (int i = 0; i < Parts.Length; i++) {
			Parts [i].GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Static;

			Parts [i].GetComponent<SpringJoint2D> ().enabled = false;
		}
	}
}
