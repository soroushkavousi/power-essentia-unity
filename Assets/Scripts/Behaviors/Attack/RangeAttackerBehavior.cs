using System;
using System.Collections;
using UnityEngine;

public class RangeAttackerBehavior : AttackerBehavior
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.HeaderStart + nameof(RangeAttackerBehavior) + Constants.HeaderEnd)]
    [SerializeField] protected Transform _projectileShotLocation = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] protected RangeAttackerState _state = default;
    [SerializeField] protected RangeWeaponBehavior _rangeWeaponBehavior = default;
    [SerializeField] protected Vector2 _attackTargetPosition = default;
    [SerializeField] protected Vector2 _aimTargetPosition = default;
    [SerializeField] protected bool _aimingIsFinished = default;
    private RangeAttackerStaticData _staticData = default;
    protected RotationUtils _rotationUtils = default;

    public override AttackerState AttackerState => (AttackerState)_state;
    public RangeAttackerState State => _state;
    public RangeWeaponBehavior RangeWeaponBehavior => _rangeWeaponBehavior;
    public Vector2 AttackTargetPosition { get => _attackTargetPosition; set => _attackTargetPosition = value; }
    public bool AimingIsFinished => _aimingIsFinished;
    public Transform ProjectileShotLocation => _projectileShotLocation;

    public void FeedData(RangeAttackerStaticData staticData, RangeWeaponBehavior rangeWeaponBehavior,
        Func<GameObject, GameObject> isTargetEnemyFunction)
    {
        _staticData = staticData;
        _rangeWeaponBehavior = rangeWeaponBehavior;
        base.FeedData(_staticData, _rangeWeaponBehavior, isTargetEnemyFunction);
        _rotationUtils = new RotationUtils(this, _projectileShotLocation.localPosition);
    }

    public override void StartAttacking()
    {
        if (_state == RangeAttackerState.STARTED)
            return;
        _state = RangeAttackerState.STARTED;
        Notify();
        if (_movementBehavior != null)
            _movementBehavior.StopMoving();
    }

    protected override void Attack()
    {
        //if(_staticData.AttackSound != null)
        //    _audioSource.PlayOneShot(_staticData.AttackSound, 0.5f);
        //_audioSource.Play();

        _state = RangeAttackerState.ATTACKING;
        _rangeWeaponBehavior.FireRoundProjectiles(_aimTargetPosition);
        Notify();
    }

    public override void StopAttacking()
    {
        if (_state == RangeAttackerState.STOPPED)
            return;
        _state = RangeAttackerState.STOPPED;
        Notify();
    }

    public void Reload()
    {
        _state = RangeAttackerState.RELOADING;
        Notify();
    }

    public void Aim()
    {
        _aimingIsFinished = false;
        _aimTargetPosition = AttackTargetPosition;
        StartAttacking();
        _state = RangeAttackerState.AIMING;
        if (_rotationUtils == null)
        {
            _aimingIsFinished = true;
            return;
        }
        _rotationUtils.RotateToTarget(_aimTargetPosition);
        StartCoroutine(CheckAimingStatus());
        Notify();
    }

    //public void AimToMousePosition()
    //{
    //    AttackTargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    Aim();
    //}

    private IEnumerator CheckAimingStatus()
    {
        yield return new WaitWhile(() => _rotationUtils.IsRotating);
        _aimingIsFinished = true;
    }
}
