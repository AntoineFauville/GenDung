using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "DiscussionRelated/SetupStoryTavern", order = 1)]
public class SetupStoryTavern : ScriptableObject
{
    public bool Kesath;
    public Conversation KesathConversation;
    public bool BarMan;
    public Conversation BarManConversation;

    public bool UnlockedFirstArea;
    public bool UnlockedSecondArea;
}
