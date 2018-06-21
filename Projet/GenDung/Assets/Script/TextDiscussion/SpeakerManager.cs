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

    private void Start()
    {
        KesathAnimatedText.ResetText();
        BarManAnimatedText.ResetText();

        for (int i = 0; i < PlayerAnimatedText.Length; i++)
        {
            PlayerAnimatedText[i].ResetText();
        }
    }

    public void StartConversation(AnimatedText animatedText)
    {
        KesathAnimatedText.ResetText();
        BarManAnimatedText.ResetText();

        for (int i = 0; i < PlayerAnimatedText.Length; i++)
        {
            PlayerAnimatedText[i].ResetText();
        }

        ConversationReader.SetAnimatedText(animatedText);
    }
}
