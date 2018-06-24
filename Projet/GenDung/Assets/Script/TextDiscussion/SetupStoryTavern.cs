using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "DiscussionRelated/SetupStoryTavern", order = 1)]
public class SetupStoryTavern : ScriptableObject
{
    [Header("Important PNJ")]
    public bool Kesath;
    public Conversation KesathConversation;
    public bool BarMan;
    public Conversation BarManConversation;
    [Space(10)]
    [Header("Tavern area we unlock with important pnj")]
    public bool UnlockedFirstArea;
    public bool UnlockedSecondArea;
    [Space(10)]
    [Header("UpgradeGuy")]
    public bool UpgradeCharacter;
    public Conversation UpgradeCharacterConversation;
    [Space(10)]
    [Header("RandomPnjs")]
    public bool PNJRandom1;
    public Conversation PNJRandom1Conversation;

    public bool PNJRandom2;
    public Conversation PNJRandom2Conversation;
}
