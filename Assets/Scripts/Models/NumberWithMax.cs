using UnityEngine;

public class NumberWithMax : ISubject, IObserver
{
    [SerializeField] protected float _value;
    [SerializeField] protected float _maxMinus;
    [SerializeField] protected Number _max;
    [SerializeField] protected float _nextLevelValue;
    private readonly ObserverCollection _observers = new();

    public float Value => _value;
    public float NextLevelValue => _nextLevelValue;
    public Number Max => _max;

    public NumberWithMax(float startValue, Observable<int> level,
        float oneLevelPercentage)
    {
        _max = new Number(startValue, level, oneLevelPercentage);
        _max.Attach(this);
        CalculateValue();
    }

    protected void CalculateValue()
    {
        var lastValue = _value;
        _value = _max.Value - _maxMinus;
        _nextLevelValue = _max.NextLevelValue - _maxMinus;
        if (lastValue != _value)
            Notify();
    }

    protected void KeepRatio()
    {
        _maxMinus = _maxMinus / _max.LastValue * _max.Value;
        CalculateValue();
    }

    protected void Decrease(float amount)
    {
        _maxMinus = Mathf.Clamp(_maxMinus + amount, 0f, _max.Value);
        CalculateValue();
    }
    protected void Increase(float amount)
        => Decrease(-amount);

    public void OnNotify(ISubject subject)
    {
        if (subject == _max)
        {
            KeepRatio();
        }
    }

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
    public void Notify() => _observers.Notify(this);
}