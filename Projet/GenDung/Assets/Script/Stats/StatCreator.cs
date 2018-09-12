using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatCreator : MonoBehaviour
{
    public Stat Strength;
    [SerializeField] private StatPreset StrengthPreset;
    public Stat Knowledge;
    [SerializeField] private StatPreset KnowledgePreset;
    public Stat Dexterity;
    [SerializeField] private StatPreset DexterityPreset;
    public Stat Wisdom;
    [SerializeField] private StatPreset WisdomPreset;
    public Stat Survivability;
    [SerializeField] private StatPreset SurvivabilityPreset;

    private void Start()
    {
        Strength = new Stat(StrengthPreset);
        Knowledge = new Stat(KnowledgePreset);
        Dexterity = new Stat(DexterityPreset);
        Wisdom = new Stat(WisdomPreset);
        Survivability = new Stat(SurvivabilityPreset);
    }
}
