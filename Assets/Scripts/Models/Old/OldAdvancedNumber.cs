//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.IO;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace Assets.Scripts.Models
//{
//    [Serializable]
//    public class OldAdvancedNumber
//    {
//        [SerializeField] private float _startValue = default;
//        [SerializeField] private float _baseValue = default;
//        [SerializeField] private float _value = default;
//        [SerializeField] private float _currentValue = default;
//        [SerializeField] private float? _currentValueMin = default;
//        [SerializeField] private float? _currentValueMax = default;
//        [SerializeField] private bool _valueIsMax = default;

//        [SerializeField] private bool _keepRatio = default;
//        [SerializeField] private bool _isFix = default;
//        [SerializeField] private float _fixCurrentValue = default;
//        [SerializeField] private float _previousValue = default;
//        [SerializeField] private float _previousCurrentValue = default;

//        [SerializeField] private ChangeCommandsProcessor _baseValueChangeCommandsProcessor = new ChangeCommandsProcessor();
//        [SerializeField] private ChangeCommandsProcessor _valueChangeCommandsProcessor = new ChangeCommandsProcessor();
//        [SerializeField] private ChangeCommandsProcessor _currentValueChangeCommandsProcessor = new ChangeCommandsProcessor();

//        public float StartValue
//        {
//            get => _startValue;
//            set
//            {
//                _startValue = value;
//                _currentValue = CurrentValue;
//                OnPostChangeActions.CallActionsSafely());
//            }
//        }
//        public float BaseValue
//        {
//            get
//            {
//                var changeAmount = _baseValueChangeCommandsProcessor.ResultAmount;
//                _baseValue = StartValue + changeAmount;
//                return _baseValue;
//            }
//        }
//        public float Value
//        {
//            get
//            {
//                var changeAmount = _valueChangeCommandsProcessor.ResultAmount;
//                _previousValue = _value;
//                // Todo Negative base value?
//                _value = BaseValue * (100 + changeAmount) / 100;
//                _value = _value.Round();
//                return _value;
//            }
//        }
//        public float CurrentValue
//        {
//            get
//            {
//                if (_isFix)
//                    return _fixCurrentValue;
//                _previousCurrentValue = _currentValue;
//                var changeAmount = _currentValueChangeCommandsProcessor.ResultAmount;
//                _currentValue = Value + changeAmount;
//                KeepRatio();
//                ApplyCurrentValueLimitations();
//                return _currentValue;
//            }
//        }

//        public bool IsFix => _isFix;
//        public float FixCurrentValue => _fixCurrentValue;
//        public int CurrentIntValue => Mathf.FloorToInt(CurrentValue);
//        public long CurrentLongValue => (long)Mathf.Floor(CurrentValue);

//        public ObservableCollection<NumberChangeCommand> BaseValueChangeCommands => _baseValueChangeCommandsProcessor.ChangeCommands;
//        public ObservableCollection<NumberChangeCommand> ValueChangeCommands => _valueChangeCommandsProcessor.ChangeCommands;
//        public ObservableCollection<NumberChangeCommand> CurrentValueChangeCommands => _currentValueChangeCommandsProcessor.ChangeCommands;

//        public OrderedList<Action<NumberChangeCommand>> OnBaseValuePreChangeActions { get; } = new OrderedList<Action<NumberChangeCommand>>();
//        public OrderedList<Action<NumberChangeCommand>> OnValuePreChangeActions { get; } = new OrderedList<Action<NumberChangeCommand>>();
//        public OrderedList<Action<NumberChangeCommand>> OnCurrentValuePreChangeActions { get; } = new OrderedList<Action<NumberChangeCommand>>();
//        public OrderedList<Action<NumberChangeCommand>> OnBaseValuePostChangeActions { get; } = new OrderedList<Action<NumberChangeCommand>>();
//        public OrderedList<Action<NumberChangeCommand>> OnValuePostChangeActions { get; } = new OrderedList<Action<NumberChangeCommand>>();
//        public OrderedList<Action<NumberChangeCommand>> OnCurrentValuePostChangeActions { get; } = new OrderedList<Action<NumberChangeCommand>>();
//        public OrderedList<Action> OnPostChangeActions { get; } = new OrderedList<Action>();

//        public OldAdvancedNumber(float? currentValueMin = null, float? currentValueMax = null,
//            bool valueIsMax = false, bool keepRatio = false)
//        {
//            _currentValueMin = currentValueMin;
//            _currentValueMax = currentValueMax;
//            _valueIsMax = valueIsMax;
//            _keepRatio = keepRatio;

//            _baseValueChangeCommandsProcessor.OnHandleAddedChangeCommandBeforeApplyActions
//                .Add(1, HandleAddedChangeCommandToBaseValueBeforeApply);
//            _baseValueChangeCommandsProcessor.OnHandleAddedChangeCommandAfterApplyActions
//                .Add(1, HandleAddedChangeCommandToBaseValueAfterApply);

//            _valueChangeCommandsProcessor.OnHandleAddedChangeCommandBeforeApplyActions
//                .Add(1, HandleAddedChangeCommandToValueBeforeApply);
//            _valueChangeCommandsProcessor.OnHandleAddedChangeCommandAfterApplyActions
//                .Add(1, HandleAddedChangeCommandToValueAfterApply);

//            _currentValueChangeCommandsProcessor.OnHandleAddedChangeCommandBeforeApplyActions
//                .Add(1, HandleAddedChangeCommandToCurrentValueBeforeApply);
//            _currentValueChangeCommandsProcessor.OnHandleAddedChangeCommandAfterApplyActions
//                .Add(1, HandleAddedChangeCommandToCurrentValueAfterApply);
//        }

