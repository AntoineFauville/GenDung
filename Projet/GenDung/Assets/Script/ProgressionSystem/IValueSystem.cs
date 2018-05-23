using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IValueSystem {

	float Value {	get; }

	void ModifyValue (float modifiedValue);

	void SetValueTo (float valueAmount);

	void ValuePowered (float factor);

    void ValuePowered(float dividedByFactor, float amountToAdd);
}
