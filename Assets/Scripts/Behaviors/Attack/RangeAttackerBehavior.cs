using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackerBehavior))]
public class RangeAttackerBehavior : MonoBehaviour
{
    [SerializeField] private Transform _projectileSpawnLocation = default;
    [SerializeField] private ProjectileBehavior _animationProjectileBehavior = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    [SerializeField] private Vector2 _attackTargetPosition = default;
    [SerializeField] private Vector2 _aimTargetPosition = default;
    [SerializeField] private bool _aimingIsFinished = default;
    private RangeAttackerStaticData _staticData = default;
    private AttackerBehavior _attackerBehavior = default;
    private RotationBehavior _rotationBehavior = default;

    public Vector2 AttackTargetPosition { get => _attackTargetPosition; set => _attackTargetPosition = value; }
    public Transform ProjectileSpawnLocation => _projectileSpawnLocation;
    public OrderedList<Action> OnReloadActions { get; } = new OrderedList<Action>();
    public OrderedList<Action> OnAimActions { get; } = new OrderedList<Action>();
    public OrderedList<Action<ProjectileBehavior>> OnCreateProjectileActions { get; } = new OrderedList<Action<ProjectileBehavior>>();
    public bool AimingIsFinished => _aimingIsFinished;

    public void FeedData(RangeAttackerStaticData staticData,
        Func<GameObject, GameObject> isTargetEnemyFunction)
    {
        _attackerBehavior = GetComponent<AttackerBehavior>();
        _rotationBehavior = GetComponent<RotationBehavior>();

        _staticData = staticData;
        _attackerBehavior.FeedData(staticData.AttackerData, isTargetEnemyFunction);
        _attackerBehavior.OnAttackingActions.Add(FireProjectile);
        if(_animationProjectileBehavior != default)
            _animationProjectileBehavior.Initialize(
                _staticData.ProjectileStaticData,
                _attackerBehavior.AttackDamage.Value,
                _attackerBehavior.CriticalChance.Value,
                _attackerBehavior.CriticalDamage.Value,
                _attackerBehavior.AttackSound, _attackerBehavior.IsTargetEnemyFunction);
    }

    public void FireProjectile()
    {
        var newProjectileBehavior = Instantiate(_staticData.ProjectileBehavior, _projectileSpawnLocation.position,
            _projectileSpawnLocation.rotation, transform.parent);
        newProjectileBehavior.Initialize(
            _staticData.ProjectileStaticData,
            _attackerBehavior.AttackDamage.Value,
            _attackerBehavior.CriticalChance.Value,
            _attackerBehavior.CriticalDamage.Value,
            _attackerBehavior.AttackSound, _attackerBehavior.IsTargetEnemyFunction);
        newProjectileBehavior.MovementBehavior.TargetPosition = _aimTargetPosition;
        OnCreateProjectileActions.CallActionsSafely(newProjectileBehavior);
        newProjectileBehavior.OnHitActions.Add(_attackerBehavior.HandleHitEvent);

        //newProjectileMovementBehavior.transform.SetParent(transform.parent, true);
        newProjectileBehavior.MovementBehavior.StartMoving();
        MusicPlayerBehavior.Instance.AudioSource.PlayOneShot(_staticData.ShootSound);
    }

    public void Reload()
    {
        OnReloadActions.CallActionsSafely();
    }

    public void Aim()
    {
        _aimingIsFinished = false;
        _aimTargetPosition = AttackTargetPosition;
        _attackerBehavior.StartAttacking();
        OnAimActions.CallActionsSafely();
        if (_rotationBehavior == null)
        {
            _aimingIsFinished = true;
            return;
        }
        _rotationBehavior.RotateToTarget(_aimTargetPosition,
            ProjectileSpawnLocation.localPosition);
        StartCoroutine(CheckAimingStatus());
    }

    //public void AimToMousePosition()
    //{
    //    AttackTargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    Aim();
    //}

    private IEnumerator CheckAimingStatus()
    {
        yield return new WaitWhile(() => _rotationBehavior.IsRotating);
        _aimingIsFinished = true;
    }
}
