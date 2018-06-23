using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpConversation : MonoBehaviour
{
    public SpeakerManager SpeakerManager;
    public UIConversation UiConversation;

    public SetupStoryTavern SetupStoryTavern;

    private void Awake()
    {
        int storyCase = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.storyCase;
        GameObject.Find("DontDestroyOnLoad").GetComponent<StoryTavernController>().GetTavernStatus(storyCase);

        SetupStoryTavern = GameObject.Find("DontDestroyOnLoad").GetComponent<StoryTavernController>().SetupStoryTavern;
        ReadStoryCase();
    }

    private void ReadStoryCase()
    {
        //reset les boutons d'interactions
        UiConversation.UnlockButton(1, false);
        UiConversation.UnlockButton(2, false);

        if (!SetupStoryTavern.UnlockedFirstArea || !SetupStoryTavern.UnlockedSecondArea)
        {
            UiConversation.ResetCadre();
        }

        if (SetupStoryTavern.UnlockedFirstArea)
        {
            UiConversation.UnlockAnArea(1);
        }

        if (SetupStoryTavern.UnlockedSecondArea)
        {
            UiConversation.UnlockAnArea(2);
        }

        if (SetupStoryTavern.Kesath)
        {
            SpeakerManager.KesathConversation = SetupStoryTavern.KesathConversation;
            UiConversation.UnlockButton(1, true);
            StartCoroutine(waitforstartquestmarks());
        }

        if (SetupStoryTavern.BarMan)
        {
            SpeakerManager.BarManConversation = SetupStoryTavern.BarManConversation;
            UiConversation.UnlockButton(2, true);
            StartCoroutine(waitforstartquestmarks2());
        }
    }

    IEnumerator waitforstartquestmarks()
    {
        yield return new WaitForSeconds(0.5f);
        UiConversation.PlayQuestMarkerAnim(1);
    }

    IEnumerator waitforstartquestmarks2()
    {
        yield return new WaitForSeconds(0.5f);
        UiConversation.PlayQuestMarkerAnim(2);
    }
}
