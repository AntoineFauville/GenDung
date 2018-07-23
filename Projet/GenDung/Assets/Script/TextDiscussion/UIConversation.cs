using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIConversation : MonoBehaviour {

    //1. know who need a quest mark
    //2. when clicked removed
    [Header("Kesath")]
    public Animator KesathQuestMarkerSupport;
    public Button KesathStartConversationButton;

    [Header("BarMan")]
    public Animator BarManQuestMarkerSupport;
    public Button BarManStartConversationButton;

    [Header("UpgradeCharacter")]
    public Animator UpgradeCharacterQuestMarkerSupport;
    public Button UpgradeCharacterStartConversationButton;

    [Header("PNJRandom1")]
    public Animator PNJRandom1QuestMarkerSupport;
    public Button PNJRandom1StartConversationButton;

    [Header("PNJRandom2")]
    public Animator PNJRandom2QuestMarkerSupport;
    public Button PNJRandom2StartConversationButton;

    [Space(20)]
    public Animator CadreConversation;
    public ScrollRect ScrollRect;

    public Animator Area1Animator;
    public Animator Area2Animator;
    public Animator Area3Animator;

    private void Awake()
    {
       ResetQuestMarker();
    }

    public void ResetQuestMarker()
    {
        KesathQuestMarkerSupport.Play("Disabled");
        BarManQuestMarkerSupport.Play("Disabled");
        UpgradeCharacterQuestMarkerSupport.Play("Disabled");
        PNJRandom1QuestMarkerSupport.Play("Disabled");
        PNJRandom2QuestMarkerSupport.Play("Disabled");
    }

    #region QuestMarkers (ADD here for new characters)
    public void PlayQuestMarkerAnim(int character)
    {
        switch (character)
        {
            case 1:
                KesathQuestMarkerSupport.Play("Normal");
                break;
            case 2:
                BarManQuestMarkerSupport.Play("Normal");
                break;
            case 3:
                UpgradeCharacterQuestMarkerSupport.Play("Normal");
                break;
            case 4:
                PNJRandom1QuestMarkerSupport.Play("Normal");
                break;
            case 5:
                PNJRandom2QuestMarkerSupport.Play("Normal");
                break;
            default:
                break;
        }
    }

    public void DisableQuestMarkerAnim(int character)
    {
        switch (character)
        {
            case 1:
                KesathQuestMarkerSupport.Play("Disabled");
                break;
            case 2:
                BarManQuestMarkerSupport.Play("Disabled");
                break;
            case 3:
                UpgradeCharacterQuestMarkerSupport.Play("Disabled");
                break;
            case 4:
                PNJRandom1QuestMarkerSupport.Play("Disabled");
                break;
            case 5:
                PNJRandom2QuestMarkerSupport.Play("Disabled");
                break;
            default:
                break;
        }
    }
    #endregion

    #region UnlockButtons (ADD here for new characters)
    public void UnlockButton(int character, bool activate)
    {
        switch (character)
        {
            case 1:
                KesathStartConversationButton.enabled = activate;
                break;
            case 2:
                BarManStartConversationButton.enabled = activate;
                break;
            case 3:
                UpgradeCharacterStartConversationButton.enabled = activate;
                break;
            case 4:
                PNJRandom1StartConversationButton.enabled = activate;
                break;
            case 5:
                PNJRandom2StartConversationButton.enabled = activate;
                break;
            default:
                break;
        }
    }
    #endregion

    #region CadreRelated
    public void StartCadre(bool activate)
    {
        if (activate)
        {
            CadreConversation.Play("CadreConversationAppearing");
            BlockScrollRect(false);
        }
        else
        {
            CadreConversation.Play("CadreConversationDis");
            BlockScrollRect(true);
        }
    }

    public void ResetCadre()
    {
        CadreConversation.Play("CadreConversationDis2");
    }
    #endregion

    public void UnlockAnArea(int areaToUnlock)
    {
        switch (areaToUnlock)
        {
            case 1:
                Area1Animator.Play("AnimationDiscoveringArea");
                break;
            case 2:
                Area2Animator.Play("AnimationDiscoveringArea");
                break;
            case 3:
                Area3Animator.Play("AnimationDiscoveringArea");
                break;
            default:
                break;
        }
    }

    public void BlockScrollRect(bool activate)
    {
        if (activate)
        {
            ScrollRect.enabled = true;
        }
        else
        {
            ScrollRect.enabled = false;
        }
    }
}
