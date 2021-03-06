﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionSetUpPlayer : MonoBehaviour {

	public PlayerButtonController prefabPlayer; 
	ProgressionSystem ProgressionSystem;
	public Transform player_Container;


	public ProgressionGameData PGameData;

	public void InitializePlayer(ProgressionSystem ProgressionSystem){
		for (int i = 1; i <= PGameData.AmountOfPlayer; i++) {

			PlayerButtonController player;

			player = Instantiate (prefabPlayer, player_Container);
			player.name = "Player_" + i;
			player.ProgressionSystem = ProgressionSystem;

			Pplayer pPlayer = new Pplayer();

			pPlayer.Name = "Player_" + i;
			pPlayer.PlayerPower.SetValueTo(1);
			pPlayer.UpgradeCost.SetValueTo (1);
			pPlayer.LocalIndex = i-1;

			ProgressionSystem.AllPlayersControllers.Add (player);

			player.PlayerDescriptionText.text = player.name + " Power : " + pPlayer.PlayerPower.Value + " Upgrade Cost : " + pPlayer.UpgradeCost.Value;
			player.LocalPPlayer = pPlayer;
		}

		ProgressionSystem.CalculateOverallPower ();
	}
}
