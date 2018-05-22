using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Dungeon {

	public string name;

	public int index;

	public IValueSystem difficulty = new ValueSystem();

	public IValueSystem rewards = new ValueSystem();
}
