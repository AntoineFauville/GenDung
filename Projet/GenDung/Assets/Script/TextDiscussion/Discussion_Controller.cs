using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Discussion_Controller : MonoBehaviour {

	public DiscussionTemplate DT;
	AnimatedText AT;
	int rnd = 0;

	void Start(){
		AT = this.GetComponent<AnimatedText> ();
		rnd --;
	}

	public void SendToAnimText(){
		
		rnd++;

		if (rnd < DT.Messages.Length) {
			
			//rnd = Random.Range (0,DT.Messages.Length);

			AT.message = DT.Messages [rnd];

			AT.ResetText ();
		} else {
			AT.message = "";
			AT.ResetText ();
		}
	}

}
