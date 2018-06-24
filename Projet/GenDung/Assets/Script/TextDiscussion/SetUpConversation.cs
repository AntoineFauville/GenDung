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
        //reset les boutons d'interactions, les mets tous en désactivé pour réactiver seulement ceux dont on a besoin par la suite
        UiConversation.UnlockButton(1, false);
        UiConversation.UnlockButton(2, false);
        UiConversation.UnlockButton(3, false);
        UiConversation.UnlockButton(4, false);
        UiConversation.UnlockButton(5, false);

        //Ceci gère toutes les endroits que l'on peut déverouiller dans la taverne
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

        //Check dans le setup si cette situation a besoin d'un pnj en particulier,
        //si oui alors on active tout ce dont il a besoin. Si non on le laisse déssactiver.
        SetupNeededCharacters();
    }

    #region SetupCharacter leur bouton et leur questmarks (ADD here for new characters)

    public void SetupNeededCharacters()
    {
        if (SetupStoryTavern.Kesath)
        {
            SpeakerManager.KesathConversation = SetupStoryTavern.KesathConversation;
            UiConversation.UnlockButton(1, true);
            StartCoroutine(waitforstartquestmarks(1));
        }
        if (SetupStoryTavern.BarMan)
        {
            SpeakerManager.BarManConversation = SetupStoryTavern.BarManConversation;
            UiConversation.UnlockButton(2, true);
            StartCoroutine(waitforstartquestmarks(2));
        }
        if (SetupStoryTavern.UpgradeCharacter)
        {
            SpeakerManager.UpgradeCharacterConversation = SetupStoryTavern.UpgradeCharacterConversation;
            UiConversation.UnlockButton(3, true);
            StartCoroutine(waitforstartquestmarks(3));
        }
        if (SetupStoryTavern.PNJRandom1)
        {
            SpeakerManager.PNJRandom1Conversation = SetupStoryTavern.PNJRandom1Conversation;
            UiConversation.UnlockButton(4, true);
            StartCoroutine(waitforstartquestmarks(4));
        }
        if (SetupStoryTavern.PNJRandom2)
        {
            SpeakerManager.PNJRandom2Conversation = SetupStoryTavern.PNJRandom2Conversation;
            UiConversation.UnlockButton(5, true);
            StartCoroutine(waitforstartquestmarks(5));
        }
    }

    #endregion



    IEnumerator waitforstartquestmarks(int value)
    {
        yield return new WaitForSeconds(0.5f);
        UiConversation.PlayQuestMarkerAnim(value);
    }
}
