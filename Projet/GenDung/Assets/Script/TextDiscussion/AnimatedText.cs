using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimatedText : MonoBehaviour
{
	//Time taken for each letter to appear (The lower it is, the faster each letter appear)
	public float letterPaused = 0.005f;
	//Message that will displays till the end that will come out letter by letter
	public string message;
	//Text for the message to display
	public Text textComp;

	public bool AnimDone;

	// Use this for initialization
	void Start ()
	{
		textComp.text = "";
	}

    public void ResetText()
    {
        textComp.text = null;
        textComp.text = "";
    }

    public void EndOfAnimResetText(){
		if (!AnimDone) {
			AnimDone = true;
			textComp.text = null;
			StartCoroutine (TypeText());
		}	
	}

	IEnumerator TypeText()
	{
		//Split each char into a char array
		foreach (char letter in message.ToCharArray()) 
		{
			//Add 1 letter each
			textComp.text += letter;
			yield return new WaitForSeconds(letterPaused);
		}
		AnimDone = false;
	}
}