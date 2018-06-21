using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationReader : MonoBehaviour
{
    public Conversation Conversation;
    public SpeakerManager SpeakerManager;
    private AnimatedText AnimatedText;
    public Button ButtonToClickNext;

    private int AmountOfDiscussion;
    private int AmountOfTextForASpecificParticipant;
    private int TextForASpecificParticipant;
    private int TalkingParticipant;

    void Start()
    {
        ButtonToClickNext.interactable = false;
        TalkingParticipant = 0;
        DefineConversation(Conversation.CharacterMonologueTemplate[TalkingParticipant]);
        AmountOfDiscussion = Conversation.CharacterMonologueTemplate.Length;
        //make sure the first text is the 0;
    }

    void DefineConversation(MonologueTemplate monologueTemplate)
    {
        AmountOfTextForASpecificParticipant = monologueTemplate.Messages.Length;
        TextForASpecificParticipant = -1;
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
                AnimatedText.ResetText();
            }
            else
            {
                AnimatedText.message = "";
                AnimatedText.ResetText();
                ChangeParticipant();
            }
        }
    }

    void ChangeParticipant()
    {
        TalkingParticipant++;
        DefineConversation(Conversation.CharacterMonologueTemplate[TalkingParticipant]);
    }
}
