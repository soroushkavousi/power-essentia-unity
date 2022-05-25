using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MagicResistance : IObserver, ISubject
{
    [SerializeField] private float _value;
    private float _startValue;
    private MagicResistanceModifier _firstModifierInChain;
    private MagicResistanceModifier _lastModifierInChain;
    private Dictionary<MagicResistanceModifierType, MagicResistanceModifier> _modifiers;
    private readonly List<IObserver> _observers = new();

    public float Value => _value;

    public MagicResistance(float startValue)
    {
        _startValue = startValue;
        _modifiers = new Dictionary<MagicResistanceModifierType, MagicResistanceModifier>();
    }

    private void CalculateValue()
    {
        _value = _firstModifierInChain.Apply(_startValue);
    }

    public void AddModifier(MagicResistanceModifier modifier)
    {
        if (_firstModifierInChain == null)
            _firstModifierInChain = modifier;
        else
            _lastModifierInChain.NextModifier = modifier;
        modifier.Attach(this);
        _lastModifierInChain = modifier;
        _modifiers.Add(modifier.Type, modifier);
    }

    public MagicResistanceModifier GetModifier(MagicResistanceModifierType type)
        => _modifiers.GetValueOrDefault(type);

    public void Update(ISubject subject)
    {
        if (subject.GetType().IsSubclassOf(typeof(MagicResistanceModifier)) == false)
            return;
        CalculateValue();
    }
    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
    public void Notify() => _observers.ForEach(observer => observer.Update(this));
}
