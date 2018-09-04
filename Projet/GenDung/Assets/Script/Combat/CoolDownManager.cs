using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownManager : MonoBehaviour
{
    [SerializeField] public CoolDownView[] CoolDownManagers;

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            ResetCoolDownViews(i);
        }
    }

    public void UpdateCoolDownViews(int coolDownViewToUpdate, int amountOfTotalTurn, int amountOfturnLeft)
    {
        CoolDownManagers[coolDownViewToUpdate].UpdateCoolDownView(true, amountOfTotalTurn, amountOfturnLeft);
    }

    public void ResetCoolDownViews(int coolDownViewToUpdate)
    {
        CoolDownManagers[coolDownViewToUpdate].ResetCoolDownView(false);
    }
}
