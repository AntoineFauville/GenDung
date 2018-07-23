using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "DiscussionRelated/ConversationTemplate", order = 1)]
public class Conversation : ScriptableObject
{
    public MonologueTemplate[] CharacterMonologueTemplate;

    public bool ConversationUnlockArea;
    public int AreaToUnlock;

    public string UnnessessaryDescriptionOfTheConversation;
}
