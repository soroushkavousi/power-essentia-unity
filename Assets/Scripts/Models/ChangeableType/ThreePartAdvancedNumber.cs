using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class ThreePartAdvancedNumber
    {
        [SerializeField] private ThreePartAdvancedNumberBase _base = default;
        [SerializeField] private ThreePartAdvancedNumberPeak _peak = default;
        [SerializeField] private ThreePartAdvancedNumberCurrent _current = default;

        public ThreePartAdvancedNumberBase Base => _base;
        public ThreePartAdvancedNumberPeak Peak => _peak;
        public ThreePartAdvancedNumberCurrent Current => _current;
        public float Value => Current.Value;
        public int IntValue => Current.IntValue;
        public long LongValue => Current.LongValue;
        public float LastValue => Current.LastValue;
        public float LastChangeAmount => Current.LastChangeAmount;
        public OrderedList<Action<NumberChangeCommand>> OnNewChangeCommandActions => Current.OnNewChangeCommandActions;
        public OrderedList<Action<NumberChangeCommand>> OnNewValueActions => Current.OnNewValueActions;

        public ThreePartAdvancedNumber(float? currentDummyMin = null, float? currentDummyMax = null,
            bool currentDummyMaxBaseOnPeak = false, bool removeCurrentMinExceed = false, 
            bool removeCurrentMaxExceed = false, bool keepRatio = false)
        {
            _base = new ThreePartAdvancedNumberBase();
            _peak = new ThreePartAdvancedNumberPeak(_base);
            _current = new ThreePartAdvancedNumberCurrent(_peak, currentDummyMin,
                currentDummyMax, currentDummyMaxBaseOnPeak, removeCurrentMinExceed, 
                removeCurrentMaxExceed, keepRatio);
        }

        private ThreePartAdvancedNumber(ThreePartAdvancedNumberBase @base, ThreePartAdvancedNumberPeak peak, 
            ThreePartAdvancedNumberCurrent current)
        {
            _base = @base;
            _peak = peak;
            _current = current;
        }

        public void FeedData(float startValue)
        {
            _base.FeedData(startValue);
            _peak.FeedData();
            _current.FeedData();
            var changeCommand = new NumberChangeCommand(NumberChangeCommandPart.FEED_DATA,
                0, nameof(OnePartAdvancedNumber), "FEED_DATA", "");
            _base.OnNewValueActions.CallActionsSafely(changeCommand);
        }

        public void Change<T>(float amount, string gameObjectName,
            T type = default, string description = "",
            [CallerFilePath] string sourceFilePath = "") 
            => _current.Change(amount, gameObjectName, type, description, sourceFilePath);

        public void Change(float amount, string gameObjectName,
            string type = "", string description = "",
            [CallerFilePath] string sourceFilePath = "")
            => _current.Change(amount, gameObjectName, type, description, sourceFilePath);

        public ThreePartAdvancedNumber Copy()
        {
            var @base = _base.Copy();
            var value = _peak.Copy(@base);
            var current = _current.Copy(value);
            var copy = new ThreePartAdvancedNumber(@base, value, current);
            return copy;
        }
    }

    [Serializable]
    public class ThreePartAdvancedNumberBase : ChangeableValue
    {
        [SerializeField] private float _startValue = default;

        public float StartValue => _startValue;
        protected override float SubValue => _startValue;
        public override NumberChangeCommandPart Part => NumberChangeCommandPart.THREE_PART_BASE_VALUE;

        public ThreePartAdvancedNumberBase() : base(100) { }

        public void FeedData(float startValue)
        {
            _startValue = startValue;
            _changeAmountFromSub = 0;
            _changeCommands.Clear();
        }

        protected override float CalculateRawValue(float subAmount, float changeAmount) => subAmount + changeAmount;
        protected override float CalculateValue(float subAmount, float changeAmount)
        {
            var value = RawValue;
            return value;
        }

        public float CalculateResultWithAdditions(float startValueAddition = 0,
            float baseAddition = 0)
        {
            var rawValue = CalculateRawValue(_startValue + startValueAddition, _changeAmountFromSub + baseAddition);
            var value = rawValue;
            return value;
        }

        public ThreePartAdvancedNumberBase Copy()
        {
            var copy = new ThreePartAdvancedNumberBase();
            copy.FeedData(_startValue);
            copy._changeAmountFromSub = _changeAmountFromSub;
            copy._rawValue = _rawValue;
            copy._lastValue = _lastValue;
            copy._value = _value;
            copy._compressOnCount = _compressOnCount;
            copy._changeCommands = new List<NumberChangeCommand>(_changeCommands);
            return copy;
        }
    }

    [Serializable]
    public class ThreePartAdvancedNumberPeak : ChangeableValue
    {
        private ThreePartAdvancedNumberBase _base = default;

        public ThreePartAdvancedNumberBase BaseValue => _base;
        public override NumberChangeCommandPart Part => NumberChangeCommandPart.THREE_PART_VALUE;
        protected override float SubValue => _base.Value;

        public ThreePartAdvancedNumberPeak(ThreePartAdvancedNumberBase @base) : base(100) 
        {
            _base = @base;
            _base.OnNewChangeCommandActions.Add(100, cc => _lastValue = Value);
            _base.OnNewValueActions.Add(0, HandleBaseValueChange);
        }

        public void FeedData()
        {
            _changeAmountFromSub = 0;
            _changeCommands.Clear();
        }

        private void HandleBaseValueChange(NumberChangeCommand changeCommand)
        {
            OnNewValueActions.CallActionsSafely(changeCommand);
        }

        protected override float CalculateRawValue(float subAmount, float changeAmount) => subAmount * (1 + changeAmount / 100);
        protected override float CalculateValue(float subAmount, float changeAmount)
        {
            var value = RawValue;
            return value;
        }

        public float CalculateResultWithAdditions(float startValueAddition = 0,
            float baseAddition = 0, float peakAddition = 0)
        {
            var @base = _base.CalculateResultWithAdditions(startValueAddition, baseAddition);
            var rawValue = CalculateRawValue(@base, _changeAmountFromSub + peakAddition);
            var value = rawValue;
            return value;
        }

        public ThreePartAdvancedNumberPeak Copy(ThreePartAdvancedNumberBase @base)
        {
            var copy = new ThreePartAdvancedNumberPeak(@base);
            copy._changeAmountFromSub = _changeAmountFromSub;
            copy._rawValue = _rawValue;
            copy._lastValue = _lastValue;
            copy._value = _value;
            copy._compressOnCount = _compressOnCount;
            copy._changeCommands = new List<NumberChangeCommand>(_changeCommands);
            return copy;
        }
    }

    [Serializable]
    public class ThreePartAdvancedNumberCurrent : ChangeableValue
    {
        private ThreePartAdvancedNumberPeak _peak = default;
        [SerializeField] private float? _dummyMin = default;
        [SerializeField] private float? _dummyMax = default;
        [SerializeField] private bool _dummyMaxBaseOnValue = default;
        [SerializeField] private bool _removeMinExceed = default;
        [SerializeField] private bool _removeMaxExceed = default;
        [SerializeField] private bool _keepRatio = default;
        [SerializeField] private bool _isFix = default;
        [SerializeField] private float _fixCurrentValue = default;

        public bool IsFix => _isFix;
        public float FixCurrentValue => _fixCurrentValue;
        public ThreePartAdvancedNumberPeak Peak => _peak;
        public override NumberChangeCommandPart Part => NumberChangeCommandPart.THREE_PART_CURRENT_VALUE;
        protected override float SubValue => _peak.Value;
        public float? DummyMax
        {
            get
            {
                if (_dummyMax.HasValue)
                    return _dummyMax;
                else if (_dummyMaxBaseOnValue)
                    return _peak.Value;
                return default;
            }
        }

        public ThreePartAdvancedNumberCurrent(ThreePartAdvancedNumberPeak value, float? dummyMin = null,
            float? dummyMax = null, bool dummyMaxBaseOnValue = false, bool removeMinExceed = false,
            bool removeMaxExceed = false, bool keepRatio = false) : base(10)
        {
            _peak = value;
            _dummyMin = dummyMin;
            _dummyMax = dummyMax;
            _dummyMaxBaseOnValue = dummyMaxBaseOnValue;
            _removeMinExceed = removeMinExceed;
            _removeMaxExceed = removeMaxExceed;
            _keepRatio = keepRatio;
            _peak.BaseValue.OnNewChangeCommandActions.Add(100, cc => _lastValue = base.Value);
            _peak.OnNewChangeCommandActions.Add(100, cc => _lastValue = base.Value);
            _peak.OnNewValueActions.Add(0, HandlePeakChange);
            OnNewChangeCommandActions.Add(99, RemoveExceedAmount);
        }

        public void FeedData()
        {
            _changeAmountFromSub = 0;
            _changeCommands.Clear();
        }

        public void Fix(float fixCurrentValue)
        {
            _isFix = true;
            _fixCurrentValue = fixCurrentValue;
            OnNewValueActions.CallActionsSafely(null);

        }
        public void UnFix()
        {
            _isFix = false;
            OnNewValueActions.CallActionsSafely(null);
        }

        private void HandlePeakChange(NumberChangeCommand changeCommand)
        {
            if (_keepRatio)
                KeepRatio();
            OnNewValueActions.CallActionsSafely(changeCommand);
        }

        private void KeepRatio()
        {
            if (_peak.LastValue == _peak.Value || _peak.LastValue == 0f ||
                _lastValue == 0f || _peak.LastValue == _lastValue)
                return;

            var valueChangeRatio = _peak.Value / _peak.LastValue;
            var correctCurrentValue = valueChangeRatio * _lastValue;
            var correctCurrentValueChangeAmount = correctCurrentValue - base.Value;
            var changeCommand = new NumberChangeCommand(Part,
               correctCurrentValueChangeAmount, null, "RATIO",
               $"KeepingRatio {valueChangeRatio} {correctCurrentValue} {correctCurrentValueChangeAmount}");
            RemoveExceedAmount(changeCommand);
            ApplyChange(changeCommand);
        }

        private void RemoveExceedAmount(NumberChangeCommand changeCommand)
        {
            if (!_removeMinExceed && !_removeMaxExceed)
                return;
            var futureCurrentValue = CalculateValueWithAdditions(
                currentAddition: changeCommand.Amount.Current.Value, applyMinMaxRestrictions: false);
            if (_removeMinExceed && futureCurrentValue < _dummyMin.Value)
            {
                var modifiedAmount = _dummyMin.Value - futureCurrentValue;
                changeCommand.Amount.Current.Change(modifiedAmount, null,
                    "MIN_EXCEED", "Current value should not exceed from value.");
            }
            var dummyMax = DummyMax;
            if (_removeMaxExceed && dummyMax.HasValue && futureCurrentValue > dummyMax.Value)
            {
                var modifiedAmount = dummyMax.Value - futureCurrentValue;
                changeCommand.Amount.Current.Change(modifiedAmount, null,
                        "MAX_EXCEED", "Current value should not exceed from value.");
            }
        }

        protected override float CalculateRawValue(float subAmount, float changeAmount) => subAmount + changeAmount;
        protected override float CalculateValue(float subAmount, float changeAmount)
        {
            if (_isFix)
                return _fixCurrentValue;
            var rawValue = RawValue;
            var value = Mathf.Clamp(rawValue, _dummyMin ?? rawValue, DummyMax ?? rawValue);
            return value;
        }

        public float CalculateValueWithAdditions(float startValueAddition = 0, 
            float baseAddition = 0, float peakAddition = 0, float currentAddition = 0, 
            bool applyMinMaxRestrictions = true)
        {
            var value = _peak.CalculateResultWithAdditions(startValueAddition, baseAddition, peakAddition);
            var amount = CalculateRawValue(value, _changeAmountFromSub + currentAddition);
            var result = amount;
            if(applyMinMaxRestrictions)
                result = Mathf.Clamp(amount, _dummyMin ?? amount, DummyMax ?? amount);
            return result;
        }

        public ThreePartAdvancedNumberCurrent Copy(ThreePartAdvancedNumberPeak value)
        {
            var copy = new ThreePartAdvancedNumberCurrent(value, _dummyMin, _dummyMax,
                _dummyMaxBaseOnValue, _removeMinExceed, _removeMaxExceed, _keepRatio);
            copy._changeAmountFromSub = _changeAmountFromSub;
            copy._rawValue = _rawValue;
            copy._lastValue = _lastValue;
            copy._peak = _peak;
            copy._compressOnCount = _compressOnCount;
            copy._changeCommands = new List<NumberChangeCommand>(_changeCommands);
            return copy;
        }
    }
}