//        private void HandleAddedChangeCommandToBaseValueBeforeApply(NumberChangeCommand changeCommand)
//        {
//            OnBaseValuePreChangeActions.CallActionsSafely(changeCommand));
//        }
//        private void HandleAddedChangeCommandToBaseValueAfterApply(NumberChangeCommand changeCommand)
//        {
//            _currentValue = CurrentValue;
//            OnBaseValuePostChangeActions.CallActionsSafely(changeCommand));
//            OnPostChangeActions.CallActionsSafely());
//        }

//        private void HandleAddedChangeCommandToValueBeforeApply(NumberChangeCommand changeCommand)
//        {
//            OnValuePreChangeActions.CallActionsSafely(changeCommand));
//        }
//        private void HandleAddedChangeCommandToValueAfterApply(NumberChangeCommand changeCommand)
//        {
//            _currentValue = CurrentValue;
//            OnValuePostChangeActions.CallActionsSafely(changeCommand));
//            OnPostChangeActions.CallActionsSafely());
//        }

//        private void HandleAddedChangeCommandToCurrentValueBeforeApply(NumberChangeCommand changeCommand)
//        {
//            OnCurrentValuePreChangeActions.CallActionsSafely(changeCommand));
//            RemoveCurrentValueExceedAmountIfValueIsMax(changeCommand);
//        }
//        private void HandleAddedChangeCommandToCurrentValueAfterApply(NumberChangeCommand changeCommand)
//        {
//            _currentValue = CurrentValue;
//            OnCurrentValuePostChangeActions.CallActionsSafely(changeCommand));
//            OnPostChangeActions.CallActionsSafely());
//        }

//        public void Fix(float fixCurrentValue)
//        {
//            _isFix = true;
//            _fixCurrentValue = fixCurrentValue;
//        }
//        public void UnFix() => _isFix = false;

//        //private NumberChangeCommand ApplyBlockOrAmpChangeCommands(NumberChangeCommand changeCommand)
//        //{
//        //    var modifiedChangeAmount = changeCommand.Value;
//        //    if (modifiedChangeAmount == 0)
//        //    {
//        //        return changeCommand;
//        //    }
//        //    else if (modifiedChangeAmount < 0)
//        //    {
//        //        var changeAmount = _blockChangeCommandsProcessor.ResultAmount;
//        //        var percentageChangeAmount = _blockPercentageChangeCommandsProcessor.ResultAmount;
//        //        if (changeAmount == 0 && percentageChangeAmount == 0)
//        //            return changeCommand;
//        //        modifiedChangeAmount = (modifiedChangeAmount + changeAmount) * (100 - percentageChangeAmount) / 100;

//        //        var modifiedCommand = new NumberChangeCommand(modifiedChangeAmount,
//        //            $"Block {percentageChangeAmount}% {changeAmount} buff", null, changeCommand);
//        //        return modifiedCommand;
//        //    }
//        //    else
//        //    {
//        //        var changeAmount = _ampChangeCommandsProcessor.ResultAmount;
//        //        var percentageChangeAmount = _ampPercentageChangeCommandsProcessor.ResultAmount;
//        //        if (changeAmount == 0 && percentageChangeAmount == 0)
//        //            return changeCommand;
//        //        modifiedChangeAmount = (modifiedChangeAmount + changeAmount) * (100 + percentageChangeAmount) / 100;

//        //        var modifiedCommand = new NumberChangeCommand(modifiedChangeAmount,
//        //            $"Amp {percentageChangeAmount}% {changeAmount} buff", null, changeCommand);
//        //        return modifiedCommand;
//        //    }
//        //}

//        private void KeepRatio()
//        {
//            if (_keepRatio == false || _previousValue == _value || _previousValue == 0f ||
//                _previousCurrentValue == 0f || _previousCurrentValue == _previousValue)
//                return;

//            var valueChangeRatio = _value / _previousValue;
//            var correctCurrentValue = valueChangeRatio * _previousCurrentValue;
//            var correctCurrentValueChangeAmount = correctCurrentValue - _currentValue;
//            var changeCommand = new NumberChangeCommand(correctCurrentValueChangeAmount,
//                null, "RATIO", $"KeepingRatio {valueChangeRatio} {correctCurrentValue} {correctCurrentValueChangeAmount}");
//            _currentValue = correctCurrentValue;
//        }

//        private void RemoveCurrentValueExceedAmountIfValueIsMax(NumberChangeCommand changeCommand)
//        {
//            var currentValue = CurrentValue;
//            var value = Value;
//            if (_valueIsMax && currentValue + changeCommand.Amount.CurrentValue.Result > value)
//            {
//                var modifiedAmount = value - (currentValue + changeCommand.Amount.CurrentValue.Result);
//                var modifiedCommand = new NumberChangeCommand(modifiedAmount, null, "EXCEED",
//                    description: "Current value should not exceed from value.");
//                changeCommand.Amount.CurrentValueChangeCommands.Add(modifiedCommand);
//            }
//            //Todo
//            //if(_negativeCurrentValue && currentValue + changeCommand.Amount < 0)
//            //{
//            //    var modifiedAmount = currentValue;
//            //    var modifiedCommand = new ChangeCommand(modifiedAmount, $"ZeroCondition",
//            //        "self", null, changeCommand);
//            //    return modifiedCommand;
//            //}
//        }

//        private void ApplyCurrentValueLimitations()
//        {
//            if (_currentValueMin.HasValue && _currentValue < _currentValueMin.Value)
//                _currentValue = _currentValueMin.Value;

//            if (_currentValueMax.HasValue && _currentValue > _currentValueMax.Value)
//                _currentValue = _currentValueMax.Value;
//        }
//    }
//}