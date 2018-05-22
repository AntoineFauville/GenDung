using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Player {

	public string name;

	public int localIndex;

	public IValueSystem playerPower = new ValueSystem();

	// define the cost for the players upgrade
	public IValueSystem upgradeCost = new ValueSystem();
}
