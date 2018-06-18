using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explo_SpellController : MonoBehaviour {

    Explo_FightController explo_Fight;
    int index;
    private bool isUiDisplayed = false;

    public void Start()
    {
        explo_Fight = GameObject.Find("BattleSystem/ScriptBattle").GetComponent<Explo_FightController>();
        GameObject.Find("BattleSystem/ClickAway").GetComponent<Button>().onClick.AddListener(() => ClickAway());
        ToolTipShow(false);
    }

    public void SetSpell(int _index)
    {
        explo_Fight.IndicatorIndex(_index);
        explo_Fight.IndicatorAttackUI(_index);
    }

    public void ToolTipShow(bool show)
    {
        isUiDisplayed = show;

        if (!isUiDisplayed)
        {
            GameObject.Find("ToolTipSpell").GetComponent<CanvasGroup>().alpha = 0;
            Debug.Log("Spell Ui is set to " + isUiDisplayed + " normally and should'nt appear");
        }
        else
        {
            GameObject.Find("ToolTipSpell").GetComponent<CanvasGroup>().alpha = 1;
            Debug.Log("Spell Ui is set to " + isUiDisplayed + " normally and should be appearing");
        }
    }

    public void UpdateToolTip(int _index)
    {
        this.index = _index;

        if (explo_Fight.FightCtrl.FighterList[explo_Fight.EntitiesIndex].EntitiesSpells[_index] != null)
        {
            GameObject.Find("ToolTipSpell").transform.Find("ToolTipSpellText").GetComponent<Text>().text = "<size=15><b>" + explo_Fight.FightCtrl.FighterList[explo_Fight.EntitiesIndex].EntitiesSpells[_index].spellName.ToString() + "</b></size>";

            if (explo_Fight.FightCtrl.FighterList[explo_Fight.EntitiesIndex].EntitiesSpells[_index].spellOccurenceType == SpellObject.SpellOccurenceType.NoTurn)
            {
                GameObject.Find("ToolTipSpell").transform.Find("ToolTipSpellDescription").GetComponent<Text>().text = "Damage : " + explo_Fight.FightCtrl.FighterList[explo_Fight.EntitiesIndex].EntitiesSpells[_index].spellDamage.ToString() + " Cost : " + explo_Fight.FightCtrl.FighterList[explo_Fight.EntitiesIndex].EntitiesSpells[_index].spellCost.ToString()
                    + '\n' + "This spell does " + explo_Fight.FightCtrl.FighterList[explo_Fight.EntitiesIndex].EntitiesSpells[_index].spellLogicType.ToString() + "." + '\n' + "Target : " + explo_Fight.FightCtrl.FighterList[explo_Fight.EntitiesIndex].EntitiesSpells[_index].spellTargetType.ToString()
                    + ".";
            }
            else
            {
                GameObject.Find("ToolTipSpell").transform.Find("ToolTipSpellDescription").GetComponent<Text>().text = "Damage : " + explo_Fight.FightCtrl.FighterList[explo_Fight.EntitiesIndex].EntitiesSpells[_index].spellDamage.ToString() + " Cost : " + explo_Fight.FightCtrl.FighterList[explo_Fight.EntitiesIndex].EntitiesSpells[_index].spellCost.ToString()
                    + '\n' + "This spell does " + explo_Fight.FightCtrl.FighterList[explo_Fight.EntitiesIndex].EntitiesSpells[_index].spellLogicType.ToString() + "." + '\n' + "Target : " + explo_Fight.FightCtrl.FighterList[explo_Fight.EntitiesIndex].EntitiesSpells[_index].spellTargetType.ToString()
                    + "." + '\n' + "Place " + explo_Fight.FightCtrl.FighterList[explo_Fight.EntitiesIndex].EntitiesSpells[_index].spellTargetFeedbackAnimationType.ToString() + " on target for " + explo_Fight.FightCtrl.FighterList[explo_Fight.EntitiesIndex].EntitiesSpells[_index].spellOccurenceType.ToString();
            }
            GameObject.Find("ToolTipSpell").transform.Find("ToolTipSpellExplanation").GetComponent<Text>().text = explo_Fight.FightCtrl.FighterList[explo_Fight.EntitiesIndex].EntitiesSpells[_index].spellDescription.ToString();
        }
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