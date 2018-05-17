﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explo_FightController : MonoBehaviour {

    int entitiesIndex = 0;
    int targetIndex;
    int rndAttackEnemy;
    bool attackMode;
    Entities fighterToAttack;
    GameObject next_Button;
    SpellObject selectedSpellObject;
    Explo_DungeonController explo_Dungeon;
    Explo_Room_FightController fightCtrl;

    public void Start()
    {
        next_Button = GameObject.Find("NextPanel");
        explo_Dungeon = GameObject.Find("ScriptBattle").GetComponent<Explo_DungeonController>();
    }

    public void NextTurn()
    {
        HideShowNext(false);

        entitiesIndex++; // getting the next entities to play.
        if (entitiesIndex >= fightCtrl.FighterList.Count) // if we match the count of fighters, we set it back to 0 for looping.
            entitiesIndex = 0;

        if (fightCtrl.FighterList[entitiesIndex].Dead) // If the entitie is dead, it will not play. Seems legit, No ?
        {
            CleanPreviousArrow();
            SetNextdeadArrow();
            NextTurn();
        }
        else
        {
            fightCtrl.FighterList[entitiesIndex].ResetActionPoints();

            SetArrow();
            ManageStatusEffects();
            ////gere les effets et ensuite lance le reste de la fight
            UpdateSpellPanel();
        }
    }

    public void EnemyTurn()
    {
        //attack enemy

        //actualEnemyPlaying = BS.FighterList[BS.actuallyPlaying];

        if (explo_Dungeon.Dungeon.Data.PlayersLeft > 0)
        {
            EnemyAttack();

            //hide next button
            HideShowNext(false);

            //next turn
            StartCoroutine(slowEnemyTurn());
        }
        else
        {
            //BS.EndBattleAllPlayerDead();
        }
    }

    public void HideShowNext(bool hide)
    {
        next_Button.GetComponent<Image>().enabled = hide;
        next_Button.GetComponent<Button>().interactable = hide;
        next_Button.GetComponent<Button>().enabled = hide;
        GameObject.Find("NextPanel/NextText").GetComponent<Text>().enabled = hide;
    }

    public void SetArrow()
    {
        Vector3 actualPosition = new Vector3(0, 0, 0);
        GameObject.Find("Pastille").GetComponent<RectTransform>().SetParent(fightCtrl.FighterList[entitiesIndex].EntitiesGO.transform.Find("Shadow"));
        GameObject.Find("Pastille").GetComponent<RectTransform>().localPosition = actualPosition;

        CleanPreviousArrow();

        fightCtrl.FighterList[entitiesIndex].EntitiesUIOrder.transform.Find("BouleVerte").GetComponent<Image>().enabled = true;
        fightCtrl.FighterList[entitiesIndex].EntitiesUIOrder.transform.Find("Scripts").GetComponent<UIOrderBattle>().Selected(true);
    }

    public void CleanPreviousArrow()
    {
        for (int i = 0; i < fightCtrl.FighterList.Count; i++)
        {
            fightCtrl.FighterList[i].EntitiesUIOrder.transform.Find("BouleVerte").GetComponent<Image>().enabled = false;
            fightCtrl.FighterList[i].EntitiesUIOrder.transform.Find("Scripts").GetComponent<UIOrderBattle>().Selected(false);
        }
    }

    public void SetNextdeadArrow()
    {
        int index;
        index = entitiesIndex + 1;
        if (index >= fightCtrl.FighterList.Count) // if we match the count of fighters, we set it back to 0 for looping.
            index = 0;

        fightCtrl.FighterList[index].EntitiesUIOrder.transform.Find("BouleVerte").GetComponent<Image>().enabled = true;
        fightCtrl.FighterList[index].EntitiesUIOrder.transform.Find("Scripts").GetComponent<UIOrderBattle>().Selected(true);
    }

    public void UpdateSpellPanel()
    {
        if (fightCtrl.FighterList[entitiesIndex] is Player)
        {
            GameObject.Find("FighterPanel").GetComponent<RectTransform>().localPosition = new Vector3(350, -200, 0);

            for (int i = 0; i < 3; i++)
            {
                GameObject.Find("Button_Spell_" + i).GetComponent<Image>().sprite = fightCtrl.FighterList[entitiesIndex].EntitiesSpells[i].spellIcon;
                GameObject.Find("Button_Spell_" + i).GetComponent<SpellPropreties>().spellObject = fightCtrl.FighterList[entitiesIndex].EntitiesSpells[i];
            }
        }
        else
            GameObject.Find("FighterPanel").GetComponent<RectTransform>().localPosition = new Vector3(0, -500, 0);
    }

    public void UpdateUIOrder()
    {
        for (int i = 0; i < FightCtrl.FighterList.Count; i++)
        {
            fightCtrl.FighterList[i].EntitiesUIOrder.transform.Find("MASK/PlayerRepresentation").GetComponent<Image>().sprite = fightCtrl.FighterList[i].EntitiesSprite;
            fightCtrl.FighterList[i].EntitiesUIOrder.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayName").GetComponent<Text>().text = fightCtrl.FighterList[i].Name.ToString();
            fightCtrl.FighterList[i].EntitiesUIOrder.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPV").GetComponent<Text>().text = "HP = " + fightCtrl.FighterList[i].Health.ToString() + " / " + fightCtrl.FighterList[i].MaxHealth.ToString();

            if (fightCtrl.FighterList[i] is Player)
                fightCtrl.FighterList[i].EntitiesUIOrder.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPA").GetComponent<Text>().text = "AP = " + fightCtrl.FighterList[i].ActionPoint.ToString() + " / " + fightCtrl.FighterList[i].MaxActionPoint.ToString();
            else
                fightCtrl.FighterList[i].EntitiesUIOrder.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPA").GetComponent<Text>().enabled = false;

            fightCtrl.FighterList[i].EntitiesUIOrder.transform.Find("LifeControl/LifeBar").GetComponent<Image>().fillAmount = fightCtrl.FighterList[i].Health / fightCtrl.FighterList[i].MaxHealth;
            fightCtrl.FighterList[i].EntitiesGO.transform.Find("LifeControl/LifeBar").GetComponent<Image>().fillAmount = fightCtrl.FighterList[i].Health / fightCtrl.FighterList[i].MaxHealth;
        }
    }

    public void SetTarget(int _target)
    {
        Debug.Log("Targer has been set to fight Index N° : " + _target);
        this.targetIndex = _target;
    }

    //public void AttackEnemy()
    //{

    //    if (attackMode)
    //    {
    //        if (fightCtrl.FighterList[targetIndex] is Foe)
    //        {
    //            //check to know on who I can click.
    //            if (BS.SelectedSpellObject.spellType == SpellObject.SpellType.Enemy)
    //            {
    //                //check if the actual player that wants to do the spell can launch the spell
    //                if (fightCtrl.FighterList[entitiesIndex].ActionPoint > 0)
    //                {
    //                    //StartCoroutine(waitForSpellEffect());
    //                    //Damage ();
    //                }
    //                else
    //                {
    //                    attackMode = false;
    //                }

    //                GetRidOfIndicatorToSeeWhichEnemyICanClickOn();

    //            }
    //        }
    //        else
    //        {
    //            //check to know on who I can click.
    //            if (BS.SelectedSpellObject.spellType == SpellObject.SpellType.Ally)
    //            {
    //                if (fightCtrl.FighterList[entitiesIndex].ActionPoint > 0)
    //                {
    //                    //do something to all the allies
    //                    //StartCoroutine(waitForSpellEffect());
    //                }
    //                else
    //                {
    //                    attackMode = false;
    //                }
    //            }
    //            else if (BS.SelectedSpellObject.spellType == SpellObject.SpellType.Self)
    //            {
    //                if (entitiesIndex == targetIndex)
    //                {
    //                    if (fightCtrl.FighterList[entitiesIndex].ActionPoint > 0)
    //                    {
    //                        //SelfHeal ();
    //                        //StartCoroutine(waitForSpellEffect());
    //                    }
    //                    else
    //                    {
    //                        attackMode = false;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    public void EnemyAttack()
    {
        do
        {
            rndAttackEnemy = Random.Range(0, fightCtrl.FighterList.Count);
            fighterToAttack = fightCtrl.FighterList[rndAttackEnemy];
        }
        while (fighterToAttack.Dead || fighterToAttack is Foe);

        StartCoroutine(waitForEnemyAttack());
    }

    public void lifeTextUp(int amount, bool crit)
    {

        Transform t;
        t = fighterToAttack.EntitiesGO.transform.GetChild(0).transform.GetChild(3);

        GameObject lifeUp;

        lifeUp = Instantiate(Resources.Load("UI_Interface/LifeShower"), t) as GameObject;

        lifeUp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        lifeUp.GetComponent<LifeShower_Controller>().ExtraScaleForCrit(crit);

        if (amount < 0)
        {
            lifeUp.transform.GetChild(0).GetComponent<Text>().text = amount.ToString();
        }
        else
        {
            lifeUp.transform.GetChild(0).GetComponent<Text>().text = "+" + amount.ToString();
        }
    }

    public void IndicatorIndex(int index)
    {
        HideIndicator();
        GameObject.Find("IndicatorSpell" + index).GetComponent<Animator>().Play("SpellIndicatorRotation");
    }

    public void HideIndicator()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject.Find("IndicatorSpell" + i).GetComponent<Animator>().Play("SpellIndicatorRotationIdle");
        }
    }

    public void IndicatorAttackUI(int _spellIndex)
    {
        print("I'll be attacking with spell " + fightCtrl.FighterList[entitiesIndex].EntitiesSpells[_spellIndex].spellName);

        for (int i = 0; i < fightCtrl.FighterList.Count; i++)
        {
            if (fightCtrl.FighterList[i] is Foe)
            {
                fightCtrl.FighterList[i].EntitiesGO.transform.Find("Shadow/Pastille2").GetComponent<Image>().enabled = false;
            }
        }

        //let only interactable object (monsters, or player if heal)
        attackMode = true;
        HideShowNext(!attackMode);


        selectedSpellObject = fightCtrl.FighterList[entitiesIndex].EntitiesSpells[_spellIndex];

        //activate for all the enemies the pastille to show where you can click
        for (int i = 0; i < fightCtrl.FighterList.Count; i++)
        {
            if (selectedSpellObject.spellType == SpellObject.SpellType.Enemy)
            {
                if (fightCtrl.FighterList[i] is Foe)
                {
                    if (!fightCtrl.FighterList[i].Dead)
                    {
                        //you can see where you click on the enemies
                        fightCtrl.FighterList[i].EntitiesGO.transform.Find("Shadow/Pastille2").GetComponent<Image>().enabled = true;
                    }
                }
            }
            else if (selectedSpellObject.spellType == SpellObject.SpellType.Ally)
            {
                if (fightCtrl.FighterList[i] is Player)
                {
                    if (!fightCtrl.FighterList[i].Dead)
                    {
                        //you can see where you click on the allies
                        //BS.FighterList [i].transform.Find ("Shadow/Pastille2").GetComponent<Image> ().enabled = true;
                    }
                }
            }
            else if (selectedSpellObject.spellType == SpellObject.SpellType.Self)
            {
                //you can see where you click on yourself
                //BS.FighterList [BS.actuallyPlaying].transform.Find ("Shadow/Pastille2").GetComponent<Image> ().enabled = true;
            }
        }
    }

    public void ManageStatusEffects()
    {
        HideShowNext(false);

        ////define all the amount of effect for the player
        //int maxEffects;
        ////check if it's a player
        //if (fightCtrl.FighterList[entitiesIndex] is Player)
        //{
        //    //max
        //    maxEffects = explo_Data.dungeonData.TempFighterObject[BS.FighterList[BS.actuallyPlaying].GetComponent<LocalDataHolder>().localIndex].playerStatus.Count;
        //    print(maxEffects);
        //}
        //else
        //{
        //    maxEffects = explo_Data.dungeonData.TempFighterObject[BS.FighterList[BS.actuallyPlaying].GetComponent<LocalDataHolder>().localIndex + 4].playerStatus.Count;
        //    print(maxEffects);
        //}
        //if (maxEffects > 0)
        //{
        //    StartCoroutine(waitForEffectEndedStartOfTurn(maxEffects));
        //}
        //else
        //{
        ContinueFightAfterEffect();
        //    DisplayUIToolTip();
        //}
    }

    public void ContinueFightAfterEffect()
    {

        if (fightCtrl.FighterList[entitiesIndex] is Foe)
        {
            EnemyTurn();
        }
        else
        {
            HideShowNext(true);
        }
    }

    public IEnumerator slowEnemyTurn()
    {
        yield return new WaitForSeconds(1.5f);
        NextTurn();
    }

    public IEnumerator waitForEnemyAttack()
    {
        fightCtrl.FighterList[entitiesIndex].EntitiesGO.transform.Find("Background").GetComponent<Animator>().Play("attackMonster");
        //actualEnemyPlaying.transform.Find("EnemyBackground").GetComponent<Animator>().Play("attackMonster");

        yield return new WaitForSeconds(1.0f);

        if (fighterToAttack is Player)
        {
            if (fighterToAttack.EntitiesAnimator)
            {
                fighterToAttack.EntitiesGO.transform.Find("PersoBackground").GetComponent<Animator>().Play("Attacked");
                //fighterToAttack.transform.Find("PersoBackground").GetComponent<Animator>().Play("Attacked");
            }
        }

        yield return new WaitForSeconds(0.3f);

        if (fighterToAttack is Player)
        {
            if (fighterToAttack.EntitiesAnimator)
            {
                fighterToAttack.EntitiesGO.transform.Find("PersoBackground").GetComponent<Animator>().Play("Idle");
                //fighterToAttack.transform.Find("PersoBackground").GetComponent<Animator>().Play("Idle");
            }
        }

        int chances = Random.Range(0, 100);

        fighterToAttack.ChangeHealth(-fightCtrl.FighterList[entitiesIndex].Attack, false);
        UpdateUIOrder();
        lifeTextUp(-fightCtrl.FighterList[entitiesIndex].Attack, false);

        //if (criticalChances >= chances)
        //{
        //    fighterToAttack.GetComponent<LocalDataHolder>().looseLife(-actualEnemyPlaying.GetComponent<LocalDataHolder>().enemyObject.atk * 1.5f, true);
        //}
        //else
        //{
        //    fighterToAttack.GetComponent<LocalDataHolder>().looseLife(-actualEnemyPlaying.GetComponent<LocalDataHolder>().enemyObject.atk, false);
        //}
    }

    public Explo_Room_FightController FightCtrl
    {
        get
        {
            return fightCtrl;
        }

        set
        {
            fightCtrl = value;
        }
    }

    public bool AttackMode
    {
        get
        {
            return attackMode;
        }

        set
        {
            attackMode = value;
        }
    }
}
