using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalDataHolder : MonoBehaviour {

	public bool player, amIPlaying;

	public EnemyObject enemyObject;

	public Character characterObject;

	// Use this for initialization
	void Start () {
		if(!player){
			this.GetComponent<Image> ().sprite = this.GetComponent<LocalDataHolder> ().enemyObject.enemyIcon;
		} else {
			this.GetComponent<Image> ().sprite = this.GetComponent<LocalDataHolder> ().characterObject.ICON;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
