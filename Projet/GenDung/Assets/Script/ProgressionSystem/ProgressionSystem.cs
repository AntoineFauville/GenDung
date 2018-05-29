using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionSystem : MonoBehaviour {

	//dungeons
	public ProgressionSetUpDungeon ProgressionSetUpDungeon;
	public List<DungeonButtonController> AllDungeonsControllers = new List<DungeonButtonController> ();

	//Players
	public ProgressionSetUpPlayer ProgressionSetUpPlayer;
	public List<PlayerButtonController> AllPlayersControllers = new List<PlayerButtonController> ();
	float _combinedPower;

	//Gold
	public ValueSystem Money = new ValueSystem();

	//LogTool
	string _debugLogMessage = "";
	public Text PowerTextAmount;
	public Text GoldTextAmount;
	public Text DebugMessageText;


	private void Start(){
		ProgressionSetUpDungeon.InitializeDungeon (this);
		ProgressionSetUpPlayer.InitializePlayer (this);

		UpdateDisplayHeader ();

		StartCoroutine (autoRefresh());
	}

	public void CalculateOverallPower(){
		_combinedPower = 0;

		for (int i = 0; i < AllPlayersControllers.Count; i++) {
			_combinedPower += AllPlayersControllers [i].LocalPPlayer.PlayerPower.Value;
		}
	}

	public void AddPower(float power){
		_combinedPower += power;
	}

	public void UpgradePlayer(Pplayer pPlayerUp){

		if (Money.Value >= pPlayerUp.UpgradeCost.Value) {

			Money.ModifyValue (-pPlayerUp.UpgradeCost.Value);

			pPlayerUp.PlayerPower.ModifyValue(1);
			pPlayerUp.UpgradeCost.ValuePowered(2);

			UpdatePlayerDescription (pPlayerUp);
			CalculateOverallPower ();

			_debugLogMessage = "Upgrade SuccessFull !";

		} else {
			Debug.Log ("Grind for upgrades");
			_debugLogMessage = "Grind for upgrades";
		}
	}

	public void UpdatePlayerDescription(Pplayer pPlayer){
		for (int i = 0; i < AllPlayersControllers.Count; i++) {
			AllPlayersControllers [pPlayer.LocalIndex].PlayerDescriptionText.text = pPlayer.Name + " Power : " + pPlayer.PlayerPower.Value + " Upgrade Cost : " + pPlayer.UpgradeCost.Value;
		}
	}

	void UpdateDisplayHeader(){
		PowerTextAmount.text = _combinedPower.ToString ();
		GoldTextAmount.text = Money.Value.ToString ();
		DebugMessageText.text = _debugLogMessage;
	}

	public void ExploreDung(ProgressionDungeon pdung){
		if (pdung.Difficulty.Value <= _combinedPower) {
			//yepee
			Money.ModifyValue (pdung.Rewards.Value);

			_debugLogMessage = "Exploration SuccessFull !";

			if (pdung.Index+1 == AllDungeonsControllers.Count) {
				_debugLogMessage = "You Won !!";
			}
		} else {
			Debug.Log ("keep grinding");
			_debugLogMessage = "Keep grinding";
		}
	}

	IEnumerator autoRefresh(){

		UpdateDisplayHeader ();

		yield return new WaitForSeconds (0.1f);

		StartCoroutine (autoRefresh());
	}
}
