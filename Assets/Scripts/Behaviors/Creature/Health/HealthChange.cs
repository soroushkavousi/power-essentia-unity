public abstract class HealthChange
{
    public float OldValue { get; }
    public float NewValue { get; }
    public float ChangeValue => NewValue - OldValue;

    public HealthChange(float oldValue, float newValue)
    {
        OldValue = oldValue;
        NewValue = newValue;
    }
}

public class HealthDamageChange : HealthChange
{
    public Damage InputDamage { get; }

    public HealthDamageChange(float oldValue, float newValue, Damage inputDamage)
        : base(oldValue, newValue)
    {
        InputDamage = inputDamage;
    }
}

public class HealthHealChange : HealthChange
{
    public float InputHealAmount { get; }

    public HealthHealChange(float oldValue, float newValue, float inputHealAmount)
        : base(oldValue, newValue)
    {
        InputHealAmount = inputHealAmount;
    }
}

public class HealthPreserveRatioChange : HealthChange
{
    public float OldMaxValue { get; }
    public float NewMaxValue { get; }

    public HealthPreserveRatioChange(float oldValue, float newValue,
        float oldMaxValue, float newMaxValue)
        : base(oldValue, newValue)
    {
        OldMaxValue = oldMaxValue;
        NewMaxValue = newMaxValue;
    }
}

public class HealthDeathChange : HealthChange
{
    public HealthDeathChange(float oldValue, float newValue)
        : base(oldValue, newValue)
    {

    }
}
