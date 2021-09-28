using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public abstract class ChangeableValue
    {
        [SerializeField] protected float _changeAmountFromSub = default;
        [SerializeField] protected float _rawValue = default;
        [SerializeField] protected float _lastValue = default;
        [SerializeField] protected float _value = default;
        [SerializeField] protected int _compressOnCount = default;
        [SerializeField] protected List<NumberChangeCommand> _changeCommands = new List<NumberChangeCommand>();

        public abstract NumberChangeCommandPart Part { get; }
        protected abstract float SubValue { get; }        
        public float ChangeAmountFromSub => _changeAmountFromSub;
        public float LastValue => _lastValue;
        public float RawValue => _rawValue = CalculateRawValue(SubValue, _changeAmountFromSub);
        public float Value => _value = CalculateValue(SubValue, _changeAmountFromSub);
        public int IntValue => Mathf.FloorToInt(Value);
        public long LongValue => (long)Mathf.Floor(Value);
        public float LastChangeAmount => Value - LastValue;
        public OrderedList<Action<NumberChangeCommand>> OnNewChangeCommandActions { get; } = new OrderedList<Action<NumberChangeCommand>>(50);
        public OrderedList<Action<NumberChangeCommand>> OnNewValueActions { get; } = new OrderedList<Action<NumberChangeCommand>>(100);

        public ChangeableValue(int compressOnCount = 100)
        {
            _compressOnCount = compressOnCount;
            OnNewChangeCommandActions.Add(100, cc => _lastValue = Value);
        }

        public void Change<T>(float amount, string gameObjectName,
            T type = default, string description = "",
            [CallerFilePath] string sourceFilePath = "") => Change(amount, gameObjectName, type.ToString(), description, sourceFilePath);

        public void Change(float amount, string gameObjectName,
            string type = "", string description = "",
            [CallerFilePath] string sourceFilePath = "")
        {
            var newChangeCommand = new NumberChangeCommand(Part,
                amount, gameObjectName, type, description, sourceFilePath);
            Change(newChangeCommand);
        }

        private void Change(NumberChangeCommand changeCommand)
        {
            OnNewChangeCommandActions.CallActionsSafely(changeCommand);
            ApplyChange(changeCommand);
            OnNewValueActions.CallActionsSafely(changeCommand);
        }

        //public void RemoveChange(string changeCommandId)
        //{
        //    var changeCommand = _changeCommands.SingleOrDefault(cc => cc.Id == changeCommandId);
        //    if (changeCommand == null)
        //        return;
        //    _changeCommands.Remove(changeCommand);
        //    _changeAmount -= changeCommand.Amount.CurrentValue.Result;
        //    OnNewResultActions.CallActionsSafely(changeCommand, ResultChangeType.REMOVED_CHANGE_COMMAND));
        //}

        protected void ApplyChange(NumberChangeCommand changeCommand)
        {
            _changeCommands.Add(changeCommand);
            _changeAmountFromSub += changeCommand.Amount.Current.Value;
            if (_changeCommands.Count > _compressOnCount)
                Compress();
        }

        protected abstract float CalculateRawValue(float subAmount, float changeAmount);
        protected abstract float CalculateValue(float subAmount, float changeAmount);

        private void Compress()
        {
            var compressCount = 4;
            var compressedAmount = _changeCommands.GetRange(0, compressCount).Sum(cc => cc.Amount.Current.Value);
            var compressedChangeCommand = new NumberChangeCommand(Part, compressedAmount, null, "COMPRESS");
            _changeCommands.RemoveRange(0, compressCount);
            _changeCommands.Insert(0, compressedChangeCommand);
        }
    }
}
