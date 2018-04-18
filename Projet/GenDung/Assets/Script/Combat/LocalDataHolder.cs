using System.Collections;
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
	void Start () {
		if(!player){
			this.GetComponent<Image> ().sprite = this.GetComponent<LocalDataHolder> ().enemyObject.enemyIcon;
			maxHealth = this.GetComponent<LocalDataHolder> ().enemyObject.health;
		} else {
			maxHealth = this.GetComponent<LocalDataHolder> ().characterObject.Health_PV;
			this.GetComponent<Image> ().sprite = this.GetComponent<LocalDataHolder> ().characterObject.ICON;
			maxActionPointPlayer = this.GetComponent<LocalDataHolder> ().characterObject.ActionPoints_PA;
			actionPointPlayer = maxActionPointPlayer;
		}

		health = maxHealth;

		transform.Find ("LifeBar").GetComponent<Image> ().fillAmount = health / maxHealth;

		SetupUiOrderObject ();
	}

	public void looseLife(int pv)
    {
		if(health > 0)
        {
			health -= pv;
		}
		UpdateLife ();
	}

	public void SetupUiOrderObject () 
	{
		if(player){
			UiOrderObject.transform.Find("MASK/PlayerRepresentation").GetComponent<Image>().sprite = this.GetComponent<LocalDataHolder> ().characterObject.ICON;
			UiOrderObject.transform.Find ("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayName").GetComponent<Text> ().text = this.GetComponent<LocalDataHolder> ().characterObject.Name.ToString();
			UiOrderObject.transform.Find ("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPV").GetComponent<Text> ().text = "PV = " + this.GetComponent<LocalDataHolder> ().characterObject.Health_PV.ToString();
			UiOrderObject.transform.Find ("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPA").GetComponent<Text> ().text = "PA = " + this.GetComponent<LocalDataHolder> ().characterObject.ActionPoints_PA.ToString();
		} else {
			UiOrderObject.transform.Find("MASK/PlayerRepresentation").GetComponent<Image>().sprite = this.GetComponent<LocalDataHolder> ().enemyObject.enemyIcon;
			UiOrderObject.transform.Find ("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayName").GetComponent<Text> ().text = this.GetComponent<LocalDataHolder> ().enemyObject.enemyName.ToString();
			UiOrderObject.transform.Find ("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPV").GetComponent<Text> ().text = "PV = " + this.GetComponent<LocalDataHolder> ().enemyObject.health.ToString();
			UiOrderObject.transform.Find ("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPA").GetComponent<Text> ().text = "PA = " + this.GetComponent<LocalDataHolder> ().enemyObject.pa.ToString();
		}

		UpdateLife ();
	}

	public void UpdateUiOrderOrder (bool trig) {
		UiOrderObject.transform.Find ("BouleVerte").GetComponent<Image> ().enabled = trig;
	}
	
	public void UpdateLife(){
		
		if (health <= 0) {
			
			dead = true;

			//can't interact with me anymore no attacks, no clicking + visual to show i'm dead
			this.gameObject.GetComponent<Button> ().enabled = false;
			this.gameObject.GetComponent<Image> ().color = Color.gray;

			if (player) {
				GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().amountOfPlayerLeft--;
                if (GameObject.Find("ScriptBattle").GetComponent<BattleSystem>().amountOfPlayerLeft <= 0)
                    GameObject.Find("ScriptBattle").GetComponent<BattleSystem>().EndBattleAllPlayerDead();

            } else {
				GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().amountOfEnemiesLeft--;
                if (GameObject.Find("ScriptBattle").GetComponent<BattleSystem>().amountOfEnemiesLeft <= 0)
                    GameObject.Find("ScriptBattle").GetComponent<BattleSystem>().EndBattleAllMonsterDead();
            }
		}

		transform.Find ("LifeBar").GetComponent<Image> ().fillAmount = health / maxHealth;
		UiOrderObject.transform.Find("PVOrderDisplay").GetComponent<Image> ().fillAmount = health / maxHealth;
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
