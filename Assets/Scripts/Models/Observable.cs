using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Observable<T> : ISubject
{
    [SerializeField] protected T _value;
    [SerializeField] protected T _lastValue;
    [SerializeField] protected bool _ignoreRepetition;
    [SerializeField] protected int _observersCount;
    private readonly ObserverCollection _observers = new();

    public virtual T Value
    {
        get => _value;
        set
        {
            if (_ignoreRepetition
                && EqualityComparer<T>.Default.Equals(_value, value))
                return;
            _lastValue = _value;
            _value = value;
            Notify();
        }
    }
    public T LastValue => _lastValue;

    public Observable(T value = default, bool ignoreRepetition = true)
    {
        _value = value;
        _ignoreRepetition = ignoreRepetition;
    }

    public void Attach(IObserver observer) { _observers.Add(observer); _observersCount = _observers.Count; }
    public void Detach(IObserver observer) { _observers.Remove(observer); _observersCount = _observers.Count; }
    public void Notify() => _observers.Notify(this);
}
