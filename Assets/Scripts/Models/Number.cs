using System;
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

    [Space(Constants.SpaceSection)]
    [SerializeField] protected float _fixValue;
    [SerializeField] protected bool _isFixed;

    [Space(Constants.SpaceSection)]
    [SerializeField] protected float _oneLevelPercentage;
    [SerializeField] protected Observable<int> _level;
    private readonly ObserverCollection _observers = new();

    public float Value => _value;
    public float LastValue => _lastValue;
    public bool IsFixed => _isFixed;
    public float NextLevelValue => _nextLevelValue;

    public Number(float startValue, Observable<int> level,
        float oneLevelPercentage, float min = float.MinValue,
        float max = float.MaxValue, float minPercentage = float.MinValue,
        float maxPercentage = float.MaxValue)
    {
        _value = _baseValue = _startValue = startValue;
        _min = min;
        _max = max;
        _minPercentage = minPercentage;
        _maxPercentage = maxPercentage;

        _level = level;
        if(_level != null)
            _level.Attach(this);
        _oneLevelPercentage = oneLevelPercentage;

        CalculateValue();
    }

    public Number(float startValue, float min = float.MinValue,
        float max = float.MaxValue, float minPercentage = float.MinValue,
        float maxPercentage = float.MaxValue) 
        : this(startValue, null, 0f, min, max, minPercentage, maxPercentage)
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
        _value = Mathf.Clamp(newValue, _min, _max);

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
        _levelPercentage = (_level.Value - 1) * _oneLevelPercentage;
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