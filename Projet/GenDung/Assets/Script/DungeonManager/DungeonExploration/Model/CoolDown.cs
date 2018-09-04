using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDown
{
    public int AmountOfTurnLeft;
    public int TotalAmountOfTurn;

    public CoolDown(int amountOfTurnLeft, int totalAmountOfTurn)
    {
        AmountOfTurnLeft = amountOfTurnLeft;
        TotalAmountOfTurn = totalAmountOfTurn;
    }
}
