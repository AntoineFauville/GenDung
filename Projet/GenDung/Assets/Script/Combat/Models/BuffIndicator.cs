using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffIndicator", menuName = "Buffs/BuffIndicator", order = 1)]
public class BuffIndicator : ScriptableObject
{
    public string buffIndName = "None";
    public int buffIndId;

    public Color color;
    public string textOfTheBuff = "+0PX";
}