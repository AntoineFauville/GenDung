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
	}

	public void looseLife(int pv){
		if(health > 0) {
			health -= pv;
		}
		UpdateLife ();
	}
	
	public void UpdateLife(){
		
		if (health <= 0) {
			
			dead = true;

			//can't interact with me anymore no attacks, no clicking + visual to show i'm dead
			this.gameObject.GetComponent<Button> ().enabled = false;
			this.gameObject.GetComponent<Image> ().color = Color.gray;

			if (player) {
				GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().amountOfPlayerLeft--;
			} else {
				GameObject.Find ("ScriptBattle").GetComponent<BattleSystem> ().amountOfEnemiesLeft--;
			}
		}

		transform.Find ("LifeBar").GetComponent<Image> ().fillAmount = health / maxHealth;
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
