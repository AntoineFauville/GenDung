using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Discussion", menuName = "DiscussionRelated/DiscussionTemplate", order =1)]
public class DiscussionTemplate : ScriptableObject
{
    public GameObject CharacterPositionPrefab;
	public string[] Messages;
    public int AmountOfMessages;
}
