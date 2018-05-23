using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueSystem : IValueSystem {

	public float Value2;

	public float Value {
		get {
			return Mathf.Round(Value2);
		}
	}

	public void ModifyValue (float modifiedValue){
	    Value2 += modifiedValue;
	}

	public void SetValueTo (float valueAmount)
	{
	    Value2 = valueAmount;
	}

	public void ValuePowered (float factor){
	    Value2 += Value2 / factor;
	}

    public void ValuePowered(float dividedByFactor, float amountToAdd)
    {
        Value2 += amountToAdd / dividedByFactor;
    }
}
