using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class AdvancedString
{
    [SerializeField] private string _lastValue = default;
    [SerializeField] private Enum _lastEnumValue = default;
    [SerializeField] private string _value = default;
    [SerializeField] private Enum _enumValue = default;
    [SerializeField] private Type _enumType = default;
    [SerializeField] private bool _isFix = default;
    [SerializeField] private string _fixValue = default;
    [SerializeField] private Enum _fixEnumValue = default;
    [SerializeField] protected int _removeOldCommandOnCount = default;
    [SerializeField] private List<StringChangeCommand> _changeCommands = new List<StringChangeCommand>();

    public string LastValue => _lastValue;
    public Enum LastEnumValue => _lastEnumValue;
    public string Value
    {
        get
        {
            if (_isFix)
                return _fixValue;
            return _value;
        }
    }
    public Enum EnumValue
    {
        get
        {
            if (_isFix)
                return _fixEnumValue;
            return _enumValue;
        }
    }

    public OrderedList<Action<StringChangeCommand>> OnNewChangeCommandActions { get; } = new OrderedList<Action<StringChangeCommand>>();
    public OrderedList<Action<StringChangeCommand>> OnNewValueActions { get; } = new OrderedList<Action<StringChangeCommand>>();
    public bool IsFix => _isFix;

    public AdvancedString()
    {
        _removeOldCommandOnCount = 10;
        OnNewChangeCommandActions.Add(100, cc =>
        {
            _lastValue = Value;
            if(_enumType != default)
                _lastEnumValue = _lastValue.ToEnum(_enumType);
        });
    }

    public void FeedData<T>(T startValue)
    {
        _enumType = typeof(T);
        FeedData(startValue.ToString());
        _lastEnumValue = _enumValue = _value.ToEnum(_enumType);
    }

    public void FeedData(string startValue)
    {
        _lastValue = _value = startValue;
        _changeCommands.Clear();
        var changeCommand = new StringChangeCommand(_value,
                nameof(AdvancedString), "FEED_DATA", "");
        OnNewValueActions.CallActionsSafely(changeCommand);
    }

    public void Change<T>(T value, string gameObjectName,
        string type = "", string description = "",
        [CallerFilePath] string sourceFilePath = "") => Change(value.ToString(), 
            gameObjectName, type, description, sourceFilePath);

    public void Change(string value, string gameObjectName,
        string type = "", string description = "",
        [CallerFilePath] string sourceFilePath = "")
    {
        var newChangeCommand = new StringChangeCommand(value,
            gameObjectName, type, description, sourceFilePath);
        OnNewChangeCommandActions.CallActionsSafely(newChangeCommand);
        Change(newChangeCommand);
        OnNewValueActions.CallActionsSafely(newChangeCommand);
    }

    private void Change(StringChangeCommand changeCommand)
    {
        _changeCommands.Add(changeCommand);
        _value = changeCommand.Value;
        if (_enumType != default)
            _enumValue = _value.ToEnum(_enumType);
        if (_changeCommands.Count > _removeOldCommandOnCount)
            _changeCommands.RemoveAt(0);
    }

    //public T GetEnumValue<T>() => _value.ToEnum<T>();

    public void Fix<T>(T fixValue) => Fix(fixValue.ToString());

    public void Fix(string fixValue)
    {
        _isFix = true;
        _fixValue = fixValue;
        if (_enumType != default)
            _fixEnumValue = _fixValue.ToEnum(_enumType);
    }
    public void UnFix() => _isFix = false;
}
