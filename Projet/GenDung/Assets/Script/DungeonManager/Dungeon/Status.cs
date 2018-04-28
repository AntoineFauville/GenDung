using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status {

	public string statusName;
	public int statusDamage;

	public enum StatusType { None, Poisonned, Healed, Sheilded, TemporaryLifed, Cursed, ResistanceReduced, AvoidanceReduced };
	public StatusType statusType;

	public int statusTurnLeft;
}
