using System;
using UnityEngine;

[Serializable]
public class Health : NumberWithMax, ISubject<Damage>, ISubject<Heal>
{
    [SerializeField] protected bool _isDead = default;
    [SerializeField] protected Number _physicalResistance;
    [SerializeField] protected Number _magicResistance;
    private readonly ObserverCollection<Damage> _damageObservers = new();
    private readonly ObserverCollection<Heal> _healObservers = new();

    public bool IsDead => _isDead;
    public Number PhysicalResistance => _physicalResistance;
    public Number MagicResistance => _magicResistance;

    public Health(float startValue, Observable<int> level, float oneLevelPercentage,
        float startPhysicalResistance, float physicalResistanceOneLevelPercentage,
        float startMagicResistance, float magicResistanceOneLevelPercentage)
        : base(startValue, level, oneLevelPercentage)
    {
        _physicalResistance = new(startPhysicalResistance, level,
            physicalResistanceOneLevelPercentage, max: 90);

        _magicResistance = new(startMagicResistance, level,
            magicResistanceOneLevelPercentage, max: 90);
    }

    protected virtual Damage ModifyDamage(Damage damage)
    {
        var modifiedDamage = 0f;
        switch (damage.Type)
        {
            case DamageType.PHYSICAL:
                modifiedDamage = damage.Value
                    .RemovePercentage(_physicalResistance.Value);
                break;
            case DamageType.MAGIC:
                modifiedDamage = damage.Value
                    .RemovePercentage(_magicResistance.Value);
                break;
            case DamageType.PURE:
                modifiedDamage = damage.Value;
                break;
        }
        return new Damage(damage.Type, modifiedDamage, damage.IsCritical);
    }

    public void Damage(Damage damage)
    {
        var modifiedDamage = ModifyDamage(damage);
        Notify(damage);
        Decrease(modifiedDamage.Value);
    }

    public void Heal(float healAmount)
    {
        var heal = new Heal(healAmount);
        Notify(heal);
        Increase(healAmount);
    }

    public void Attach(IObserver<Damage> observer) => _damageObservers.Add(observer);
    public void Detach(IObserver<Damage> observer) => _damageObservers.Remove(observer);
    public void Notify(Damage damage) => _damageObservers.Notify(this, damage);

    public void Attach(IObserver<Heal> observer) => _healObservers.Add(observer);
    public void Detach(IObserver<Heal> observer) => _healObservers.Remove(observer);
    public void Notify(Heal heal) => _healObservers.Notify(this, heal);
}
