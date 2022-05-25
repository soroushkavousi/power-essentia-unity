using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Damage
{
    private DamageType _type;
    private float _value;

    public DamageType Type => _type;
    public float Value => _value;

    public Damage(DamageType type, float value)
    {
        _type = type;
        _value = value;
    }
}
