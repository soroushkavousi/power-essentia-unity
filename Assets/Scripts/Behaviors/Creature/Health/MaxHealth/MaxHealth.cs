using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MaxHealth : IObserver, ISubject
{
    private float _startValue;
    private MaxHealthModifier _firstModifierInChain;
    private MaxHealthModifier _lastModifierInChain;
    private Dictionary<MaxHealthModifierType, MaxHealthModifier> _modifiers;
    private readonly List<IObserver> _observers = new();
    [SerializeField] private float _value;

    public float Value { get => _value; private set { _value = value; Notify(); } }

    public MaxHealth(float startValue)
    {
        _startValue = startValue;
        _modifiers = new Dictionary<MaxHealthModifierType, MaxHealthModifier>();
    }

    private void CalculateValue()
    {
        Value = _firstModifierInChain.Apply(_startValue);
    }

    public void AddModifier(MaxHealthModifier modifier)
    {
        if (_firstModifierInChain == null)
            _firstModifierInChain = modifier;
        else
            _lastModifierInChain.NextModifier = modifier;
        modifier.Attach(this);
        _lastModifierInChain = modifier;
        _modifiers.Add(modifier.Type, modifier);
    }

    public MaxHealthModifier GetModifier(MaxHealthModifierType type)
        => _modifiers.GetValueOrDefault(type);

    public void Update(ISubject subject)
    {
        if (subject.GetType().IsSubclassOf(typeof(MaxHealthModifier)) == false)
            return;
        CalculateValue();
    }

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
    public void Notify() => _observers.ForEach(observer => observer.Update(this));
}
