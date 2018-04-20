﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalDataHolder : MonoBehaviour {

	public bool player, amIPlaying, dead;

	public EnemyObject enemyObject;

	public Character characterObject;

	public float maxHealth;
	public float health;

	public int fighterIndex;
	public int localIndex;

	public int maxActionPointPlayer;
	public int actionPointPlayer;

	public GameObject UiOrderObject;

	// Use this for initialization
	public void Initialize () {

		//if at the start and the enemyObject and the character Object are empty, it means we are not been selected by the holy church.
		//you need to die.

		if (enemyObject == null && characterObject == null) {

			print (this.gameObject.name + " has died, sorry");

			this.gameObject.transform.SetParent (GameObject.Find("BackupInvocationsEnemies").transform);

		} else {
			
			if(!player){
				this.gameObject.transform.SetParent(GameObject.Find("EnemyPanelPlacement").transform);

				this.GetComponent<Image> ().sprite = this.GetComponent<LocalDataHolder> ().enemyObject.enemyIcon;
				maxHealth = this.GetComponent<LocalDataHolder> ().enemyObject.health;
				health = maxHealth;
			} else {
				maxHealth = GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedCharacterList [localIndex].Health_PV;
				health = GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data> ().dungeonData.characterObject [localIndex].tempHealth;
				this.GetComponent<Image> ().sprite = this.GetComponent<LocalDataHolder> ().characterObject.ICON;
				maxActionPointPlayer = this.GetComponent<LocalDataHolder> ().characterObject.ActionPoints_PA;
				actionPointPlayer = maxActionPointPlayer;
			}

			transform.Find ("LifeBar").GetComponent<Image> ().fillAmount = health / maxHealth;

			SetupUiOrderObject ();
		}
	}

	public void looseLife(int pv)
    {
		if(health > 0)
        {
			health -= pv;
		}

		if (health <= 0) {

			health = 0;

			dead = true;

			if(player)
			GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data> ().dungeonData.characterObject [localIndex].died = true;

			//can't interact with me anymore no attacks, no clicking + visual to show i'm dead
			this.gameObject.GetComponent<Button> ().enabled = false;
			this.gameObject.GetComponent<Image> ().color = Color.gray;

			if (player) {
				GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().amountOfPlayerLeft--;
				GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data> ().amountOfPlayerLeft--; //renvoie a l'explo data qu'un est mort pour l'initialization lors du deuxieme combat.
				if (GameObject.Find("ScriptBattle").GetComponent<BattleSystem>().amountOfPlayerLeft <= 0)
					GameObject.Find("ScriptBattle").GetComponent<BattleSystem>().EndBattleAllPlayerDead();

			} else {
				GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().amountOfEnemiesLeft--;
				if (GameObject.Find("ScriptBattle").GetComponent<BattleSystem>().amountOfEnemiesLeft <= 0)
					GameObject.Find("ScriptBattle").GetComponent<BattleSystem>().EndBattleAllMonsterDead();
			}
		}

		SetupUiOrderObject ();
	}

	public void SetupUiOrderObject () 
	{
		if(player){
			UiOrderObject.transform.Find("MASK/PlayerRepresentation").GetComponent<Image>().sprite = this.GetComponent<LocalDataHolder> ().characterObject.ICON;
			UiOrderObject.transform.Find ("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayName").GetComponent<Text> ().text = this.GetComponent<LocalDataHolder> ().characterObject.Name.ToString();
			UiOrderObject.transform.Find ("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPV").GetComponent<Text> ().text = "PV = " + health.ToString() + " / " + this.GetComponent<LocalDataHolder> ().characterObject.Health_PV.ToString();
			UiOrderObject.transform.Find ("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPA").GetComponent<Text> ().text = "PA = " + actionPointPlayer.ToString() + " / " + this.GetComponent<LocalDataHolder> ().characterObject.ActionPoints_PA.ToString();
		} else {
			UiOrderObject.transform.Find("MASK/PlayerRepresentation").GetComponent<Image>().sprite = this.GetComponent<LocalDataHolder> ().enemyObject.enemyIcon;
			UiOrderObject.transform.Find ("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayName").GetComponent<Text> ().text = this.GetComponent<LocalDataHolder> ().enemyObject.enemyName.ToString();
			UiOrderObject.transform.Find ("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPV").GetComponent<Text> ().text = "PV = " + health.ToString() + " / " + this.GetComponent<LocalDataHolder> ().enemyObject.health.ToString();
			UiOrderObject.transform.Find ("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPA").GetComponent<Text> ().enabled = false;
		}

		UpdateLife ();
	}

	public void UpdateUiOrderOrder (bool trig) {
		UiOrderObject.transform.Find ("BouleVerte").GetComponent<Image> ().enabled = trig;
		UiOrderObject.transform.Find ("Scripts").GetComponent<UIOrderBattle> ().Selected (trig);
	}
	
	public void UpdateLife()
	{
		transform.Find ("LifeBar").GetComponent<Image> ().fillAmount = health / maxHealth;
		UiOrderObject.transform.Find("PVOrderDisplay").GetComponent<Image> ().fillAmount = health / maxHealth;

		if (player) {
			GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data> ().dungeonData.characterObject [localIndex].tempHealth = health;
		}
	}

	public void AttackEnemy(){
		if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().attackMode) {
			if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().actuallyPlaying].GetComponent<LocalDataHolder> ().actionPointPlayer > 0) {
				Damage ();
			} else {
				GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().attackMode = false;
			}
		} 
	}

	void Damage(){
		//print ("enemy lost " + GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellDamage);
		GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList[fighterIndex].GetComponent<LocalDataHolder> ().looseLife (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellDamage);
	
		GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList[GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().actuallyPlaying].GetComponent<LocalDataHolder>().actionPointPlayer -= GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellCost;
	}
}
