﻿using System;
using UnityEngine;

[Serializable]
public class Number : ISubject, IObserver
{
    [SerializeField] protected float _value;
    [SerializeField] protected float _limitedBasePercentage;
    [SerializeField] protected float _basePercentage;
    [SerializeField] protected float _baseValue;
    [SerializeField] protected float _levelPercentage;
    [SerializeField] protected float _startValue;

    [Space(Constants.SpaceSection)]
    [SerializeField] protected float _lastValue;
    [SerializeField] protected float _nextLevelValue;

    [Space(Constants.SpaceSection)]
    [SerializeField] protected float _min;
    [SerializeField] protected float _max;
    [SerializeField] protected float _minPercentage;
    [SerializeField] protected float _maxPercentage;
    [SerializeField] protected float _randomnessPercentage;

    [Space(Constants.SpaceSection)]
    [SerializeField] protected float _fixValue;
    [SerializeField] protected bool _isFixed;

    [Space(Constants.SpaceSection)]
    [SerializeField] protected float _oneLevelPercentage;
    [SerializeField] protected Level _level;
    private readonly ObserverCollection _observers = new();

    public float Value => _value;
    public float LastValue => _lastValue;
    public bool IsFixed => _isFixed;
    public float NextLevelValue => _nextLevelValue;

    public Number(Level level, NumberLevelInfo levelInfo, float min = float.MinValue,
        float max = float.MaxValue, float minPercentage = float.MinValue,
        float maxPercentage = float.MaxValue, float randomnessPercentage = 0f)
    {
        _value = _baseValue = _startValue = levelInfo.StartValue;
        _min = min;
        _max = max;
        _minPercentage = minPercentage;
        _maxPercentage = maxPercentage;
        _randomnessPercentage = randomnessPercentage;

        _level = level;
        if (_level != null)
            _level.Attach(this);
        _oneLevelPercentage = levelInfo.OneLevelPercentage;

        CalculateValue();
    }

    public Number(float startValue, float min = float.MinValue,
        float max = float.MaxValue, float minPercentage = float.MinValue,
        float maxPercentage = float.MaxValue)
        : this(null, new(startValue, 0f), min, max, minPercentage, maxPercentage)
    {

    }

    protected void CalculateValue()
    {
        if (_isFixed)
        {
            _value = _fixValue;
            return;
        }

        CalculateBaseValue();
        var lastValue = _value;
        _limitedBasePercentage = Mathf.Clamp(_basePercentage, _minPercentage, _maxPercentage);
        var newValue = _baseValue.AddPercentage(_limitedBasePercentage);
        newValue = Mathf.Clamp(newValue, _min, _max);
        _value = newValue.Randomize(_randomnessPercentage);

        CalculateNextLevelValue();

        if (lastValue != _value)
        {
            _lastValue = lastValue;
            Notify();
        }
    }

    protected void CalculateBaseValue()
    {
        if (_level == null)
            return;

        var levelFactor = _level.Value - 1;
        if (_level.IsMax)
        {
            var levelValueUnits = _level.Value % 10;
            if (levelValueUnits == 0 || levelValueUnits == 5)
                levelFactor += 1;
        }
        _levelPercentage = levelFactor * _oneLevelPercentage;
        _baseValue = _startValue.AddPercentage(_levelPercentage);
    }

    protected void CalculateNextLevelValue()
    {
        if (_level == null)
            return;
        var nextLevelBaseValue = _startValue.AddPercentage(_levelPercentage + _oneLevelPercentage);
        var newNextLevelValue = nextLevelBaseValue.AddPercentage(_limitedBasePercentage);
        _nextLevelValue = Mathf.Clamp(newNextLevelValue, _min, _max);
    }

    public void Increase(float percentage)
    {
        _basePercentage += percentage;
        CalculateValue();
    }
    public void Decrease(float percentage)
        => Increase(-percentage);

    public void Fix(float fixValue)
    {
        _fixValue = fixValue;
        _isFixed = true;
        CalculateValue();
    }

    public void Unfix()
    {
        _isFixed = false;
        CalculateValue();
    }

    public void OnNotify(ISubject subject)
    {
        CalculateValue();
    }

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
    public void Notify() => _observers.Notify(this);
}