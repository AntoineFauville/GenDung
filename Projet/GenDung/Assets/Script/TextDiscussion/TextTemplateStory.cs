using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Story", menuName = "Story/StoryTemplateTextHolder", order = 1)]
public class TextTemplateStory : ScriptableObject
{
    [TextArea]
    public string StoryText;

}
