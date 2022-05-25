using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PhysicalResistanceMaxModifier : PhysicalResistanceModifier
{
    private float _maxResistance;

    public override PhysicalResistanceModifierType Type => PhysicalResistanceModifierType.MAX;

    public PhysicalResistanceMaxModifier()
    {
        _maxResistance = 99;
    }

    public override float Apply(float value) => CalculateNewValue(value);

    private float CalculateNewValue(float value)
    {
        if (value > _maxResistance)
            return _maxResistance;
        return value;
    }
}
