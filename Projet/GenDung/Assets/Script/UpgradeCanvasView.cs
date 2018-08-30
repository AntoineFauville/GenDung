using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCanvasView : MonoBehaviour
{
    [SerializeField] private Text _strengthModificator;
    [SerializeField] private Text _strengthStatDisplay;
    [SerializeField] private Text _knowledgeModificator;
    [SerializeField] private Text _knowledgeStatDisplay;
    [SerializeField] private Text _dexterityModificator;
    [SerializeField] private Text _dexterityStatDisplay;
    [SerializeField] private Text _wisdomModificator;
    [SerializeField] private Text _wisdomStatDisplay;
    [SerializeField] private Text _constitutionModificator;
    [SerializeField] private Text _constitutionStatDisplay;
    [Space(10)]
    [SerializeField] private Text _resistanceDisplay;
    [Space(10)]
    [SerializeField] private Text _playerGoldDisplay;
    [SerializeField] private Text _goldCostForUpgradesDisplay;
    [Space(10)]
    [SerializeField] private Text _SpellOneNameDisplay;
    [SerializeField] private Text _SpellOneDescriptionDisplay;
    [SerializeField] private Image _imageSpellOne;
    [SerializeField] private Text _SpellTwoNameDisplay;
    [SerializeField] private Text _SpellTwoDescriptionDisplay;
    [SerializeField] private Image _imageSpellTwo;
    [SerializeField] private Text _SpellThreeNameDisplay;
    [SerializeField] private Text _SpellThreeDescriptionDisplay;
    [SerializeField] private Image _imageSpellThree;


    public void UpdateModificator(int statModified,int modificatorValue)
    {
        switch (statModified)
        {
            case 0 :
                _strengthModificator.text = "+ " + modificatorValue.ToString();
                break;
            case 1:
                _knowledgeModificator.text = "+ " + modificatorValue.ToString();
                break;
            case 2:
                _dexterityModificator.text = "+ " + modificatorValue.ToString();
                break;
            case 3:
                _wisdomModificator.text = "+ " + modificatorValue.ToString();
                break;
            case 4:
                _constitutionModificator.text = "+ " + modificatorValue.ToString();
                break;
        }
    }

    public void UpdateStat(int stat, int statValue)
    {
        switch (stat)
        {
            case 0:
                _strengthStatDisplay.text = statValue.ToString();
                break;
            case 1:
                _knowledgeStatDisplay.text = statValue.ToString();
                break;
            case 2:
                _dexterityStatDisplay.text = statValue.ToString();
                break;
            case 3:
                _wisdomStatDisplay.text = statValue.ToString();
                break;
            case 4:
                _constitutionStatDisplay.text = statValue.ToString();
                break;
        }
    }

    public void UpdateResistanceText(int strength, int knowledge, int dexterity, int wisdom, int constitution)
    {
        _resistanceDisplay.text = "Physical Resistance : " + (strength * 3 + dexterity).ToString() + "%" +
                                  "Blood Resistance : " + (knowledge * 2 + wisdom).ToString() + "%" +
                                  "Magic Resistance : " + (knowledge + wisdom).ToString() + "%" +
                                  "Nature Resistance : " + (dexterity).ToString() + "%" +
                                  "Critical Resistance : " + (knowledge + dexterity * 2 + wisdom).ToString() + "%" +
                                  "Dodging Resistance : " + (strength + dexterity + wisdom + constitution).ToString() + "%";
    }

    public void UpdatePlayerGoldDisplay(int playerGold)
    {
        _playerGoldDisplay.text = playerGold.ToString();
    }

    public void UpdateGoldCostForUpgrade(int totalCost)
    {
        _goldCostForUpgradesDisplay.text = totalCost.ToString();
    }

    public void UpdateSpellIcon(Image spellOneImage, Image spellTwoImage, Image spellThreeImage)
    {
        _imageSpellOne.sprite = spellOneImage.sprite;
        _imageSpellTwo.sprite = spellTwoImage.sprite;
        _imageSpellThree.sprite = spellThreeImage.sprite;
    }

    public void UpdateSpellName(int spell, string spellName)
    {
        switch (spell)
        {
            case 0:
                _SpellOneNameDisplay.text = spellName;
                break;
            case 1:
                _SpellTwoNameDisplay.text = spellName;
                break;
            case 2:
                _SpellThreeNameDisplay.text = spellName;
                break;
        }
    }

    public void UpdateSpell(int spell, string spellDescription)
    {
        switch (spell)
        {
            case 0:
                _SpellOneDescriptionDisplay.text = spellDescription;
                break;
            case 1:
                _SpellTwoDescriptionDisplay.text = spellDescription;
                break;
            case 2:
                _SpellThreeDescriptionDisplay.text = spellDescription;
                break;
        }
    }
}
