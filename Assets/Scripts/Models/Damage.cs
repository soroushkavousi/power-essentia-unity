﻿public class Damage
{
    private readonly DamageType _type;
    private float _value;
    private readonly bool _isCritical;

    public DamageType Type => _type;
    public float Value { get => _value; set => _value = value; }
    public bool IsCritical => _isCritical;

    public Damage(DamageType type, float value, bool isCritical = false)
    {
        _type = type;
        _value = value;
        _isCritical = isCritical;
    }
}
