using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalDataHolder : MonoBehaviour {

	public bool player;
	public bool amIPlaying;
	public bool dead;

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

	private Status status;

	BattleSystem BS;
	SavingSystem saveSystem;
	Explo_DataController explo_Data;
	EffectController effect_Controller;
	ProjectileManager projectile_Manager;
	TextLooseLife_Controller tLL_Controller;

	GameObject scriptBattleHolder;
	GameObject dontDestroyOnLoad;
	Transform Background;

	// Use this for initialization
	public void Initialize () {

		scriptBattleHolder = GameObject.Find ("BattleSystem/ScriptBattle");
		dontDestroyOnLoad = GameObject.Find ("DontDestroyOnLoad");

		saveSystem = dontDestroyOnLoad.GetComponent<SavingSystem> ();
		explo_Data = dontDestroyOnLoad.GetComponent<Explo_DataController> ();

		BS = scriptBattleHolder.GetComponent<BattleSystem> ();
		effect_Controller = dontDestroyOnLoad.GetComponent<EffectController> ();
		projectile_Manager = scriptBattleHolder.GetComponent<ProjectileManager> ();

		tLL_Controller = this.GetComponent<TextLooseLife_Controller> ();

		if (!player) {
			Background = this.transform.Find ("EnemyBackground");
		} else {
			Background = this.transform.Find("PersoBackground");
		}

		//if at the start and the enemyObject and the character Object are empty, it means we are not been selected by the holy church.
		//you need to die.

		if (enemyObject == null && characterObject == null) {

			print (this.gameObject.name + " has died, sorry");

			this.gameObject.transform.SetParent (GameObject.Find("BackupInvocationsEnemies").transform);

		} else {

			if(!player){
				this.gameObject.transform.SetParent(GameObject.Find("EnemyPanelPlacement").transform);
				Background.GetComponent<Image> ().sprite = enemyObject.enemyIcon;
				maxHealth = enemyObject.health;
				health = maxHealth;

				this.transform.Find("EffectLayer").GetComponent<Animator>().Play("Effect_None");
			} else {
				maxHealth = saveSystem.gameData.SavedCharacterList [localIndex].Health_PV;
				health = explo_Data.dungeonData.TempFighterObject [localIndex].tempHealth;
				Background.GetComponent<Image> ().sprite = characterObject.ICON;
				this.transform.Find("EffectLayer").GetComponent<Animator>().Play("Effect_None");


				maxActionPointPlayer = characterObject.ActionPoints_PA;
				actionPointPlayer = maxActionPointPlayer;
			}

			transform.Find ("LifeControl/LifeBar").GetComponent<Image> ().fillAmount = health / maxHealth;

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
				explo_Data.dungeonData.TempFighterObject [localIndex].died = true;
				Background.GetComponent<Button> ().enabled = false;
				Background.GetComponent<Image> ().color = Color.gray;
			} else {
				explo_Data.dungeonData.TempFighterObject [localIndex+4].died = true;
				Background.GetComponent<Button> ().enabled = false;
				Background.GetComponent<Animator> ().Play ("Death");
			}


			if (player) {
				BS.amountOfPlayerLeft--;
				explo_Data.amountOfPlayerLeft--; //renvoie a l'explo data qu'un est mort pour l'initialization lors du deuxieme combat.
				if (BS.amountOfPlayerLeft <= 0)
					BS.EndBattleAllPlayerDead();

			} else {

				AddEnemyKilled (enemyObject);

				BS.amountOfEnemiesLeft--;
				if (BS.amountOfEnemiesLeft <= 0)
					BS.EndBattleAllMonsterDead();
			}
		}

		SetupUiOrderObject ();

		tLL_Controller.lifeTextUp (pv);
	}

	public void SetupUiOrderObject () 
	{
		if(player){
			UiOrderObject.transform.Find("MASK/PlayerRepresentation").GetComponent<Image>().sprite = characterObject.ICON;
			UiOrderObject.transform.Find ("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayName").GetComponent<Text> ().text = characterObject.Name.ToString();
			UiOrderObject.transform.Find ("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPV").GetComponent<Text> ().text = "PV = " + health.ToString() + " / " + characterObject.Health_PV.ToString();
			UiOrderObject.transform.Find ("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPA").GetComponent<Text> ().text = "PA = " + actionPointPlayer.ToString() + " / " + characterObject.ActionPoints_PA.ToString();
		} else {
			UiOrderObject.transform.Find("MASK/PlayerRepresentation").GetComponent<Image>().sprite = enemyObject.enemyIcon;
			UiOrderObject.transform.Find ("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayName").GetComponent<Text> ().text = enemyObject.enemyName.ToString();
			UiOrderObject.transform.Find ("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPV").GetComponent<Text> ().text = "PV = " + health.ToString() + " / " + enemyObject.health.ToString();
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
		transform.Find ("LifeControl/LifeBar").GetComponent<Image> ().fillAmount = health / maxHealth;
		UiOrderObject.transform.Find("PVOrderDisplay").GetComponent<Image> ().fillAmount = health / maxHealth;

		if (player) {
			explo_Data.dungeonData.TempFighterObject [localIndex].tempHealth = health;
		}
	}

	public void AttackEnemy(){

		if (BS.attackMode) 
		{
			if (!BS.FighterList [fighterIndex].GetComponent<LocalDataHolder> ().player) 
			{
				//check to know on who I can click.
				if (BS.SelectedSpellObject.spellType == SpellObject.SpellType.Enemy) {
					//check if the actual player that wants to do the spell can launch the spell
					if (BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().actionPointPlayer > 0) {
						StartCoroutine(waitForSpellEffect());
						//Damage ();
					} else {
						BS.attackMode = false;
					}

					GetRidOfIndicatorToSeeWhichEnemyICanClickOn ();

				}
			} 
			else 
			{
				//check to know on who I can click.
				if (BS.SelectedSpellObject.spellType == SpellObject.SpellType.Ally) 
				{
					if (BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().actionPointPlayer > 0) {
						//do something to all the allies
						StartCoroutine(waitForSpellEffect());
					} else {
						BS.attackMode = false;
					}
				} 
				else if(BS.SelectedSpellObject.spellType == SpellObject.SpellType.Self)
				{
					if (BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().fighterIndex == fighterIndex) {
						if (BS.FighterList [BS.actuallyPlaying].GetComponent<LocalDataHolder> ().actionPointPlayer > 0) {
							//SelfHeal ();
							StartCoroutine(waitForSpellEffect());
						} else {
							BS.attackMode = false;
						}
					}
				}
			}
		} 
	}

	void GetRidOfIndicatorToSeeWhichEnemyICanClickOn () {
		//make sure for the enemies to not show if they are not dead the fact that you can click on them
		for (int i = 0; i < BS.FighterList.Count; i++) {
			if (!BS.FighterList [i].GetComponent<LocalDataHolder> ().player) {
				BS.FighterList [i].transform.Find ("Shadow/Pastille2").GetComponent<Image> ().enabled = false;
			}
		}
	}

	void AddEnemyKilled(EnemyObject enemy){
		explo_Data.EnemyCalculEndDungeon (enemy);
	}

	void CalculDamageDone (SpellObject.SpellLogicType spellLogicType) {

		print (spellLogicType);

		if (spellLogicType == SpellObject.SpellLogicType.Damage) {
			BS.FighterList[indexFighterToAttack].GetComponent<LocalDataHolder> ().looseLife (-BS.SelectedSpellObject.spellDamage);
		}

		else if (spellLogicType == SpellObject.SpellLogicType.Heal) {
			BS.FighterList[indexFighterToAttack].GetComponent<LocalDataHolder> ().looseLife (BS.SelectedSpellObject.spellDamage);
		}



		if (health > maxHealth) {
			health = maxHealth;
		}
	}

	void DefineTargetIndex(SpellObject.SpellTargetType spellTargetType){

		if (spellTargetType == SpellObject.SpellTargetType.EnemySingle) {
			indexFighterToAttack = fighterIndex;
		} else if (spellTargetType == SpellObject.SpellTargetType.PlayerSingle) {
			indexFighterToAttack = BS.actuallyPlaying;
		}
	}

	void AssignEffect(int index){

		status = new Status ();

		status.statusName = effect_Controller.AllStatus [index].statusName;
		status.statusDamage = effect_Controller.AllStatus [index].statusDamage;
		status.statusTurnLeft = (int)BS.SelectedSpellObject.spellOccurenceType;

		print (status.statusName + " " + status.statusDamage + " " + " " + status.statusTurnLeft);

		//assign effect type

		if (index == 0) {
			status.statusType = Status.StatusType.Healed;
		} else if(index == 1) {
			status.statusType = Status.StatusType.Poisonned;
		}else if(index == 2) {
			status.statusType = Status.StatusType.Spike;
		}else if(index == 3) {
			status.statusType = Status.StatusType.TemporaryLifed;
		}

		if (player) {
			explo_Data.dungeonData.TempFighterObject [localIndex].playerStatus.Add (status);
		} else {
			explo_Data.dungeonData.TempFighterObject [localIndex+4].playerStatus.Add (status);
		}


	}

	void CheckDuringCombatEffect(bool playEffect){

		Animator anim;
		anim = BS.FighterList [indexFighterToAttack].transform.Find ("EffectLayer").GetComponent<Animator> ();

		if (BS.SelectedSpellObject.spellTargetEffectAppearing == SpellObject.SpellTargetEffectAppearing.Spike && playEffect) 
		{
			anim.Play(effect_Controller.effect_List[3]);

		}
		else if (BS.SelectedSpellObject.spellTargetEffectAppearing == SpellObject.SpellTargetEffectAppearing.Roots && playEffect)
		{
			anim.Play(effect_Controller.effect_List[4]);
		}
		else if (BS.SelectedSpellObject.spellTargetEffectAppearing == SpellObject.SpellTargetEffectAppearing.ProjectileVic && playEffect)
		{
			projectile_Manager.LaunchProjectile (BS.FighterList[BS.actuallyPlaying].transform,BS.FighterList[indexFighterToAttack].transform);
		}
		else if (!playEffect) 
		{
			anim.Play(effect_Controller.effect_List[0]);
		}
	}

	void CheckExtraEffect(bool playEffect){

		Animator animExtra;
		animExtra = BS.FighterList [indexFighterToAttack].transform.Find ("EffectLayer").GetComponent<Animator> ();

		if (BS.SelectedSpellObject.spellTargetFeedbackAnimationType == SpellObject.SpellTargetFeedbackAnimationType.Healed && playEffect)
		{
			animExtra.Play(effect_Controller.effect_List[1]);

			//DOTS
			if (BS.SelectedSpellObject.spellOccurenceType == SpellObject.SpellOccurenceType.NoTurn) {

			} else {
				AssignEffect (0); //0 = healing
			}

		} else if (BS.SelectedSpellObject.spellTargetFeedbackAnimationType == SpellObject.SpellTargetFeedbackAnimationType.Poisonned && playEffect)
		{

			animExtra.Play(effect_Controller.effect_List[2]);
			//BS.FighterList [indexFighterToAttack].transform.Find ("EffectLayer").GetComponent<Image> ().color = new Color (255, 255, 255, alpha);

			//DOTS
			if (BS.SelectedSpellObject.spellOccurenceType == SpellObject.SpellOccurenceType.NoTurn) {

			} else {
				AssignEffect (1); //1 = poisoning
			}

		} else if (BS.SelectedSpellObject.spellTargetFeedbackAnimationType == SpellObject.SpellTargetFeedbackAnimationType.Spike && playEffect)
		{

			animExtra.Play(effect_Controller.effect_List[3]);
			//BS.FighterList [indexFighterToAttack].transform.Find ("EffectLayer").GetComponent<Image> ().color = new Color (255, 255, 255, alpha);

			//DOTS
			if (BS.SelectedSpellObject.spellOccurenceType == SpellObject.SpellOccurenceType.NoTurn) {

			} else {
				AssignEffect (2); //1 = poisoning
			}

		}else if (BS.SelectedSpellObject.spellTargetFeedbackAnimationType == SpellObject.SpellTargetFeedbackAnimationType.TemporaryLifed && playEffect)
		{

			//DOTS
			if (health >= maxHealth) {

			}else{
				if (BS.SelectedSpellObject.spellOccurenceType == SpellObject.SpellOccurenceType.NoTurn) {

				} else {
					AssignEffect (3); 
				}
			}

		}
		else if (!playEffect) {
			animExtra.Play(effect_Controller.effect_List[0]);
		}
	}

	void ReduceFromActionPoint(){
		BS.FighterList[BS.actuallyPlaying].GetComponent<LocalDataHolder>().actionPointPlayer -= BS.SelectedSpellObject.spellCost;

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
		BS.FighterList [BS.actuallyPlaying].transform.Find ("PersoBackground").GetComponent<Animator> ().Play (BS.SelectedSpellObject.spellAnimator.ToString());
	}

	void AnimFeedbackEnemy(SpellObject.SpellTargetType spellTarget, bool on){

		Animator EnemyAnimator = this.transform.Find ("EnemyBackground").GetComponent<Animator> ();

		if (spellTarget == SpellObject.SpellTargetType.EnemySingle) {
			if (on) {
				EnemyAnimator.Play ("DamageMonster");
			} else {
				EnemyAnimator.Play ("IdleMonster");
			}
		}
	}

	IEnumerator waitForSpellEffect()
	{
		//define the index of who'll be attacking
		DefineTargetIndex (BS.SelectedSpellObject.spellTargetType);

		//check the action point of the player
		ReduceFromActionPoint ();

		//play player animation
		PlayerAnimationPropreties();

		//Wait for anim player to finish depending on time of spell anim time
		//if it contains a reaction or a spell invocation at the enemy's place we need to instantiate or play an effect on the enemy
		if(BS.SelectedSpellObject.EffectAppearingDuringPlayerAnim){
			yield return new WaitForSeconds (BS.SelectedSpellObject.SpellCastAnimationTime/2);

			CheckDuringCombatEffect (true);

			yield return new WaitForSeconds (BS.SelectedSpellObject.SpellCastAnimationTime/2);

			CheckDuringCombatEffect(false);
		}
		else
		{
			yield return new WaitForSeconds (BS.SelectedSpellObject.SpellCastAnimationTime);
		}

		//calculate the chances to hit or crit == Calculate if "missed" or "critical chance" or "regular spell effect"
		CalculChances ();
		//depending on the result, throw here an inidactor to know if we continue the attack or not.

		if (AttackContinue) {

			if (BS.SelectedSpellObject.spellLogicType == SpellObject.SpellLogicType.Damage) {
				//if it's damage make the fighter react to taking damages
				AnimFeedbackEnemy (BS.SelectedSpellObject.spellTargetType, true);

				// wait for anim enemy reaction to spell. (Constant of 1 sec for exemple) + Launched Hit or Critical animation
				yield return new WaitForSeconds (1.0f);

				AnimFeedbackEnemy (BS.SelectedSpellObject.spellTargetType, false);
			}
			//do the damage on the target (healing included)
			CalculDamageDone (BS.SelectedSpellObject.spellLogicType);

			//check if the extra effect is != none, so then we need to make an animation for that.
			if (BS.SelectedSpellObject.spellTargetFeedbackAnimationType != SpellObject.SpellTargetFeedbackAnimationType.None) {
				//wait for anim Feedback Animation on target
				CheckExtraEffect (true);
				yield return new WaitForSeconds (0.8f);
				CheckExtraEffect (false);
			}
		} else {
			// wait for anim enemy reaction to spell. + launch MISSED animation
			yield return new WaitForSeconds (0.5f);
		}
	}
}
