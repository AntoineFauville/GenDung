using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConversation : MonoBehaviour {

	//1. know who need a quest mark
    //2. when clicked removed

    public Animator KesathQuestMarkerSupport;
    public Animator BarManQuestMarkerSupport;

    public Animator CadreConversation;

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
        }
        else
        {
            CadreConversation.Play("CadreConversationDis");
        }
    }

    public void ResetCadre()
    {
        CadreConversation.Play("CadreConversationDis2");
    }
}
