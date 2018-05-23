using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pdungeon {

	public string Name;

	public int Index;

	public IValueSystem Difficulty = new ValueSystem();

	public IValueSystem Rewards = new ValueSystem();
}
