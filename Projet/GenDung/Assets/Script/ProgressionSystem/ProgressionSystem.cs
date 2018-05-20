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
	public List<P_Dungeon> all_Dungeons = new List<P_Dungeon> ();

	//Players
	public int amountOfPlayer = 1;
	public List<GameObject> all_Players = new List<GameObject> ();
	public int combinedPower;

	//Gold
	public int gold = 1;

	//LogTool
	string debugLogMessage = "";

	void Start(){
		InitializeDungeon ();
		InitializePlayer ();
		UpdateDisplayHeader ();

		StartCoroutine (autoRefresh());
	}

	void InitializeDungeon(){
		for (int i = 1; i < amountOfDungeon+1; i++) {

			GameObject dungeon;

			dungeon = Instantiate (prefabDungeon, dungeon_Container) as GameObject;
			dungeon.transform.SetParent (dungeon_Container);
			dungeon.name = "Dungeon_" + i;

			P_Dungeon p_Dungeon = new P_Dungeon();

			p_Dungeon.name = "Dungeon_" + i;
			p_Dungeon.power_difficulty = i*i*2;
			p_Dungeon.dungeon_Reward = i*2;
			p_Dungeon.index = i-1;

			all_Dungeons.Add(p_Dungeon);

			dungeon.transform.Find ("Dungeon_Description/Dungeon_Description_Text").GetComponent<Text> ().text = dungeon.name + " Difficulty " + p_Dungeon.power_difficulty + " Reward : " + p_Dungeon.dungeon_Reward;
			dungeon.transform.Find ("Dungeon_Button").GetComponent<Dungeon_Button_Controller> ().p_Dungeon = p_Dungeon;
		}
	}

	void InitializePlayer(){
		for (int i = 1; i < amountOfPlayer+1; i++) {

			GameObject player;

			player = Instantiate (prefabPlayer, player_Container) as GameObject;
			player.name = "Player_" + i;

			P_Player p_Player = new P_Player ();

			p_Player.name = "Player_" + i;
			p_Player.power = 1;
			p_Player.localIndex = i-1;

			all_Players.Add (player);

			player.transform.Find ("Player_Description/Player_Description_Text").GetComponent<Text> ().text = player.name + " Power : " + p_Player.power + " Upgrade Cost : " + p_Player.upgradeCost;
			player.transform.Find ("Player_Upgrade_Button").GetComponent<Player_Button_Controller> ().p_Player = p_Player;
		}

		CalculateOverallPower ();
	}

	public void CalculateOverallPower(){
		combinedPower = 0;

		for (int i = 0; i < all_Players.Count; i++) {
			combinedPower += all_Players [i].transform.Find ("Player_Upgrade_Button").GetComponent<Player_Button_Controller> ().p_Player.power;
		}
	}

	public void AddPower(int power){
		combinedPower += power;
	}

	public void ModifyGold(int earnedGold){
		gold += earnedGold;
	}

	public void UpgradePlayer(P_Player p_PlayerUp){

		if (gold >= p_PlayerUp.upgradeCost) {

			ModifyGold (-p_PlayerUp.upgradeCost);

			p_PlayerUp.power += 1;
			p_PlayerUp.upgradeCost += p_PlayerUp.upgradeCost/2;

			UpdatePlayerDescription (p_PlayerUp);
			CalculateOverallPower ();

			debugLogMessage = "Upgrade SuccessFull !";

		} else {
			Debug.Log ("Grind for upgrades");
			debugLogMessage = "Grind for upgrades";
		}
	}

	public void UpdatePlayerDescription(P_Player p_Player){
		for (int i = 0; i < all_Players.Count; i++) {
			all_Players [p_Player.localIndex].transform.Find ("Player_Description/Player_Description_Text").GetComponent<Text> ().text = p_Player.name + " Power : " + p_Player.power + " Upgrade Cost : " + p_Player.upgradeCost;
		}
	}

	void UpdateDisplayHeader(){
		GameObject.Find ("Power_Text_Amount").GetComponent<Text> ().text = combinedPower.ToString ();
		GameObject.Find ("Gold_Text_Amount").GetComponent<Text> ().text = gold.ToString ();

		GameObject.Find ("Debug_Message_Text").GetComponent<Text> ().text = debugLogMessage;
	}

	public void ExploreDung(P_Dungeon p_dung){
		if (p_dung.power_difficulty <= combinedPower) {
			//yepee
			ModifyGold (p_dung.dungeon_Reward);

			debugLogMessage = "Exploration SuccessFull !";

			if (p_dung.index+1 == all_Dungeons.Count) {
				debugLogMessage = "You Won !!";
			}
		} else {
			Debug.Log ("keep grinding");
			debugLogMessage = "Keep grinding";
		}


	}

	IEnumerator autoRefresh(){

		UpdateDisplayHeader ();

		yield return new WaitForSeconds (0.1f);

		StartCoroutine (autoRefresh());
	}
}
