using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationReader : MonoBehaviour
{
    public Conversation Conversation;
    public AnimatedText AnimatedText;
    Vector3 PositionOfTheText = new Vector3();

    private int AmountOfDiscussion;
    private int AmountOfTextForASpecificParticipant;
    private int TextForASpecificParticipant;
    private int Participant;

    void Start()
    {
        Participant = 0;
        DefinePosition(Participant);
        DefineConversation(Participant);

        //make sure the first text is the 0;
    }

    void DefineConversation(int Participant)
    {
        AmountOfDiscussion = Conversation.AmountCharacterAndText;
        AmountOfTextForASpecificParticipant = Conversation.CharacterAndText[Participant].AmountOfMessages;
        TextForASpecificParticipant = -1;
    }

    void DefinePosition(int Participant)
    {
        PositionOfTheText = Conversation.CharacterAndText[Participant].CharacterPositionPrefab.transform.localPosition;
        
        AnimatedText.transform.position = PositionOfTheText;
    }

    public void ClickToSeeText()
    {
        if (!AnimatedText.AnimDone)
        {
            TextForASpecificParticipant++;

            if (TextForASpecificParticipant < AmountOfTextForASpecificParticipant)
            {
                AnimatedText.message = Conversation.CharacterAndText[Participant].Messages[TextForASpecificParticipant];
                AnimatedText.ResetText();
            }
            else
            {
                AnimatedText.message = "";
                AnimatedText.ResetText();
                ChangeParticipant();
            }
            //else
            //{
            //    //change participant.
            //    Participant++;
            //    DefinePosition(Participant);
            //    //teleport the thing.
            //    DefineConversation(Participant);
            //    AnimatedText.ResetText();
            //}
        }
    }

    void ChangeParticipant()
    {
        Participant++;
        DefineConversation(Participant);
        DefinePosition(Participant);
    }
}
