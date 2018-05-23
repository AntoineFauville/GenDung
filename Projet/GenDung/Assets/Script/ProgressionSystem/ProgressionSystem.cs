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
	public List<Pdungeon> all_Dungeons = new List<Pdungeon> ();

	//Players
	public int amountOfPlayer = 1;
	public List<GameObject> all_Players = new List<GameObject> ();
	public float combinedPower;

	//Gold
	public IValueSystem Money = new ValueSystem();

	//LogTool
	string debugLogMessage = "";

	private void Start(){
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

		    Pdungeon pDungeon = new Pdungeon();

			pDungeon.Name = "Dungeon_" + i;
			pDungeon.Index = i-1;

			pDungeon.Difficulty.SetValueTo (i*i*2);
			pDungeon.Rewards.SetValueTo (i*2);
			print(pDungeon.Rewards.Value);

			all_Dungeons.Add(pDungeon);

			dungeon.transform.Find ("Dungeon_Description/Dungeon_Description_Text").GetComponent<Text> ().text = dungeon.name + " Difficulty " + pDungeon.Difficulty.Value + " Reward : " + pDungeon.Rewards.Value;
			dungeon.transform.Find ("Dungeon_Button").GetComponent<Dungeon_Button_Controller> ().pDungeon = pDungeon;
		}
	}

	void InitializePlayer(){
		for (int i = 1; i < amountOfPlayer+1; i++) {

			GameObject player;

			player = Instantiate (prefabPlayer, player_Container) as GameObject;
			player.name = "Player_" + i;

			Pplayer pPlayer = new Pplayer();

			pPlayer.Name = "Player_" + i;
			pPlayer.PlayerPower.SetValueTo(1);
			pPlayer.UpgradeCost.SetValueTo (1);
			pPlayer.LocalIndex = i-1;

			all_Players.Add (player);

			player.transform.Find ("Player_Description/Player_Description_Text").GetComponent<Text> ().text = player.name + " Power : " + pPlayer.PlayerPower.Value + " Upgrade Cost : " + pPlayer.UpgradeCost.Value;
			player.transform.Find ("Player_Upgrade_Button").GetComponent<Player_Button_Controller> ().pPlayer = pPlayer;
		}

		CalculateOverallPower ();
	}

	public void CalculateOverallPower(){
		combinedPower = 0;

		for (int i = 0; i < all_Players.Count; i++) {
			combinedPower += all_Players [i].transform.Find ("Player_Upgrade_Button").GetComponent<Player_Button_Controller> ().pPlayer.PlayerPower.Value;
		}
	}

	public void AddPower(float power){
		combinedPower += power;
	}

	public void ModifyGold(float earnedGold){
		Money.ModifyValue (earnedGold);
	}

	public void UpgradePlayer(Pplayer pplayerUp){

		if (Money.Value >= pplayerUp.UpgradeCost.Value) {

			ModifyGold (-pplayerUp.UpgradeCost.Value);

		    pplayerUp.PlayerPower.ModifyValue(1);
		    pplayerUp.UpgradeCost.ValuePowered(2);

			UpdatePlayerDescription (pplayerUp);
			CalculateOverallPower ();

			debugLogMessage = "Upgrade SuccessFull !";

		} else {
			Debug.Log ("Grind for upgrades");
			debugLogMessage = "Grind for upgrades";
		}
	}

	public void UpdatePlayerDescription(Pplayer pplayer){
		for (int i = 0; i < all_Players.Count; i++) {
			all_Players [pplayer.LocalIndex].transform.Find ("Player_Description/Player_Description_Text").GetComponent<Text> ().text = pplayer.Name + " Power : " + pplayer.PlayerPower.Value + " Upgrade Cost : " + pplayer.UpgradeCost.Value;
		}
	}

	void UpdateDisplayHeader(){
		GameObject.Find ("Power_Text_Amount").GetComponent<Text> ().text = combinedPower.ToString ();
		GameObject.Find ("Gold_Text_Amount").GetComponent<Text> ().text = Money.Value.ToString ();

		GameObject.Find ("Debug_Message_Text").GetComponent<Text> ().text = debugLogMessage;
	}

	public void ExploreDung(Pdungeon pdung){
		if (pdung.Difficulty.Value <= combinedPower) {
			//yepee
			ModifyGold (pdung.Rewards.Value);

			debugLogMessage = "Exploration SuccessFull !";

			if (pdung.Index+1 == all_Dungeons.Count) {
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
