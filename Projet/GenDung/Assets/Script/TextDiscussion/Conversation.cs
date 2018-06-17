using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "DiscussionRelated/ConversationTemplate", order = 1)]
public class Conversation : ScriptableObject
{
    public DiscussionTemplate[] CharacterAndText;
    public int AmountCharacterAndText;
}
