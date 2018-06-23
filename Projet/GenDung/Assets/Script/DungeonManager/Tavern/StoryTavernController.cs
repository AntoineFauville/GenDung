using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTavernController : MonoBehaviour
{
    public SetupStoryTavern[] SetupStoryTavernList;
    
    [Header("Leave Empty")]
    public SetupStoryTavern SetupStoryTavern;

    public void SetTavernStatus(int storyCase)
    {
        //send to savegame
        GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.storyCase = storyCase;
    }

    public void GetTavernStatus(int storyCase)
    {
        switch (storyCase)
        {
            case 1:
                Debug.Log("We'll set the tavern to: First Time you enter the tavern");
                SetupStoryTavern = SetupStoryTavernList[0];
                ; break;
            case 2:
                Debug.Log("We'll set the tavern to: Anytime after first time entered");
                SetupStoryTavern = SetupStoryTavernList[1];
                break;
            case 3:
                Debug.Log("We'll set the tavern to: Anytime after first time entered");
                SetupStoryTavern = SetupStoryTavernList[2];
                break;
            case 4:
                Debug.Log("We'll set the tavern to: Introduction To Upgrade Guy");
                SetupStoryTavern = SetupStoryTavernList[3];
                break;
            case 5:
                Debug.Log("We'll set the tavern to: Not defined yet");
                SetupStoryTavern = SetupStoryTavernList[4];
                break;
            default:
                Debug.Log("unknown case");
                break;
        }
    }
}
