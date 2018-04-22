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
		if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().attackMode) {
			if (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().actuallyPlaying].GetComponent<LocalDataHolder> ().actionPointPlayer > 0) {
				Damage ();
			} else {
				GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().attackMode = false;
			}

			//make sure for the enemies to not show if they are not dead the fact that you can click on them
			for (int i = 0; i < GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList.Count; i++) 
			{
				if (!GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [i].GetComponent<LocalDataHolder> ().player) 
				{
					GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [i].transform.Find ("Shadow/Pastille2").GetComponent<Image> ().enabled = false;
				}
			}
		} 
	}

	void Damage(){
		//print ("enemy lost " + GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellDamage);
			
		GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList[GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().actuallyPlaying].GetComponent<LocalDataHolder>().actionPointPlayer -= GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellCost;
	
		GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList [GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().actuallyPlaying].transform.Find ("PersoBackground").GetComponent<Animator> ().Play (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellAnimator.ToString());

		StartCoroutine (waitForAnimToProc());
	}


	void AddEnemyKilled(EnemyObject enemy){
		GameObject.Find ("DontDestroyOnLoad").GetComponent<Explo_Data> ().EnemyCalculEndDungeon (enemy);
	}

	IEnumerator waitForAnimToProc() {
		yield return new WaitForSeconds (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.SpellCastAnimationTime);

		this.transform.Find("EnemyBackground").GetComponent<Animator> ().Play ("DamageMonster");

		yield return new WaitForSeconds (0.5f);

		this.transform.Find("EnemyBackground").GetComponent<Animator> ().Play ("IdleMonster");

		GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().FighterList[fighterIndex].GetComponent<LocalDataHolder> ().looseLife (GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().SelectedSpellObject.spellDamage);
	}
}
