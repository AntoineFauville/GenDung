using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffIndicatorGestion : MonoBehaviour {

    public BuffIndicator[] buffIndicatorList;

    public void GetBuffIndicator(int localChar, int buffNumber, int value, float time)
    {
        StartCoroutine(Wait(localChar,buffNumber,value,time));
    }

    public IEnumerator Wait(int localChar, int buffNumber, int value, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject.Find("Character_" + localChar).transform.Find("Unit/Cube/Panel").GetComponent<Animator>().Play("AnimationTopPlayerBUFF");
        GameObject.Find("Character_" + localChar).transform.Find("Unit/Cube/Panel/Text").GetComponent<Text>().text = buffIndicatorList[buffNumber].textOfTheBuffA + value + " " + buffIndicatorList[buffNumber].textOfTheBuffB;
        GameObject.Find("Character_" + localChar).transform.Find("Unit/Cube/Panel/Text").GetComponent<Text>().color = buffIndicatorList[buffNumber].color;
    }
}
