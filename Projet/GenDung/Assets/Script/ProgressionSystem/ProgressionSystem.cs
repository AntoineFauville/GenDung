using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionSystem : MonoBehaviour {

	//dungeons
	public ProgressionSetUpDungeon ProgressionSetUpDungeon;
	public List<DungeonButtonController> all_Dungeons_Controllers = new List<DungeonButtonController> ();

	//Players
	public ProgressionSetUpPlayer ProgressionSetUpPlayer;
	public List<PlayerButtonController> all_Players_Controllers = new List<PlayerButtonController> ();
	float combinedPower;

	//Gold
	public ValueSystem Money = new ValueSystem();

	//LogTool
	string debugLogMessage = "";
	public Text Power_Text_Amount;
	public Text Gold_Text_Amount;
	public Text Debug_Message_Text;


	private void Start(){
		ProgressionSetUpDungeon.InitializeDungeon (this);
		ProgressionSetUpPlayer.InitializePlayer (this);

		UpdateDisplayHeader ();

		StartCoroutine (autoRefresh());
	}

	public void CalculateOverallPower(){
		combinedPower = 0;

		for (int i = 0; i < all_Players_Controllers.Count; i++) {
			combinedPower += all_Players_Controllers [i].LocalPPlayer.PlayerPower.Value;
		}
	}

	public void AddPower(float power){
		combinedPower += power;
	}

	public void UpgradePlayer(Pplayer pPlayerUp){

		if (Money.Value >= pPlayerUp.UpgradeCost.Value) {

			Money.ModifyValue (-pPlayerUp.UpgradeCost.Value);

			pPlayerUp.PlayerPower.ModifyValue(1);
			pPlayerUp.UpgradeCost.ValuePowered(2);

			UpdatePlayerDescription (pPlayerUp);
			CalculateOverallPower ();

			debugLogMessage = "Upgrade SuccessFull !";

		} else {
			Debug.Log ("Grind for upgrades");
			debugLogMessage = "Grind for upgrades";
		}
	}

	public void UpdatePlayerDescription(Pplayer pPlayer){
		for (int i = 0; i < all_Players_Controllers.Count; i++) {
			all_Players_Controllers [pPlayer.LocalIndex].PlayerDescriptionText.text = pPlayer.Name + " Power : " + pPlayer.PlayerPower.Value + " Upgrade Cost : " + pPlayer.UpgradeCost.Value;
		}
	}

	void UpdateDisplayHeader(){
		Power_Text_Amount.text = combinedPower.ToString ();
		Gold_Text_Amount.text = Money.Value.ToString ();
		Debug_Message_Text.text = debugLogMessage;
	}

	public void ExploreDung(Pdungeon pdung){
		if (pdung.Difficulty.Value <= combinedPower) {
			//yepee
			Money.ModifyValue (pdung.Rewards.Value);

			debugLogMessage = "Exploration SuccessFull !";

			if (pdung.Index+1 == all_Dungeons_Controllers.Count) {
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
