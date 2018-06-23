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

    public RectTransform ContentCamera;
    private Vector2 cameraPosition;
    private bool moveCamera;

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
                Define(KesathAnimatedText, KesathConversation, 1, new Vector2(-80, -56));
                Debug.Log("Kesath Talking");
                break;
            case 1:
                //BarMan starts a conversation
                //return then BarMan animated text
                Define(BarManAnimatedText, BarManConversation, 2, new Vector2(185, -121));
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

    void Define(AnimatedText animatedTextDefine, Conversation Conversation, int questMarker, Vector2 pos)
    {
        animatedText = animatedTextDefine;

        ConversationReader.DefineConversataion(Conversation);
        ConversationReader.DefineMonologue(Conversation.CharacterMonologueTemplate[0]);
        ConversationReader.TextForASpecificParticipant = -1;
        ConversationReader.TalkingParticipant = 0;

        UiConversation.DisableQuestMarkerAnim(questMarker);
        UiConversation.StartCadre(true);
        cameraPosition = pos;
        moveCamera = true;
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

    public void ResetAllSpeakerText()
    {
        KesathAnimatedText.ResetText();
        BarManAnimatedText.ResetText();

        for (int i = 0; i < PlayerAnimatedText.Length; i++)
        {
            PlayerAnimatedText[i].ResetText();
        }
    }

    public void MoveCamToConversation()
    {
        ContentCamera.anchoredPosition = Vector2.Lerp(ContentCamera.anchoredPosition, cameraPosition, 5f * Time.deltaTime);
    }

    private void Update()
    {
        float distance = Vector2.Distance(ContentCamera.anchoredPosition, cameraPosition);
        if (distance > 1 && moveCamera)
        {
            MoveCamToConversation();
        }
        else
        {
            moveCamera = false;
        }
    }
}
