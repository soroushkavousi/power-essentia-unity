using Assets.Scripts.Models;
using System;
using UnityEngine;

public abstract class WeaponBehavior : MonoBehaviour, ISubject<HitParameters>
{
    [Header(Constants.HeaderStart + nameof(WeaponBehavior) + Constants.HeaderEnd)]

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] protected Number _attackDamage;
    [SerializeField] protected Number _attackSpeed;
    [SerializeField] protected Number _criticalChance;
    [SerializeField] protected Number _criticalDamage;
    private WeaponStaticData _staticData = default;
    protected Observable<int> _level = default;
    private readonly ObserverCollection<HitParameters> _hitObservers = new();

    public Number AttackDamage => _attackDamage;
    public Number AttackSpeed => _attackSpeed;
    public Number CriticalChance => _criticalChance;
    public Number CriticalDamage => _criticalDamage;
    public Func<GameObject, GameObject> IsTargetEnemyFunction { get; set; }

    protected void FeedData(WeaponStaticData staticData)
    {
        _staticData = staticData;
    }

    public virtual void Initialize(Observable<int> level,
        Func<GameObject, GameObject> isTargetEnemyFunction)
    {
        _level = level;

        _attackDamage = new(_staticData.Damage, level,
            _staticData.DamageLevelPercentage, minPercentage: -95f);

        _attackSpeed = new(_staticData.Speed, level,
            _staticData.SpeedLevelPercentage, min: 0f, minPercentage: -95f);

        _criticalChance = new(_staticData.CriticalChance, level,
            _staticData.CriticalChanceLevelPercentage, min: 0f, max: 100f);

        _criticalDamage = new(_staticData.CriticalDamage, level,
            _staticData.CriticalDamageLevelPercentage, min: 0f);

        IsTargetEnemyFunction = isTargetEnemyFunction;
    }

    public void Attach(IObserver<HitParameters> observer) => _hitObservers.Add(observer);
    public void Detach(IObserver<HitParameters> observer) => _hitObservers.Remove(observer);
    public void Notify(HitParameters hitParameters) => _hitObservers.Notify(this, hitParameters);
}
