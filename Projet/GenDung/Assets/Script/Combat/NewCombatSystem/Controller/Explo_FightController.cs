using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explo_FightController : MonoBehaviour {

    int entitiesIndex = 0;
    int targetIndex;
    int rndAttackEnemy;
    bool attackMode;
    bool critHit;
    bool attackContinue;
    Entities fighterToAttack;
    GameObject next_Button;
    SpellObject selectedSpellObject;
    Explo_DungeonController explo_Dungeon;
    ProjectileManager projectile_Manager;
    Explo_Room_FightController fightCtrl;
    Explo_Status exploStatus;

    public void Start()
    {
        next_Button = GameObject.Find("NextPanel");
        explo_Dungeon = GameObject.Find("ScriptBattle").GetComponent<Explo_DungeonController>();
        projectile_Manager = GameObject.Find("ScriptBattle").GetComponent<ProjectileManager>();
        attackMode = false;
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
            StartCoroutine(SlowEnemyTurn());
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

        fightCtrl.FighterList[entitiesIndex].EntitiesIndicator.enabled = true;
        fightCtrl.FighterList[entitiesIndex].EntitiesUIOrder.transform.Find("Scripts").GetComponent<UIOrderBattle>().Selected(true);
    }

    public void CleanPreviousArrow()
    {
        for (int i = 0; i < fightCtrl.FighterList.Count; i++)
        {
            fightCtrl.FighterList[i].EntitiesIndicator.enabled = false;
            fightCtrl.FighterList[i].EntitiesUIOrder.transform.Find("Scripts").GetComponent<UIOrderBattle>().Selected(false);
        }
    }

    public void SetNextdeadArrow()
    {
        int index;
        index = entitiesIndex + 1;
        if (index >= fightCtrl.FighterList.Count) // if we match the count of fighters, we set it back to 0 for looping.
            index = 0;

        fightCtrl.FighterList[index].EntitiesIndicator.enabled = true;
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
        this.targetIndex = _target;
        fighterToAttack = fightCtrl.FighterList[_target];
        AttackEnemy();
    }

    public void AttackEnemy()
    {
        if (attackMode)
        {
            if (fightCtrl.FighterList[targetIndex] is Foe && !fightCtrl.FighterList[targetIndex].Dead)
            {
                //check to know on who I can click.
                if (selectedSpellObject.spellType == SpellObject.SpellType.Enemy)
                {
                    //check if the actual player that wants to do the spell can launch the spell
                    if (fightCtrl.FighterList[entitiesIndex].ActionPoint > 0)
                    {
                        StartCoroutine(WaitForSpellEffect());
                        //Damage ();
                    }
                    else
                    {
                        attackMode = false;
                    }

                    //GetRidOfIndicatorToSeeWhichEnemyICanClickOn();
                    //make sure for the enemies to not show if they are not dead the fact that you can click on them
                    for (int i = 0; i < fightCtrl.FighterList.Count; i++)
                    {
                        if (fightCtrl.FighterList[i] is Foe)
                        {
                            fightCtrl.FighterList[i].EntitiesGO.transform.Find("Shadow/Pastille2").GetComponent<Image>().enabled = false;
                        }
                    }

                }
            }
            else
            {
                //check to know on who I can click.
                if (selectedSpellObject.spellType == SpellObject.SpellType.Ally)
                {
                    if (fightCtrl.FighterList[entitiesIndex].ActionPoint > 0)
                    {
                        //do something to all the allies
                        StartCoroutine(WaitForSpellEffect());
                    }
                    else
                    {
                        attackMode = false;
                    }
                }
                else if (selectedSpellObject.spellType == SpellObject.SpellType.Self)
                {
                    if (entitiesIndex == targetIndex)
                    {
                        if (fightCtrl.FighterList[entitiesIndex].ActionPoint > 0)
                        {
                            //SelfHeal ();
                            StartCoroutine(WaitForSpellEffect());
                        }
                        else
                        {
                            attackMode = false;
                        }
                    }
                }
            }
        }
    }

    public void EnemyAttack()
    {
        do
        {
            rndAttackEnemy = Random.Range(0, fightCtrl.FighterList.Count);
            fighterToAttack = fightCtrl.FighterList[rndAttackEnemy];
        }
        while (fighterToAttack.Dead || fighterToAttack is Foe);

        StartCoroutine(WaitForEnemyAttack());
    }

    public void EffectAssignement()
    {
        if (selectedSpellObject.spellEffect == SpellObject.SpellEffect.None)
        {
            return;
        }

        switch (selectedSpellObject.spellEffect)
        {
            case SpellObject.SpellEffect.None:
                break;

            case SpellObject.SpellEffect.Effect_Arcane_Projectile:
                projectile_Manager.LaunchProjectile(fightCtrl.FighterList[entitiesIndex].EntitiesGO.transform, fightCtrl.FighterList[targetIndex].EntitiesGO.transform);
                break;

            case SpellObject.SpellEffect.Effect_Blood:
                fightCtrl.FighterList[targetIndex].EntitiesEffectAnimator.Play("Effect_Blood");
                break;

            case SpellObject.SpellEffect.Effect_Roots:
                fightCtrl.FighterList[targetIndex].EntitiesEffectAnimator.Play("Effect_Rooted");
                break;

            case SpellObject.SpellEffect.Effect_Spike:
                fightCtrl.FighterList[targetIndex].EntitiesEffectAnimator.Play("Effect_Blood");
                break;
        }
    }

    public void StopEffect()
    {
        fightCtrl.FighterList[targetIndex].EntitiesEffectAnimator.Play("Effect_None");
    }

    public void StatusAssignement()
    {
        if (selectedSpellObject.spellStatus == SpellObject.SpellStatus.None )
        {
            return;
        }

        switch(selectedSpellObject.spellStatus)
        {
            case SpellObject.SpellStatus.Poisoned:

                for (int i = 0; i < fightCtrl.FighterList[targetIndex].EntitiesStatus.Count; i++)
                {
                    if (fightCtrl.FighterList[targetIndex].EntitiesStatus[i] is Explo_Status_Poisoned)
                    {
                        return;
                    } 
                }
                exploStatus = new Explo_Status_Poisoned(fightCtrl.FighterList[targetIndex]);
                fightCtrl.FighterList[targetIndex].EntitiesStatus.Add(exploStatus);
                Debug.Log("Added Effect");
                break;

            case SpellObject.SpellStatus.Healed:

                for (int i = 0; i < fightCtrl.FighterList[targetIndex].EntitiesStatus.Count; i++)
                {
                    if (fightCtrl.FighterList[targetIndex].EntitiesStatus[i] is Explo_Status_Healed)
                    {
                        return;
                    }
                }

                exploStatus = new Explo_Status_Healed(fightCtrl.FighterList[targetIndex]);
                fightCtrl.FighterList[targetIndex].EntitiesStatus.Add(exploStatus);
                break;

            case SpellObject.SpellStatus.Sheilded:

                for (int i = 0; i < fightCtrl.FighterList[targetIndex].EntitiesStatus.Count; i++)
                {
                    if (fightCtrl.FighterList[targetIndex].EntitiesStatus[i] is Explo_Status_Shielded)
                    {
                        return;
                    }
                }

                exploStatus = new Explo_Status_Shielded(fightCtrl.FighterList[targetIndex]);
                fightCtrl.FighterList[targetIndex].EntitiesStatus.Add(exploStatus);
                break;

            case SpellObject.SpellStatus.TemporaryLifed:

                for (int i = 0; i < fightCtrl.FighterList[targetIndex].EntitiesStatus.Count; i++)
                {
                    if (fightCtrl.FighterList[targetIndex].EntitiesStatus[i] is Explo_Status_TemporaryLife)
                    {
                        return;
                    }
                }


                exploStatus = new Explo_Status_TemporaryLife(fightCtrl.FighterList[targetIndex]);
                fightCtrl.FighterList[targetIndex].EntitiesStatus.Add(exploStatus);
                break;

            case SpellObject.SpellStatus.Cursed:

                for (int i = 0; i < fightCtrl.FighterList[targetIndex].EntitiesStatus.Count; i++)
                {
                    if (fightCtrl.FighterList[targetIndex].EntitiesStatus[i] is Explo_Status_Cursed)
                    {
                        return;
                    }
                }


                exploStatus = new Explo_Status_Cursed(fightCtrl.FighterList[targetIndex]);
                fightCtrl.FighterList[targetIndex].EntitiesStatus.Add(exploStatus);
                break;

            case SpellObject.SpellStatus.ResistanceReduced:

                for (int i = 0; i < fightCtrl.FighterList[targetIndex].EntitiesStatus.Count; i++)
                {
                    if (fightCtrl.FighterList[targetIndex].EntitiesStatus[i] is Explo_Status_ResistanceReduced)
                    {
                        return;
                    }
                }


                exploStatus = new Explo_Status_ResistanceReduced(fightCtrl.FighterList[targetIndex]);
                fightCtrl.FighterList[targetIndex].EntitiesStatus.Add(exploStatus);
                break;

            case SpellObject.SpellStatus.AvoidanceReduced:

                for (int i = 0; i < fightCtrl.FighterList[targetIndex].EntitiesStatus.Count; i++)
                {
                    if (fightCtrl.FighterList[targetIndex].EntitiesStatus[i] is Explo_Status_AvoidanceReduced)
                    {
                        return;
                    }
                }


                exploStatus = new Explo_Status_AvoidanceReduced(fightCtrl.FighterList[targetIndex]);
                fightCtrl.FighterList[targetIndex].EntitiesStatus.Add(exploStatus);
                break;

            case SpellObject.SpellStatus.Spike:

                for (int i = 0; i < fightCtrl.FighterList[targetIndex].EntitiesStatus.Count; i++)
                {
                    if (fightCtrl.FighterList[targetIndex].EntitiesStatus[i] is Explo_Status_Spike)
                    {
                        return;
                    }
                }


                exploStatus = new Explo_Status_Spike(fightCtrl.FighterList[targetIndex]);
                fightCtrl.FighterList[targetIndex].EntitiesStatus.Add(exploStatus);
                break;

            default:
                Debug.Log("Ow ow ow MotherFucker !!!");
                break;
        }
    }

    public void LifeTextUp(int amount, bool crit)
    {

        Transform t;
        t = fighterToAttack.EntitiesTextLifeDisplayTransform;

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
                        //fightCtrl.FighterList [i].EntitiesGO.transform.Find ("Shadow/Pastille2").GetComponent<Image> ().enabled = true;
                    }
                }
            }
            else if (selectedSpellObject.spellType == SpellObject.SpellType.Self)
            {
                //you can see where you click on yourself
                //fightCtrl.FighterList[entitiesIndex].EntitiesGO.transform.Find ("Shadow/Pastille2").GetComponent<Image> ().enabled = true;
            }
        }
    }

    void CalculChances()
    {

        int randChancesToHit = Random.Range(0, 100);
        int randChancesToCrit = Random.Range(0, 100);

        if (randChancesToHit >= selectedSpellObject.chancesOfMiss)
        {
            if (randChancesToCrit <= selectedSpellObject.chancesOfCrit)
            {
                critHit = true;
            }
            attackContinue = true;
        }
        else
        {
            print("missed");
        }
    }

    void CalculDamageDone(SpellObject.SpellLogicType spellLogicType, SpellObject.SpellTargetType spellTargetType)
    {

        print(spellLogicType);
        print(spellTargetType);

        if (spellLogicType == SpellObject.SpellLogicType.Damage)
        {
            if (critHit)
            {
                if (spellTargetType == SpellObject.SpellTargetType.EnemyAll)
                {
                    for (int enemyNumber = 0; enemyNumber < fightCtrl.FighterList.Count; enemyNumber++)
                    {
                        if (fightCtrl.FighterList[enemyNumber] is Foe)
                        {
                            fightCtrl.FighterList[enemyNumber].ChangeHealth(-selectedSpellObject.spellDamage * 1.5f, true);
                        }
                    }
                } else if (spellTargetType == SpellObject.SpellTargetType.PlayerAll)
                {
                    for (int enemyNumber = 0; enemyNumber < fightCtrl.FighterList.Count; enemyNumber++)
                    {
                        if (fightCtrl.FighterList[enemyNumber] is Player)
                        {
                            fightCtrl.FighterList[enemyNumber].ChangeHealth(-selectedSpellObject.spellDamage * 1.5f, true);
                        }
                    }
                } else
                {
                    fightCtrl.FighterList[targetIndex].ChangeHealth(-selectedSpellObject.spellDamage * 1.5f, true);
                }
            }
            else
            {
                if (spellTargetType == SpellObject.SpellTargetType.EnemyAll)
                {
                    for (int enemyNumber = 0; enemyNumber < fightCtrl.FighterList.Count; enemyNumber++)
                    {
                        if (fightCtrl.FighterList[enemyNumber] is Foe)
                        {
                            fightCtrl.FighterList[enemyNumber].ChangeHealth(-selectedSpellObject.spellDamage * 1.5f, false);
                        }
                    }
                }
                else if (spellTargetType == SpellObject.SpellTargetType.PlayerAll)
                {
                    for (int enemyNumber = 0; enemyNumber < fightCtrl.FighterList.Count; enemyNumber++)
                    {
                        if (fightCtrl.FighterList[enemyNumber] is Player)
                        {
                            fightCtrl.FighterList[enemyNumber].ChangeHealth(-selectedSpellObject.spellDamage * 1.5f, false);
                        }
                    }
                }
                else
                {
                    fightCtrl.FighterList[targetIndex].ChangeHealth(-selectedSpellObject.spellDamage, false);
                }
            }
        }

        else if (spellLogicType == SpellObject.SpellLogicType.Heal)
        {
            if (critHit)
            {
                if (spellTargetType == SpellObject.SpellTargetType.EnemyAll)
                {
                    for (int enemyNumber = 0; enemyNumber < fightCtrl.FighterList.Count; enemyNumber++)
                    {
                        if (fightCtrl.FighterList[enemyNumber] is Foe)
                        {
                            fightCtrl.FighterList[enemyNumber].ChangeHealth(selectedSpellObject.spellDamage * 1.5f, true);
                        }
                    }
                }
                else if (spellTargetType == SpellObject.SpellTargetType.PlayerAll)
                {
                    for (int enemyNumber = 0; enemyNumber < fightCtrl.FighterList.Count; enemyNumber++)
                    {
                        if (fightCtrl.FighterList[enemyNumber] is Player)
                        {
                            fightCtrl.FighterList[enemyNumber].ChangeHealth(selectedSpellObject.spellDamage * 1.5f, true);
                        }
                    }
                }
                else
                {
                    fightCtrl.FighterList[targetIndex].ChangeHealth(selectedSpellObject.spellDamage * 1.5f, true);
                }
            }
            else
            {
                if (spellTargetType == SpellObject.SpellTargetType.EnemyAll)
                {
                    for (int enemyNumber = 0; enemyNumber < fightCtrl.FighterList.Count; enemyNumber++)
                    {
                        if (fightCtrl.FighterList[enemyNumber] is Foe)
                        {
                            fightCtrl.FighterList[enemyNumber].ChangeHealth(selectedSpellObject.spellDamage * 1.5f, false);
                        }
                    }
                }
                else if (spellTargetType == SpellObject.SpellTargetType.PlayerAll)
                {
                    for (int enemyNumber = 0; enemyNumber < fightCtrl.FighterList.Count; enemyNumber++)
                    {
                        if (fightCtrl.FighterList[enemyNumber] is Player)
                        {
                            fightCtrl.FighterList[enemyNumber].ChangeHealth(selectedSpellObject.spellDamage * 1.5f, false);
                        }
                    }
                }
                else
                {
                    fightCtrl.FighterList[targetIndex].ChangeHealth(selectedSpellObject.spellDamage, false);
                }
            }
        }



        if (fightCtrl.FighterList[targetIndex].Health > fightCtrl.FighterList[targetIndex].MaxHealth)
        {
            fightCtrl.FighterList[targetIndex].Health = fightCtrl.FighterList[targetIndex].MaxHealth;
        }

        critHit = false;
    }

    public void ManageStatusEffects()
    {
        HideShowNext(false);

        if (fightCtrl.FighterList[entitiesIndex].EntitiesStatus.Count == 0)
        {
            ContinueFightAfterEffect();
            return;
        }

        StartCoroutine(WaitForEffectToBeAppliedAtTurnStart());
    }

    public void ContinueFightAfterEffect()
    {

        if (fightCtrl.FighterList[entitiesIndex] is Foe)
        {
            StartCoroutine(WaitBeforeFoeTurn());
        }
        else
        {
            HideShowNext(true);
        }
    }

    void AnimFeedbackEnemy(SpellObject.SpellTargetType spellTarget, bool on)
    {

        Animator EnemyAnimator = fightCtrl.FighterList[targetIndex].EntitiesGO.transform.Find("Background").GetComponent<Animator>();

        if (spellTarget == SpellObject.SpellTargetType.EnemySingle)
        {
            if (on)
            {
                EnemyAnimator.Play("DamageMonster");
            }
            else
            {
                EnemyAnimator.Play("IdleMonster");
            }
        }
    }

    public IEnumerator SlowEnemyTurn()
    {
        yield return new WaitForSeconds(1.5f);
        NextTurn();
    }

    public IEnumerator WaitForEnemyAttack()
    {
        fightCtrl.FighterList[entitiesIndex].EntitiesGO.transform.Find("Background").GetComponent<Animator>().Play("attackMonster");

        yield return new WaitForSeconds(1.0f);

        if (fighterToAttack is Player)
        {
            if (fighterToAttack.EntitiesAnimator)
            {
                fighterToAttack.EntitiesGO.transform.Find("Background").GetComponent<Animator>().Play("Attacked");
            }
        }

        yield return new WaitForSeconds(0.3f);

        if (fighterToAttack is Player)
        {
            if (fighterToAttack.EntitiesAnimator)
            {
                fighterToAttack.EntitiesGO.transform.Find("Background").GetComponent<Animator>().Play("Idle");
            }
        }

        int chances = Random.Range(0, 100);


        fighterToAttack.ChangeHealth(-fightCtrl.FighterList[entitiesIndex].Attack, false);
        UpdateUIOrder();
        LifeTextUp(-fightCtrl.FighterList[entitiesIndex].Attack, false);

        //if (criticalChances >= chances)
        //{
        //    fighterToAttack.GetComponent<LocalDataHolder>().looseLife(-actualEnemyPlaying.GetComponent<LocalDataHolder>().enemyObject.atk * 1.5f, true);
        //}
        //else
        //{
        //    fighterToAttack.GetComponent<LocalDataHolder>().looseLife(-actualEnemyPlaying.GetComponent<LocalDataHolder>().enemyObject.atk, false);
        //}
    }

    IEnumerator WaitForSpellEffect()
    {
        //define the index of who'll be attacking
        //DefineTargetIndex(BS.SelectedSpellObject.spellTargetType);

        //check the action point of the player
        fightCtrl.FighterList[entitiesIndex].ChangeActionPoints(selectedSpellObject.spellCost);

        //play player animation
        fightCtrl.FighterList[entitiesIndex].EntitiesGO.transform.Find("Background").GetComponent<Animator>().Play(selectedSpellObject.spellAnimator.ToString());
        //PlayerAnimationPropreties()


        //calculate the chances to hit or crit == Calculate if "missed" or "critical chance" or "regular spell effect"
        CalculChances();
        //depending on the result, throw here an inidactor to know if we continue the attack or not.


        //Wait for anim player to finish depending on time of spell anim time
        //if it contains a reaction or a spell invocation at the enemy's place we need to instantiate or play an effect on the enemy
        yield return new WaitForSeconds(selectedSpellObject.SpellCastAnimationTime / 2);
        EffectAssignement(); // Assign Status -- it's part 2. 
        yield return new WaitForSeconds(selectedSpellObject.SpellCastAnimationTime / 2);
        StopEffect();

        if (attackContinue)
        {

            if (selectedSpellObject.spellLogicType == SpellObject.SpellLogicType.Damage)
            {
                //if it's damage make the fighter react to taking damages
                AnimFeedbackEnemy(selectedSpellObject.spellTargetType, true);

                // wait for anim enemy reaction to spell. (Constant of 1 sec for exemple) + Launched Hit or Critical animation
                yield return new WaitForSeconds(1.0f);

                AnimFeedbackEnemy(selectedSpellObject.spellTargetType, false);
            }
            //do the damage on the target (healing included)
            CalculDamageDone(selectedSpellObject.spellLogicType, selectedSpellObject.spellTargetType);

            //check if the extra effect is != none, so then we need to make an animation for that.
            if (selectedSpellObject.spellTargetFeedbackAnimationType != SpellObject.SpellTargetFeedbackAnimationType.None)
            {
                //wait for anim Feedback Animation on target
                //CheckExtraEffect(true);
                yield return new WaitForSeconds(0.8f);
                //CheckExtraEffect(false);
            }

            UpdateUIOrder(); // Call the Update of UI to display Damage made to the Enemy.
            if (selectedSpellObject.spellLogicType == SpellObject.SpellLogicType.Damage)
                LifeTextUp(-selectedSpellObject.spellDamage, false);
            else if (selectedSpellObject.spellLogicType == SpellObject.SpellLogicType.Heal)
                LifeTextUp(selectedSpellObject.spellDamage, false);

            StatusAssignement();

            if (exploStatus != null)
            {
                yield return new WaitForSeconds(exploStatus.AnimationDuration);
                exploStatus.Entity.EntitiesEffectAnimator.Play("Effect_None");
            }
        }
        else
        {
            // wait for anim enemy reaction to spell. + launch MISSED animation
            yield return new WaitForSeconds(0.5f);
        }

        HideIndicator();
        attackMode = false;
        HideShowNext(true);
    }

    public IEnumerator WaitForEffectToBeAppliedAtTurnStart()
    {
        for (int i = 0; i < fightCtrl.FighterList[entitiesIndex].EntitiesStatus.Count; i++)
        {
            fightCtrl.FighterList[entitiesIndex].EntitiesStatus[i].DoSomething(i);
            yield return new WaitForSeconds(fightCtrl.FighterList[targetIndex].EntitiesStatus[i].AnimationDuration);
            fightCtrl.FighterList[entitiesIndex].EntitiesEffectAnimator.Play("Effect_None");

            if (fightCtrl.FighterList[entitiesIndex].EntitiesStatus[i] is Explo_Status_Healed )
                LifeTextUp(Mathf.RoundToInt(fightCtrl.FighterList[entitiesIndex].EntitiesStatus[i].TickValue), false);
            else
            {
                //if it's damage make the fighter react to taking damages
                AnimFeedbackEnemy(selectedSpellObject.spellTargetType, true);

                // wait for anim enemy reaction to spell. (Constant of 1 sec for exemple) + Launched Hit or Critical animation
                yield return new WaitForSeconds(1.0f);

                AnimFeedbackEnemy(selectedSpellObject.spellTargetType, false);

                LifeTextUp(-Mathf.RoundToInt(fightCtrl.FighterList[entitiesIndex].EntitiesStatus[i].TickValue), false);
            }
        }

        ContinueFightAfterEffect();
    }

    public IEnumerator WaitBeforeFoeTurn()
    {
        yield return new WaitForSeconds(0.5f);
        EnemyTurn();
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

    public int EntitiesIndex
    {
        get
        {
            return entitiesIndex;
        }

        set
        {
            entitiesIndex = value;
        }
    }
}
