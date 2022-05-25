using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PhysicalResistance : IObserver, ISubject
{
    [SerializeField] private float _value;
    private float _startValue;
    private PhysicalResistanceModifier _firstModifierInChain;
    private PhysicalResistanceModifier _lastModifierInChain;
    private Dictionary<PhysicalResistanceModifierType, PhysicalResistanceModifier> _modifiers;
    private readonly List<IObserver> _observers = new();

    public float Value => _value;

    public PhysicalResistance(float startValue)
    {
        _startValue = startValue;
        _modifiers = new Dictionary<PhysicalResistanceModifierType, PhysicalResistanceModifier>();
    }

    private void CalculateValue()
    {
        _value = _firstModifierInChain.Apply(_startValue);
    }

    public void AddModifier(PhysicalResistanceModifier modifier)
    {
        if (_firstModifierInChain == null)
            _firstModifierInChain = modifier;
        else
            _lastModifierInChain.NextModifier = modifier;
        modifier.Attach(this);
        _lastModifierInChain = modifier;
        _modifiers.Add(modifier.Type, modifier);
    }

    public PhysicalResistanceModifier GetModifier(PhysicalResistanceModifierType type)
        => _modifiers.GetValueOrDefault(type);

    public void Update(ISubject subject)
    {
        if (subject.GetType().IsSubclassOf(typeof(PhysicalResistanceModifier)) == false)
            return;
        CalculateValue();
    }
    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
    public void Notify() => _observers.ForEach(observer => observer.Update(this));
}
