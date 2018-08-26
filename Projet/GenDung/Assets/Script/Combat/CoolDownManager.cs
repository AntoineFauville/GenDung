using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownManager : MonoBehaviour
{
    [SerializeField] public CoolDownView[] CoolDownManagers;

    public void UpdateCoolDownViews(int coolDownViewToUpdate, int amountOfTotalTurn, int amountOfturnLeft)
    {
        CoolDownManagers[coolDownViewToUpdate].UpdateCoolDownView(true, amountOfTotalTurn, amountOfturnLeft);
    }

    public void ResetCoolDownViews(int coolDownViewToUpdate, int turnAmount)
    {
        CoolDownManagers[coolDownViewToUpdate].ResetCoolDownView(false);
    }
}
