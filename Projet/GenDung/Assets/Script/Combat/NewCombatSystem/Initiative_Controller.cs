using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Initiative_Controller : MonoBehaviour {

	[Header("Initiative")]
	public Dictionary<GameObject, int> UnOrderedFighterList = new Dictionary<GameObject, int>();

	BattleSystem BS;
	PlayerSetup_Controller playerSetup_Controller;
	EnemySetup_Controller enemySetup_Controller;

	GameObject scriptBattleHolder;

	// Use this for initialization
	void Start () {
		scriptBattleHolder = GameObject.Find ("BattleSystem/ScriptBattle");

		BS = scriptBattleHolder.GetComponent<BattleSystem> ();
		playerSetup_Controller = scriptBattleHolder.GetComponent<PlayerSetup_Controller> ();
		enemySetup_Controller = scriptBattleHolder.GetComponent<EnemySetup_Controller> (); 
	}

	public void SetFighterIndex(){

		BS.FighterList = UnOrderedFighterList.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();

		for (int i = 0; i < BS.FighterList.Count; i++) {
			BS.FighterList[i].GetComponent<LocalDataHolder> ().fighterIndex = i;

			GameObject UiBattleDisplay;

			UiBattleDisplay = Instantiate(Resources.Load("UI_Interface/UIBattleOrderDisplay"), GameObject.Find ("OrderBattlePanel").transform) as GameObject;

			BS.FighterList [i].GetComponent<LocalDataHolder> ().UiOrderObject = UiBattleDisplay;
			BS.FighterList [i].GetComponent<ToolTipStatus_Controller> ().start ();
		}

		//hide the others and make the initializer work
		for (int i = 0; i < 4; i++) {
			GameObject.Find (enemySetup_Controller.enemyString + i).GetComponent<LocalDataHolder> ().Initialize();
			GameObject.Find	(playerSetup_Controller.playerString + i).GetComponent<LocalDataHolder> ().Initialize();
		}

		//make sure for the enemies to not show if they are not dead the fact that you can click on them
		for (int i = 0; i < BS.FighterList.Count; i++) 
		{
			if (!BS.FighterList [i].GetComponent<LocalDataHolder> ().player) 
			{
				BS.FighterList [i].transform.Find ("Shadow/Pastille2").GetComponent<Image> ().enabled = false;
			}
		}
	}
}
