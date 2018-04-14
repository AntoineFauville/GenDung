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

	// Use this for initialization
	void Start () {
		if(!player){
			this.GetComponent<Image> ().sprite = this.GetComponent<LocalDataHolder> ().enemyObject.enemyIcon;
			maxHealth = this.GetComponent<LocalDataHolder> ().enemyObject.health;
		} else {
			maxHealth = this.GetComponent<LocalDataHolder> ().characterObject.Health_PV;
			this.GetComponent<Image> ().sprite = this.GetComponent<LocalDataHolder> ().characterObject.ICON;
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

			this.gameObject.GetComponent<Button> ().enabled = false;
			this.gameObject.GetComponent<Image> ().color = Color.gray;
		}

		transform.Find ("LifeBar").GetComponent<Image> ().fillAmount = health / maxHealth;
	}
}
