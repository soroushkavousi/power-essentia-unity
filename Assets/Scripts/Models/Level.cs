using System;
using UnityEngine;

[Serializable]
public class Level : Observable<int>
{
    [SerializeField] private int _max;

    public override int Value
    {
        get => _value;
        set
        {
            var newValue = Mathf.Clamp(value, 1, _max);
            if (_value == newValue)
                return;
            _lastValue = _value;
            _value = newValue;
            Notify();
        }
    }
    public int Max => _max;
    public bool IsMax => _value == _max;

    public Level(int value, int max)
    {
        _max = max;
        Value = value;
    }
}
