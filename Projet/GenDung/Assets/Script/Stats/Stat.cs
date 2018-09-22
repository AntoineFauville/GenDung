public class Stat {
    
    //health
    private int _healthModificator;
    //Damage
    private int _physicalDamageModificator;
    private int _bloodDamageModificator;
    private int _natureDamageModificator;
    private int _magicDamageModificator;
    //resistance
    private int _physicalResistance;
    private int _bloodResistance;
    private int _natureResistance;
    private int _magicResistance;
    //Chances
    private int _dodgingChance;
    private int _criticalChance;

    public int HealthModificator
    {
        get
        {
            return _healthModificator;
        }
        set
        {
            _healthModificator = value;
        }
    }

    public int BloodDamageModificator
    {
        get
        {
            return _bloodDamageModificator;
        }
        set
        {
            _bloodDamageModificator = value;
        }
    }

    public int PhysicalDamageModificator
    {
        get
        {
            return _physicalDamageModificator;
        }
        set
        {
            _physicalDamageModificator = value;
        }
    }

    public int NatureDamageModificator
    {
        get
        {
            return _natureDamageModificator;
        }
        set
        {
            _natureDamageModificator = value;
        }
    }

    public int MagicDamageModificator
    {
        get
        {
            return _magicDamageModificator;
        }
        set
        {
            _magicDamageModificator = value;
        }
    }

    public int PhysicalResistance
    {
        get
        {
            return _physicalResistance;
        }
        set
        {
            _physicalResistance = value;
        }
    }

    public int BloodResistance
    {
        get
        {
            return _bloodResistance;
        }
        set
        {
            _bloodResistance = value;
        }
    }

    public int NatureResistance
    {
        get
        {
            return _natureResistance;
        }
        set
        {
            _natureResistance = value;
        }
    }

    public int MagicResistance
    {
        get
        {
            return _magicResistance;
        }
        set
        {
            _magicResistance = value;
        }
    }

    public int DodgingChance
    {
        get
        {
            return _dodgingChance;
        }
        set
        {
            _dodgingChance = value;
        }
    }

    public int CriticalChance
    {
        get
        {
            return _criticalChance;
        }
        set
        {
            _criticalChance = value;
        }
    }

    public Stat(StatPreset statPreset)
    {
        HealthModificator = statPreset.HealthModificator;

        PhysicalDamageModificator = statPreset.PhysicDamageModificator;
        BloodDamageModificator = statPreset.BloodDamageModificator;
        NatureDamageModificator = statPreset.NatureDamageModificator;
        MagicDamageModificator = statPreset.MagicDamageModificator;

        PhysicalResistance = statPreset.PhysicalResistance;
        BloodResistance = statPreset.BloodResistance;
        NatureResistance = statPreset.NatureResistance;
        MagicResistance = statPreset.MagicResistance;

        DodgingChance = statPreset.DodgingChance;
        CriticalChance = statPreset.CriticalChance;
    }
}
