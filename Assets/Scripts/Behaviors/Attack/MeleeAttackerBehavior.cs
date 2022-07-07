using System;
using UnityEngine;

public class MeleeAttackerBehavior : AttackerBehavior
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.HeaderStart + nameof(MeleeAttackerBehavior) + Constants.HeaderEnd)]

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private MeleeAttackerState _state = default;
    [SerializeField] protected MeleeWeaponBehavior _meleeWeaponBehavior = default;
    private MeleeAttackerStaticData _staticData = default;

    public override AttackerState AttackerState => (AttackerState)_state;
    public MeleeAttackerState State => _state;

    public void FeedData(MeleeAttackerStaticData staticData, MeleeWeaponBehavior _meleeWeaponBehavior,
        Func<GameObject, GameObject> isTargetEnemyFunction)
    {
        _staticData = staticData;
        base.FeedData(staticData, _meleeWeaponBehavior, isTargetEnemyFunction);
    }

    public override void StartAttacking()
    {
        if (_state == MeleeAttackerState.STARTED)
            return;
        _state = MeleeAttackerState.STARTED;
        Notify();
        if (_movementBehavior != null)
            _movementBehavior.StopMoving();
    }

    protected override void Attack()
    {
        //if(_staticData.AttackSound != null)
        //    _audioSource.PlayOneShot(_staticData.AttackSound, 0.5f);
        //_audioSource.Play();

        _state = MeleeAttackerState.ATTACKING;
        _meleeWeaponBehavior.StrikeTheEnemy(CurrentEnemy);
        Notify();
    }

    public override void StopAttacking()
    {
        if (_state == MeleeAttackerState.STOPPED)
            return;
        _state = MeleeAttackerState.STOPPED;
        Notify();
    }
}
