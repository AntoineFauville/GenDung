using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pplayer {

	public string Name;

	public int LocalIndex;

	public IValueSystem PlayerPower = new ValueSystem();

	// define the cost for the players upgrade
	public IValueSystem UpgradeCost = new ValueSystem();
}
