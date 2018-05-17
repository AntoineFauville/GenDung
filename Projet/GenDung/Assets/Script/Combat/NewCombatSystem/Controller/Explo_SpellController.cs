using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explo_SpellController : MonoBehaviour {

    Explo_FightController explo_Fight;

    public void Start()
    {
        explo_Fight = GameObject.Find("BattleSystem/ScriptBattle").GetComponent<Explo_FightController>();
        GameObject.Find("BattleSystem/ClickAway").GetComponent<Button>().onClick.AddListener(() => ClickAway());
    }

    public void SetSpell(int _index)
    {
        explo_Fight.IndicatorIndex(_index);
        explo_Fight.IndicatorAttackUI(_index);
    }

    public void ClickAway()
    {
        explo_Fight.AttackMode = false;
        explo_Fight.HideShowNext(!explo_Fight.AttackMode);

        explo_Fight.HideIndicator();

        //make sure for the enemies to not show if they are not dead the fact that you can click on them
        for (int i = 0; i < explo_Fight.FightCtrl.FighterList.Count; i++)
        {
            if (explo_Fight.FightCtrl.FighterList[i] is Foe)
            {
                explo_Fight.FightCtrl.FighterList[i].EntitiesGO.transform.Find("Shadow/Pastille2").GetComponent<Image>().enabled = false;
            }
        }
    }
}