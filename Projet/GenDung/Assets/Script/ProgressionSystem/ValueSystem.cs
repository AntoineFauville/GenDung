using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueSystem {

	public float Value;

	public void ModifyValue (float modifiedValue){
		Value += modifiedValue;
	}

	public void SetValueTo (float valueAmount)
	{
		Value = valueAmount;
	}

	public void ValuePowered (float factor){
		Value += Value / factor;
	}

    public void ValuePowered(float dividedByFactor, float amountToAdd)
    {
		Value += amountToAdd / dividedByFactor;
    }

    public ValueSystem(float value = 0)
    {
        Value = value;
    }
}
