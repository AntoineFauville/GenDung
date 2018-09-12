using UnityEngine;

[CreateAssetMenu(fileName = "Stat", menuName = "Characters/Stat", order = 1)]
public class StatPreset : ScriptableObject
{
    [Space(10)]
    //health
    public int HealthModificator;
    [Space(10)]
    //Damage
    public int PhysicDamageModificator;
    public int BloodDamageModificator;
    public int NatureDamageModificator;
    public int MagicDamageModificator;
    [Space(10)]
    //resistance
    public int PhysicalResistance;
    public int BloodResistance;
    public int NatureResistance;
    public int MagicResistance;
    [Space(10)]
    //Chances
    public int DodgingChance;
    public int CriticalChance;
    
}
