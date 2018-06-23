using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIConversation : MonoBehaviour {

	//1. know who need a quest mark
    //2. when clicked removed

    public Animator KesathQuestMarkerSupport;
    public Animator BarManQuestMarkerSupport;

    public Animator CadreConversation;

    public ScrollRect ScrollRect;

    public Animator Area1Animator;
    public Animator Area2Animator;
    public Animator Area3Animator;

    private void Awake()
    {
       // ResetQuestMarker();
    }

    public void ResetQuestMarker()
    {
        KesathQuestMarkerSupport.Play("Disabled");
        BarManQuestMarkerSupport.Play("Disabled");
    }

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
            default:
                break;
        }
    }

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
}
