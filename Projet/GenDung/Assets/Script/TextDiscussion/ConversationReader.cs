using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationReader : MonoBehaviour
{
   
    public SpeakerManager SpeakerManager;
    private AnimatedText AnimatedText;
    public Button ButtonToClickNext;
    public Image ButtonToClickNextImage;

    private int AmountOfDiscussion;
    private int AmountOfTextForASpecificParticipant;
   
    [Header("Stay Empty")]
    public Conversation Conversation;
    public int TextForASpecificParticipant;
    public int TalkingParticipant;

    void Start()
    {
        SetButtonToClickNext(false);
    }

    public void SetButtonToClickNext(bool activate)
    {
        ButtonToClickNext.interactable = activate;
        ButtonToClickNextImage.raycastTarget = activate;
    }

    public void DefineConversataion(Conversation conversation)
    {
        Conversation = conversation;
        AmountOfDiscussion = conversation.CharacterMonologueTemplate.Length;
        Debug.Log("Nbr Monologue : " + AmountOfDiscussion);
    }

    public void DefineMonologue(MonologueTemplate monologueTemplate)
    {
        AmountOfTextForASpecificParticipant = monologueTemplate.Messages.Length;
        TextForASpecificParticipant = -1;
        Debug.Log("Nbr Text : " + AmountOfTextForASpecificParticipant);
    }

    public void SetAnimatedText(AnimatedText animatedText)
    {
        AnimatedText = animatedText;
    }

    public void ClickToSeeText()
    {
        if (!AnimatedText.AnimDone)
        {
            TextForASpecificParticipant++;

            if (TextForASpecificParticipant < AmountOfTextForASpecificParticipant)
            {
                AnimatedText.message = Conversation.CharacterMonologueTemplate[TalkingParticipant].Messages[TextForASpecificParticipant];
                AnimatedText.EndOfAnimResetText();
            }
            else
            {
                AnimatedText.message = "";
                AnimatedText.EndOfAnimResetText();
                ChangeParticipant();
            }
        }
    }

    void ChangeParticipant()
    {
        TalkingParticipant++;
        Debug.Log("Participant suivant");
        if (TalkingParticipant < AmountOfDiscussion)
        {
            SpeakerManager.ChangeParticipant(Conversation.CharacterMonologueTemplate[TalkingParticipant]);
            DefineMonologue(Conversation.CharacterMonologueTemplate[TalkingParticipant]);
        }
        else
        {
            SetButtonToClickNext(false);
            SpeakerManager.ResetAllSpeakerText();
            SpeakerManager.UiConversation.StartCadre(false);
        }
    }
}
