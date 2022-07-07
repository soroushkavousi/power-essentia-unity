//using UnityEngine;

//public class DemonAttackDamage : AttackDamage, IObserver
//{
//    [SerializeField] protected LevelModifier _levelModifier;

//    public DemonAttackDamage(float baseValue,
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
//        newValue = newValue.AddPercentage(_buffModifier.Value);
//        _value.Value = newValue;
//    }
//}
