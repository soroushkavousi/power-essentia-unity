using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class AdvancedBoolean
{
    [SerializeField] private bool _lastValue = default;
    [SerializeField] private bool _value = default;
    [SerializeField] private bool _isFix = default;
    [SerializeField] private bool _fixValue = default;
    [SerializeField] protected int _removeOldCommandOnCount = default;
    [SerializeField] private List<BooleanChangeCommand> _changeCommands = new List<BooleanChangeCommand>();

    public bool LastValue => _lastValue;
    public bool Value
    {
        get
        {
            if (_isFix)
                return _fixValue;
            return _value;
        }
    }

    public OrderedList<Action<BooleanChangeCommand>> OnNewChangeCommandActions { get; } = new OrderedList<Action<BooleanChangeCommand>>();
    public OrderedList<Action<BooleanChangeCommand>> OnNewValueActions { get; } = new OrderedList<Action<BooleanChangeCommand>>();
    public bool IsFix => _isFix;

    public AdvancedBoolean()
    {
        _removeOldCommandOnCount = 10;
        OnNewChangeCommandActions.Add(100, cc =>
        {
            _lastValue = Value;
        });
    }

    public void FeedData(bool startValue)
    {
        _lastValue = _value = startValue;
        _changeCommands.Clear();
        var changeCommand = new BooleanChangeCommand(_value,
                nameof(AdvancedBoolean), "FEED_DATA", "");
        OnNewValueActions.CallActionsSafely(changeCommand);
    }

    public void Change(bool value, string gameObjectName,
        string type = "", string description = "",
        [CallerFilePath] string sourceFilePath = "")
    {
        var newChangeCommand = new BooleanChangeCommand(value,
            gameObjectName, type, description, sourceFilePath);
        OnNewChangeCommandActions.CallActionsSafely(newChangeCommand);
        Change(newChangeCommand);
        OnNewValueActions.CallActionsSafely(newChangeCommand);
    }

    private void Change(BooleanChangeCommand changeCommand)
    {
        _changeCommands.Add(changeCommand);
        _value = changeCommand.Value;
        if (_changeCommands.Count > _removeOldCommandOnCount)
            _changeCommands.RemoveAt(0);
    }

    //public T GetEnumValue<T>() => _value.ToEnum<T>();

    public void Fix<T>(T fixValue) => Fix(fixValue.ToString());

    public void Fix(bool fixValue)
    {
        _isFix = true;
        _fixValue = fixValue;
    }
    public void UnFix() => _isFix = false;
}
