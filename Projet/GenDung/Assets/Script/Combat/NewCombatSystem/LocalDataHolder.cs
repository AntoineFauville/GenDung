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
	public int indexFighterToAttack;

	public int maxActionPointPlayer;
	public int actionPointPlayer;

	public GameObject UiOrderObject;

	public bool AttackContinue;

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
				this.transform.Find("EnemyBackground").GetComponent<Image> ().sprite = this.GetComponent<LocalDataHolder> ().enemyObject.enemyIcon;
				maxHealth = this.GetComponent<LocalDataHolder> ().enemyObject.health;
				health = maxHealth;
			} else {
				maxHealth = GameObject.Find ("DontDestroyOnLoad").GetComponent<SavingSystem> ().gameData.SavedCharacterList [localIndex].Health_PV;
				health = GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data> ().dungeonData.characterObject [localIndex].tempHealth;
				this.transform.Find("PersoBackground").GetComponent<Image> ().sprite = this.GetComponent<LocalDataHolder> ().characterObject.ICON;
				this.transform.Find ("EffectLayer").GetComponent<Image> ().color = new Color (255, 255, 255, 0);

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
			health += pv;
		}

		if (health <= 0) {

			health = 0;

			dead = true;

			//can't interact with me anymore no attacks, no clicking + visual to show i'm dead
			if(player) {
				GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data> ().dungeonData.characterObject [localIndex].died = true;
				this.transform.Find("PersoBackground").GetComponent<Button> ().enabled = false;
				this.transform.Find("PersoBackground").GetComponent<Image> ().color = Color.gray;
			} else {
				this.transform.Find("EnemyBackground").GetComponent<Button> ().enabled = false;
				this.transform.Find("EnemyBackground").GetComponent<Image> ().color = Color.gray;
			}


			if (player) {
				GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().amountOfPlayerLeft--;
				GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data> ().amountOfPlayerLeft--; //renvoie a l'explo data qu'un est mort pour l'initialization lors du deuxieme combat.
				if (GameObject.Find("ScriptBattle").GetComponent<BattleSystem>().amountOfPlayerLeft <= 0)
					GameObject.Find("ScriptBattle").GetComponent<BattleSystem>().EndBattleAllPlayerDead();

			} else {
				
				AddEnemyKilled (enemyObject);

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
		
		if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().attackMode) 
		{
			if (!GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [fighterIndex].GetComponent<LocalDataHolder> ().player) 
			{
				//check to know on who I can click.
				if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellType == SpellObject.SpellType.Enemy) {
					//check if the actual player that wants to do the spell can launch the spell
					if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().actuallyPlaying].GetComponent<LocalDataHolder> ().actionPointPlayer > 0) {
						StartCoroutine(waitForSpellEffect());
						//Damage ();
					} else {
						GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().attackMode = false;
					}

					GetRidOfIndicatorToSeeWhichEnemyICanClickOn ();

				}
			} 
			else 
			{
				//check to know on who I can click.
				if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellType == SpellObject.SpellType.Ally) 
				{
					if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().actuallyPlaying].GetComponent<LocalDataHolder> ().actionPointPlayer > 0) {
						//do something to all the allies
						StartCoroutine(waitForSpellEffect());
					} else {
						GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().attackMode = false;
					}
				} 
				else if(GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellType == SpellObject.SpellType.Self)
				{
					if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().actuallyPlaying].GetComponent<LocalDataHolder> ().fighterIndex == fighterIndex) {
						if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().actuallyPlaying].GetComponent<LocalDataHolder> ().actionPointPlayer > 0) {
							//SelfHeal ();
							StartCoroutine(waitForSpellEffect());
						} else {
							GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().attackMode = false;
						}
					}
				}
			}
		} 
	}

	void GetRidOfIndicatorToSeeWhichEnemyICanClickOn () {
		//make sure for the enemies to not show if they are not dead the fact that you can click on them
		for (int i = 0; i < GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList.Count; i++) {
			if (!GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [i].GetComponent<LocalDataHolder> ().player) {
				GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [i].transform.Find ("Shadow/Pastille2").GetComponent<Image> ().enabled = false;
			}
		}
	}

	void AddEnemyKilled(EnemyObject enemy){
		GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data> ().EnemyCalculEndDungeon (enemy);
	}

	void CalculDamageDone (SpellObject.SpellLogicType spellLogicType) {

		print (spellLogicType);

		if (spellLogicType == SpellObject.SpellLogicType.Damage) {
			GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList[indexFighterToAttack].GetComponent<LocalDataHolder> ().looseLife (-GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellDamage);
		}

		else if (spellLogicType == SpellObject.SpellLogicType.Heal) {
			GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList[indexFighterToAttack].GetComponent<LocalDataHolder> ().looseLife (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellDamage);
		}



		if (health > maxHealth) {
			health = maxHealth;
		}
	}

	void DefineTargetIndex(SpellObject.SpellTargetType spellTargetType){
		
		if (spellTargetType == SpellObject.SpellTargetType.EnemySingle) {
			indexFighterToAttack = fighterIndex;
		} else if (spellTargetType == SpellObject.SpellTargetType.PlayerSingle) {
			indexFighterToAttack = GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().actuallyPlaying;
		}
	}

	void CheckExtraEffect(int alpha){
		if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellTargetFeedbackAnimationType == SpellObject.SpellTargetFeedbackAnimationType.Healed) {
			GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [indexFighterToAttack].transform.Find ("EffectLayer").GetComponent<Image> ().color = new Color (255, 255, 255, alpha);
		}
	}

	void ReduceFromActionPoint(){
		GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList[GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().actuallyPlaying].GetComponent<LocalDataHolder>().actionPointPlayer -= GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellCost;

	}

	void CalculChances(){

		int randChancesToHit = Random.Range (0, 100);

		if (randChancesToHit >= 10) {
			AttackContinue = true;
		} else {
			print ("missed");
		}
	}

	void PlayerAnimationPropreties(){
		//what if we throw a fire ball, we need to say find distance and make the path for the fireball
		GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().actuallyPlaying].transform.Find ("PersoBackground").GetComponent<Animator> ().Play (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellAnimator.ToString());
	}

	void AnimFeedbackEnemy(SpellObject.SpellTargetType spellTarget, bool on){
		if (spellTarget == SpellObject.SpellTargetType.EnemySingle) {
			if (on) {
				this.transform.Find ("EnemyBackground").GetComponent<Animator> ().Play ("DamageMonster");
			} else {
				this.transform.Find ("EnemyBackground").GetComponent<Animator> ().Play ("IdleMonster");
			}
		}
	}

	IEnumerator waitForSpellEffect()
	{
		//define the index of who'll be attacking
		DefineTargetIndex (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellTargetType);

		//check the action point of the player
		ReduceFromActionPoint ();

		//play player animation
		PlayerAnimationPropreties();

		//Wait for anim player to finish depending on time of spell anim time
		yield return new WaitForSeconds (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.SpellCastAnimationTime);

		//calculate the chances to hit or crit == Calculate if "missed" or "critical chance" or "regular spell effect"
		CalculChances ();
		//depending on the result, throw here an inidactor to know if we continue the attack or not.

		if (AttackContinue) {

			if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellLogicType == SpellObject.SpellLogicType.Damage) {
				//if it's damage make the fighter react to taking damages
				AnimFeedbackEnemy (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellTargetType, true);

				// wait for anim enemy reaction to spell. (Constant of 1 sec for exemple) + Launched Hit or Critical animation
				yield return new WaitForSeconds (1.0f);

				AnimFeedbackEnemy (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellTargetType, false);
			}
			//do the damage on the target (healing included)
			CalculDamageDone (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellLogicType);

			//check if the extra effect is != none, so then we need to make an animation for that.
			if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellTargetFeedbackAnimationType != SpellObject.SpellTargetFeedbackAnimationType.None) {
				//wait for anim Feedback Animation on target
				CheckExtraEffect (1);
				yield return new WaitForSeconds (1.0f);
				CheckExtraEffect (0);
			}
		} else {
			// wait for anim enemy reaction to spell. + launch MISSED animation
			yield return new WaitForSeconds (0.5f);
		}
	}
}
