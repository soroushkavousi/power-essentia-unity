using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MagicResistanceLevelModifier : MagicResistanceModifier
{
    private int _level;

    private readonly float _resistancePerLevel;

    public int Level { get => _level; set { _level = value; Notify(); } }

    public override MagicResistanceModifierType Type => MagicResistanceModifierType.LEVEL;

    public MagicResistanceLevelModifier(int level, float healthPerLevel)
    {
        Level = level;
        _resistancePerLevel = healthPerLevel;
    }

    public override float Apply(float value)
    {
        var newValue = CalculateNewValue(value);
        if(NextModifier != null)
            return NextModifier.Apply(newValue);
        return newValue;
    }

    protected virtual float CalculateNewValue(float value)
        => value + Level * _resistancePerLevel;
}
