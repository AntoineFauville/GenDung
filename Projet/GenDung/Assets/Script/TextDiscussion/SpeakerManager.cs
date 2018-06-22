using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakerManager : MonoBehaviour
{
    public AnimatedText KesathAnimatedText;
    public AnimatedText BarManAnimatedText;
    public AnimatedText[] PlayerAnimatedText;
    
    public ConversationReader ConversationReader;
    public UIConversation UiConversation;

    AnimatedText animatedText;

    [Header("Stay Empty")]
    public Conversation KesathConversation;
    public Conversation BarManConversation;

    private void Start()
    {
        ResetAllSpeakerText();
    }

    public void StartConversation(int speaker)
    {
        ResetAllSpeakerText();
        
        switch (speaker)
        {
            case 0:
                //kesath starts a conversation
                //return then kesath animated text
                animatedText = KesathAnimatedText;
                ConversationReader.DefineConversataion(KesathConversation);
                ConversationReader.DefineMonologue(KesathConversation.CharacterMonologueTemplate[0]);
                ConversationReader.TextForASpecificParticipant = -1;
                ConversationReader.TalkingParticipant = 0;
                UiConversation.DisableQuestMarkerAnim(1);
                UiConversation.StartCadre(true);
                Debug.Log("Kesath Talking");
                break;
            case 1:
                //BarMan starts a conversation
                //return then BarMan animated text
                animatedText = BarManAnimatedText;
                ConversationReader.DefineConversataion(BarManConversation);
                ConversationReader.DefineMonologue(BarManConversation.CharacterMonologueTemplate[0]);
                ConversationReader.TextForASpecificParticipant = -1;
                ConversationReader.TalkingParticipant = 0;
                UiConversation.DisableQuestMarkerAnim(2);
                UiConversation.StartCadre(true);
                Debug.Log("BarMan Talking");
                break;
            case 2:
                //Players starts a conversation
                //return then kesath animated text
                int rnd = Random.Range(1, PlayerAnimatedText.Length);
                animatedText = PlayerAnimatedText[rnd];
                Debug.Log("Player Talking");
                break;
            default:
                break;
        }

        ConversationReader.SetButtonToClickNext(true);

        ConversationReader.SetAnimatedText(animatedText);
    }

    public void ResetAllSpeakerText()
    {
        KesathAnimatedText.ResetText();
        BarManAnimatedText.ResetText();

        for (int i = 0; i < PlayerAnimatedText.Length; i++)
        {
            PlayerAnimatedText[i].ResetText();
        }
    }

    public void ChangeParticipant(MonologueTemplate monologueTemplate)
    {
        ResetAllSpeakerText();

        switch (monologueTemplate.characterReference)
        {
            case MonologueTemplate.CharacterReference.Kesath:
                animatedText = KesathAnimatedText;
                Debug.Log("Kesath Talking");
                break;
            case MonologueTemplate.CharacterReference.BarMan:
                animatedText = BarManAnimatedText;
                Debug.Log("BarMan Talking");
                break;
            case MonologueTemplate.CharacterReference.Players:
                int rnd = Random.Range(1, PlayerAnimatedText.Length);
                animatedText = PlayerAnimatedText[rnd];
                Debug.Log("Player Talking");
                break;
            default:
                break;
        }

        ConversationReader.SetAnimatedText(animatedText);
    }
}
