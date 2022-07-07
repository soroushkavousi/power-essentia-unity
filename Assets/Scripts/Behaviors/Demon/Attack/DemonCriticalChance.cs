//using UnityEngine;

//public class DemonCriticalChance : CriticalChance, IObserver
//{
//    [SerializeField] protected LevelModifier _levelModifier;

//    public DemonCriticalChance(float startValue,
//        Observable<int> level, float oneLevelPercentage) 
//        : base(startValue)
//    {
//        _levelModifier = new(level, oneLevelPercentage);
//        _levelModifier.Attach(this);
//        CalculateValue();
//    }

//    protected override void CalculateValue()
//    {
//        var newValue = _startValue;
//        newValue = newValue.AddPercentage(_levelModifier.Value);
//        _value.Value = newValue.WithMin(0f).WithMax(100f);
//    }
//}
