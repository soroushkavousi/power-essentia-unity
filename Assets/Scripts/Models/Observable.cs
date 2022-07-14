using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Observable<T> : ISubject
{
    [SerializeField] protected T _value;
    [SerializeField] protected T _lastValue;
    private readonly ObserverCollection _observers = new();

    public virtual T Value
    {
        get => _value;
        set
        {
            if (EqualityComparer<T>.Default.Equals(_value, value))
                return;
            _lastValue = _value;
            _value = value;
            Notify();
        }
    }
    public T LastValue => _lastValue;

    public Observable(T value = default)
    {
        _value = value;
    }

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
    public void Notify() => _observers.Notify(this);
}
