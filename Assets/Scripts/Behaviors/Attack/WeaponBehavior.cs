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
    private WeaponStaticData _weaponStaticData = default;
    protected Level _level = default;
    private readonly ObserverCollection<HitParameters> _hitObservers = new();

    public Number AttackDamage => _attackDamage;
    public Number AttackSpeed => _attackSpeed;
    public Number CriticalChance => _criticalChance;
    public Number CriticalDamage => _criticalDamage;
    public Func<GameObject, GameObject> IsTargetEnemyFunction { get; set; }

    protected void FeedData(WeaponStaticData weaponStaticData)
    {
        _weaponStaticData = weaponStaticData;
    }

    public virtual void Initialize(Level level,
        Func<GameObject, GameObject> isTargetEnemyFunction)
    {
        _level = level;

        _attackDamage = new(_level, _weaponStaticData.DamageLevelInfo,
            minPercentage: -95f);

        var maxAttackSpeed = GameManagerBehavior.Instance.Settings.MaxAttackSpeed;
        _attackSpeed = new(_level, _weaponStaticData.SpeedLevelInfo,
            min: 0f, max: maxAttackSpeed, minPercentage: -95f);

        _criticalChance = new(_level, _weaponStaticData.CriticalChanceLevelInfo,
            min: 0f, max: 100f);

        _criticalDamage = new(_level, _weaponStaticData.CriticalDamageLevelInfo,
            min: 0f);

        IsTargetEnemyFunction = isTargetEnemyFunction;
    }

    public void Attach(IObserver<HitParameters> observer) => _hitObservers.Add(observer);
    public void Detach(IObserver<HitParameters> observer) => _hitObservers.Remove(observer);
    public void Notify(HitParameters hitParameters) => _hitObservers.Notify(this, hitParameters);
}
