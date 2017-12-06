﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffIndicatorGestion : MonoBehaviour {

    public BuffIndicator[] buffIndicatorList;

    public void GetBuffIndicator(int buffNumber,int localChar)
    {
        GameObject.Find("Character_" + localChar).transform.Find("Panel").GetComponent<Animator>().Play("AnimationTopPlayerBUFF");
        GameObject.Find("Character_" + localChar).transform.Find("Panel/Text").GetComponent<Text>().text = buffIndicatorList[buffNumber].textOfTheBuff;
        GameObject.Find("Character_" + localChar).transform.Find("Panel/Text").GetComponent<Text>().Color = buffIndicatorList[buffNumber].color;
    }
}
