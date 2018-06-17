using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Discussion_Controller : MonoBehaviour {

	public DiscussionTemplate DT;
	AnimatedText AT;
	int TextIndex = 0;

    public bool AreWeLooping;

	void Start(){
		AT = this.GetComponent<AnimatedText> ();
		TextIndex --;
	}

	public void SendToAnimText(){

        if (!AT.AnimDone)
        {
            TextIndex++;

            if (TextIndex < DT.Messages.Length)
            {

                //TextIndex = Random.Range (0,DT.Messages.Length);

                AT.message = DT.Messages[TextIndex];

                AT.ResetText();
            }
            else
            {
                AT.message = "";
                AT.ResetText();
            }

            if (TextIndex == DT.Messages.Length)
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
        TextIndex = 0;
    }

}
