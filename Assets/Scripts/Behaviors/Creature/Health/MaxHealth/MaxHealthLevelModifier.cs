using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MaxHealthLevelModifier : MaxHealthModifier
{
    private int _level;

    private readonly float _healthPerLevel;

    public int Level { get => _level; set { _level = value; Notify(); } }

    public override MaxHealthModifierType Type => MaxHealthModifierType.LEVEL;

    public MaxHealthLevelModifier(int level, float healthPerLevel)
    {
        Level = level;
        _healthPerLevel = healthPerLevel;
    }

    public override float Apply(float value)
    {
        var newValue = CalculateNewValue(value);
        if(NextModifier != null)
            return NextModifier.Apply(newValue);
        return newValue;
    }

    protected virtual float CalculateNewValue(float value)
        => value + Level * _healthPerLevel;
}
