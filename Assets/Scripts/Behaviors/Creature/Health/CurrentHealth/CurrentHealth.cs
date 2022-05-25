using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CurrentHealth : ISubject
{
    [SerializeField] private float _value;
    private float _minusValue;
    private MaxHealth _maxHealth;
    private CurrentHealthPhysicalDamageModifier _physicalDamageModifier;
    private CurrentHealthMagicDamageModifier _magicDamageModifier;
    private readonly List<IObserver> _observers = new();

    public float Value { get => _value; private set { _value = value; Notify(); } }

    public CurrentHealth(MaxHealth maxHealth, CurrentHealthPhysicalDamageModifier physicalDamageModifier,
        CurrentHealthMagicDamageModifier magicDamageModifier)
    {
        _maxHealth = maxHealth;
        _physicalDamageModifier = physicalDamageModifier;
        _magicDamageModifier = magicDamageModifier;
    }

    public void DealDamage(Damage damage)
    {
        var modifiedDamage = CalculateModifiedDamage(damage);
        var maxHealth = _maxHealth.Value;
        _minusValue = Math.Clamp(_minusValue + modifiedDamage, 0f, maxHealth);
        Value = maxHealth - _minusValue;
    }

    public void Heal(float healAmount)
    {
        var maxHealth = _maxHealth.Value;
        _minusValue = Math.Clamp(_minusValue - healAmount, 0f, maxHealth);
        Value = maxHealth - _minusValue;
    }

    private float CalculateModifiedDamage(Damage damage)
    {
        switch (damage.Type)
        {
            case DamageType.PHYSICAL:
                return _physicalDamageModifier.Apply(damage.Value);
            case DamageType.MAGICAL:
                return _magicDamageModifier.Apply(damage.Value);
            case DamageType.PURE:
            default:
                return damage.Value;
        }
    }

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
    public void Notify() => _observers.ForEach(observer => observer.Update(this));
}
