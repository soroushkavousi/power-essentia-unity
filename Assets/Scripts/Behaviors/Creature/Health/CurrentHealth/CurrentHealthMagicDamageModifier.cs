using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CurrentHealthMagicDamageModifier
{
    private MagicResistance _resistance;

    public CurrentHealthMagicDamageModifier(MagicResistance resistance)
    {
        _resistance = resistance;
    }

    public float Apply(float damage)
    {
        var currentResistance = _resistance.Value;
        var modifiedDamage = damage.MeasureRemainingPercentage(currentResistance);
        return modifiedDamage;
    }
}
