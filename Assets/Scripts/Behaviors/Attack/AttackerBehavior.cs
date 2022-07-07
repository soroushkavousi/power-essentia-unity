using Assets.Scripts.Models;
using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public abstract class AttackerBehavior : MonoBehaviour, IObserver, ISubject,
    ISubject<HitParameters>, IObserver<HitParameters>
{
    [Header(Constants.HeaderStart + nameof(AttackerBehavior) + Constants.HeaderEnd)]
    [SerializeField] protected Transform _weaponLocation = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] protected AttackerState _attackerState = default;
    [SerializeField] protected WeaponBehavior _weaponBehavior = default;
    [SerializeField] protected GameObject _currentEnemy = default;
    private AttackerStaticData _staticData = default;
    protected readonly string _attackSpeedName = "AttackSpeed";
    protected Animator _animator = default;
    protected MovementBehavior _movementBehavior = default;
    protected AudioSource _audioSource = default;
    protected readonly ObserverCollection _observers = new();
    protected readonly ObserverCollection<HitParameters> _hitObservers = new();

    public Transform WeaponLocation => _weaponLocation;
    public abstract AttackerState AttackerState { get; }
    public Func<GameObject, GameObject> IsTargetEnemyFunction { get; set; }
    public GameObject CurrentEnemy { get => _currentEnemy; set => _currentEnemy = value; }

    protected void FeedData(AttackerStaticData staticData, WeaponBehavior weaponBehavior,
        Func<GameObject, GameObject> isTargetEnemyFunction)
    {
        _staticData = staticData;
        _animator = GetComponent<Animator>();
        _movementBehavior = GetComponent<MovementBehavior>();
        _audioSource = GetComponent<AudioSource>();
        IsTargetEnemyFunction = isTargetEnemyFunction;
        _weaponBehavior = weaponBehavior;
        _weaponBehavior.Attach(this);
        _weaponBehavior.AttackSpeed.Attach(this);
        _animator.SetFloat(_attackSpeedName, _weaponBehavior.AttackSpeed.Value);
    }

    public abstract void StartAttacking();
    protected abstract void Attack();
    public abstract void StopAttacking();

    private void SubmitAttackSpeedChange()
    {
        _animator.SetFloat(_attackSpeedName, _weaponBehavior.AttackSpeed.Value);
    }

    public void OnNotify(ISubject subject)
    {
        if (subject == _weaponBehavior.AttackSpeed)
        {
            SubmitAttackSpeedChange();
        }
    }

    public void OnNotify(ISubject<HitParameters> subject, HitParameters hitParameters)
    {
        if (ReferenceEquals(subject, _weaponBehavior))
        {
            Notify(hitParameters);
        }
    }

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
    public void Notify() => _observers.Notify(this);

    public void Attach(IObserver<HitParameters> observer) => _hitObservers.Add(observer);
    public void Detach(IObserver<HitParameters> observer) => _hitObservers.Remove(observer);
    public void Notify(HitParameters hitParameters) => _hitObservers.Notify(this, hitParameters);
}
