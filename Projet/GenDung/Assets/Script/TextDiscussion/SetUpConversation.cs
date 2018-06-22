using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpConversation : MonoBehaviour
{

    public int StoryCase;

    public Conversation Setup1IntroBarMan;
    public Conversation Setup1IntroKesath;

    public Conversation Setup2IntroBarMan;
    public Conversation Setup2IntroKesath;

    public SpeakerManager SpeakerManager;
    public UIConversation UiConversation;

    private void Start()
    {
        StoryCase = 1;
        ReadStoryCase();
    }

    private void ReadStoryCase()
    {
        switch (StoryCase)
        {
            case 1:
                Debug.Log("First Time you enter the tavern");

                //set the possible conversation buttons.
                //set according to those button the conversation with it
                SpeakerManager.KesathConversation = Setup1IntroKesath;
                UiConversation.PlayQuestMarkerAnim(1);
                SpeakerManager.BarManConversation = Setup1IntroBarMan;
                UiConversation.PlayQuestMarkerAnim(2);
                UiConversation.ResetCadre();


                break;
            case 2:
                Debug.Log("Second Time you enter the tavern");

                //set the possible conversation buttons.
                //set according to those button the conversation with it
                SpeakerManager.KesathConversation = Setup2IntroKesath;
                SpeakerManager.BarManConversation = Setup2IntroBarMan;
                UiConversation.PlayQuestMarkerAnim(2);
                UiConversation.ResetCadre();

                break;
            default:
                Debug.Log("unknown case");
                break;
        }
    }
}
