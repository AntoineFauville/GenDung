using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueSystem : IValueSystem {

	public float value2;

	public float value {
		get {
			return Mathf.Round(value2);
		}
	}

	public void ModifyValue (float modifiedValue){
		value2 += modifiedValue;
	}

	public void SetValueTo (float valueAmount)
	{
		value2 = valueAmount;
	}

	public void ValuePowered (float factor){
		value2 += value2 / factor;
	}
}
