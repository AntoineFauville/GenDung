using UnityEngine;

[CreateAssetMenu(fileName = "Stat", menuName = "Characters/Stat", order = 1)]
public class Stat : ScriptableObject
{
    [Space(10)]
    //health
    public int HealthModificator;
    [Space(10)]
    //Damage
    public int PhysicModificator;
    public int BloodModificator;
    public int NatureModificator;
    public int ArcaneModificator;
    [Space(10)]
    //resistance
    public int PhysicalResistance;
    public int BloodResistance;
    public int NatureResistance;
    public int ArcaneResistance;
    [Space(10)]
    //Chances
    public int DodgingChance;
    public int CriticalChance;
    
}
