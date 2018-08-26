using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatModel", menuName = "Stats/StatModel", order = 1)]
public class Statistique : ScriptableObject
{
    public int HealthModificator;
    public int PhysicDamageModificator;
    public int BloodDamageModificator;
    public int NatureDamageModificator;
    public int MagicDamageModificator;
    [Space(10)]
    public int PhysicResistanceModificator;
    public int BloodResistanceModificator;
    public int NatureResistanceModificator;
    public int MagicResistanceModificator;
    [Space(10)]
    public int DodgingChances;
    public int CriticalChances;
}
