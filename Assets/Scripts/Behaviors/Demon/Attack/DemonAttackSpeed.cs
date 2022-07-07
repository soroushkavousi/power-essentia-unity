//using UnityEngine;

//public class DemonAttackSpeed : AttackSpeed, IObserver
//{
//    [SerializeField] protected LevelModifier _levelModifier;

//    public DemonAttackSpeed(float baseValue,
//        Observable<int> level, float oneLevelPercentage) 
//        : base(baseValue)
//    {
//        _levelModifier = new(level, oneLevelPercentage);
//        _levelModifier.Attach(this);
//        CalculateValue();
//    }

//    protected override void CalculateValue()
//    {
//        var newValue = _startValue;
//        newValue = newValue.AddPercentage(_levelModifier.Value);
//        newValue = newValue.AddPercentage(_slowSpeedModifier.Value);
//        _value.Value = newValue.WithMin(0f);
//    }
//}
