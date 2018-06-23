using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Discussion", menuName = "DiscussionRelated/MonologueTemplate", order =1)]
public class MonologueTemplate : ScriptableObject
{
    public enum CharacterReference { BarMan, Kesath, Players };
    public CharacterReference characterReference;
    public string[] Messages;
}
