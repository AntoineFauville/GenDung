using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Discussion_Controller : MonoBehaviour {

	public DiscussionTemplate DT;
	AnimatedText AT;
	int rnd = 0;

    public bool AreWeLooping;

	void Start(){
		AT = this.GetComponent<AnimatedText> ();
		rnd --;
	}

	public void SendToAnimText(){

        if (!AT.AnimDone)
        {
            rnd++;

            if (rnd < DT.Messages.Length)
            {

                //rnd = Random.Range (0,DT.Messages.Length);

                AT.message = DT.Messages[rnd];

                AT.ResetText();
            }
            else
            {
                AT.message = "";
                AT.ResetText();
            }

            if (rnd == DT.Messages.Length)
            {
                LoopConversation();
            }
        }
	}

    public void LoopConversation()
    {
        if (AreWeLooping) {
            StartCoroutine(loopwait());
        }
    }

    IEnumerator loopwait() {
        yield return new WaitForSeconds(2.0f);
        rnd = 0;
    }

}
