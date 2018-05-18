using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionSystem : MonoBehaviour {

	//prefabs
	public GameObject prefabPlayer; 
	public GameObject prefabDungeon;

	public Transform dungeon_Container;
	public Transform player_Container;

	//dungeons
	public int amountOfDungeon = 1;
	public int power_difficulty = 1;

	//Players
	public int amountOfPlayer = 1;
	public int power = 1;
	public int combinedPower;

	//Gold
	public int gold = 1;

	void Start(){
		InitializeDungeon ();
		InitializePlayer ();
		InitialiseHeader ();
	}

	void InitializeDungeon(){
		for (int i = 1; i < amountOfDungeon+1; i++) {

			GameObject dungeon;

			dungeon = Instantiate (prefabDungeon, dungeon_Container) as GameObject;

			dungeon.transform.SetParent (dungeon_Container);

			dungeon.name = "Dungeon_" + i;

			dungeon.transform.Find ("Dungeon_Description/Dungeon_Description_Text").GetComponent<Text> ().text = dungeon.name + " Difficulty " + i * 3 + " Reward : " + i*2;
		}
	}

	void InitializePlayer(){
		for (int i = 1; i < amountOfPlayer+1; i++) {

			GameObject player;

			player = Instantiate (prefabPlayer, player_Container) as GameObject;

			player.name = "Player_" + i;

			AddPower (power);

			player.transform.Find ("Player_Description/Player_Description_Text").GetComponent<Text> ().text = player.name + " Power : " + power;
		}
	}

	void AddPower(int power){
		combinedPower += power;
	}

	void InitialiseHeader(){
		GameObject.Find ("Power_Text_Amount").GetComponent<Text> ().text = combinedPower.ToString ();
		GameObject.Find ("Gold_Text_Amount").GetComponent<Text> ().text = gold.ToString ();
	}
}
