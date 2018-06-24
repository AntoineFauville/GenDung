using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakerManager : MonoBehaviour
{
    public AnimatedText KesathAnimatedText;
    public AnimatedText BarManAnimatedText;
    public AnimatedText[] PlayerAnimatedText;
    public AnimatedText UpgradeCharacterAnimatedText;
    public AnimatedText PNJRandom1AnimatedText;
    public AnimatedText PNJRandom2AnimatedText;

    public ConversationReader ConversationReader;
    public UIConversation UiConversation;

    AnimatedText animatedText;

    public RectTransform ContentCamera;
    private Vector2 cameraPosition;
    private bool moveCamera;

    [Header("Stay Empty")]
    public Conversation KesathConversation;
    public Conversation BarManConversation;
    public Conversation UpgradeCharacterConversation;
    public Conversation PNJRandom1Conversation;
    public Conversation PNJRandom2Conversation;

    private void Start()
    {
        ResetAllSpeakerText();
    }

    #region Starting a conversation, when we click on the button it does the following (ADD here for new characters)

    public void StartConversation(int speaker)
    {
        ResetAllSpeakerText();

        switch (speaker)
        {
            case 1:
                Define(KesathAnimatedText, KesathConversation, 1, new Vector2(-80, -56));
                Debug.Log("Kesath Talking");
                break;
            case 2:
                Define(BarManAnimatedText, BarManConversation, 2, new Vector2(185, -121));
                Debug.Log("BarMan Talking");
                break;
            case 3:
                Define(UpgradeCharacterAnimatedText, UpgradeCharacterConversation, 3, new Vector2(-15, 40));
                Debug.Log("UpgradeCharacter Talking");
                break;
            case 4:
                Define(PNJRandom1AnimatedText, PNJRandom1Conversation, 4, new Vector2(0, 0));
                Debug.Log("PNJRandom1 Talking");
                break;
            case 5:
                Define(PNJRandom2AnimatedText, PNJRandom2Conversation, 5, new Vector2(0, 0));
                Debug.Log("PNJRandom2 Talking");
                break;
            default:
                Debug.Log("Unknow Speaker Talking");
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
    #endregion
    
    #region While your are in a conversation, switching the participant (ADD here for new character)

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
            case MonologueTemplate.CharacterReference.UpgradeCharacter:
                animatedText = UpgradeCharacterAnimatedText;
                Debug.Log("UpgradeCharacter Talking");
                break;
            case MonologueTemplate.CharacterReference.PNJRandom1:
                animatedText = PNJRandom1AnimatedText;
                Debug.Log("PNJRandom1 Talking");
                break;
            case MonologueTemplate.CharacterReference.PNJRandom2:
                animatedText = PNJRandom2AnimatedText;
                Debug.Log("PNJRandom2 Talking");
                break;
            default:
                break;
        }

        ConversationReader.SetAnimatedText(animatedText);
    }

    #endregion
    
    public void ResetAllSpeakerText()
    {
        KesathAnimatedText.ResetText();
        BarManAnimatedText.ResetText();

        for (int i = 0; i < PlayerAnimatedText.Length; i++)
        {
            PlayerAnimatedText[i].ResetText();
        }

        UpgradeCharacterAnimatedText.ResetText();
        PNJRandom1AnimatedText.ResetText();
        PNJRandom2AnimatedText.ResetText();
    }

    #region CameraMovingToPosition

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

    #endregion

}
