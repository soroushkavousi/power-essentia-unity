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
    public class OnePartAdvancedNumber : ChangeableValue
    {
        [SerializeField] private float _startValue = default;
        [SerializeField] private float? _dummyMin = default;
        [SerializeField] private float? _dummyMax = default;
        [SerializeField] private bool _removeMinExceed = default;
        [SerializeField] private bool _removeMaxExceed = default;
        [SerializeField] private bool _isFix = default;
        [SerializeField] private float _fixValue = default;

        public float StartValue => _startValue;
        public bool IsFix => _isFix;
        public float FixValue => _fixValue;
        protected override float SubValue => _startValue;
        public override NumberChangeCommandPart Part => NumberChangeCommandPart.ONE_PART_VALUE;

        public OnePartAdvancedNumber(float? dummyMin = null, float? dummyMax = null, 
            bool removeMinExceed = false, bool removeMaxExceed = false) : base(100)
        {
            _dummyMin = dummyMin;
            _dummyMax = dummyMax;
            _removeMinExceed = removeMinExceed;
            _removeMaxExceed = removeMaxExceed;
            OnNewChangeCommandActions.Add(99, RemoveExceedAmount);
        }

        public void FeedData(float startValue)
        {
            _startValue = startValue;
            _changeAmountFromSub = 0;
            _changeCommands.Clear();
            var changeCommand = new NumberChangeCommand(NumberChangeCommandPart.FEED_DATA,
                0, nameof(OnePartAdvancedNumber), "FEED_DATA", "");
            OnNewValueActions.CallActionsSafely(changeCommand);
        }

        public void Fix(float fixValue)
        {
            _isFix = true;
            _fixValue = fixValue;
        }
        public void UnFix() => _isFix = false;

        private void RemoveExceedAmount(NumberChangeCommand changeCommand)
        {
            if (!_removeMinExceed && !_removeMaxExceed)
                return;
            var futureCurrentValue = CalculateResultWithAdditions(
                valueAddition: changeCommand.Amount.Current.Value, applyMinMaxRestrictions: false);
            if (_removeMinExceed && futureCurrentValue < _dummyMin.Value)
            {
                var modifiedAmount = _dummyMin.Value - futureCurrentValue;
                changeCommand.Amount.Current.Change(modifiedAmount, null,
                    "MIN_EXCEED", "Current value should not exceed from value.");
            }
            if (_removeMaxExceed && futureCurrentValue > _dummyMax.Value)
            {
                var modifiedAmount = _dummyMax.Value - futureCurrentValue;
                changeCommand.Amount.Current.Change(modifiedAmount, null,
                        "MAX_EXCEED", "Current value should not exceed from value.");
            }
        }

        protected override float CalculateRawValue(float subAmount, float changeAmount) => subAmount + changeAmount;
        protected override float CalculateValue(float subAmount, float changeAmount)
        {
            if (_isFix)
                return _fixValue;
            var amount = RawValue;
            var result = Mathf.Clamp(amount, _dummyMin ?? amount, _dummyMax ?? amount);
            return result;
        }

        public float CalculateResultWithAdditions(float startValueAddition = 0,
            float valueAddition = 0, bool applyMinMaxRestrictions = true)
        {
            var amount = CalculateRawValue(_startValue + startValueAddition, _changeAmountFromSub + valueAddition);
            var result = amount;
            if(applyMinMaxRestrictions)
                result = Mathf.Clamp(amount, _dummyMin ?? amount, _dummyMax ?? amount);
            return result;
        }

        public OnePartAdvancedNumber Copy()
        {
            var copy = new OnePartAdvancedNumber(_dummyMin, _dummyMax,
                _removeMinExceed, _removeMaxExceed);
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
}