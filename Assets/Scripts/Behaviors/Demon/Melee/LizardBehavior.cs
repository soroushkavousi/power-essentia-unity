using Assets.Scripts.Enums;
using UnityEngine;

[RequireComponent(typeof(StateManagerBehavior))]
[RequireComponent(typeof(MeleeAttackerBehavior))]
[RequireComponent(typeof(AIAttackerBehavior))]
public class LizardBehavior : DemonBehavior
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.HeaderStart + nameof(LizardBehavior) + Constants.HeaderEnd)]
    [SerializeField] private MeleeDemonStaticData _staticData = default;
    [SerializeField] private MeleeWeaponBehavior _meleeWeaponBehavior = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    private StateManagerBehavior _stateManagerBehavior = default;
    private MeleeAttackerBehavior _meleeAttackerBehavior = default;
    private AIAttackerBehavior _aiAttackerBehavior = default;
    private MovementBehavior _movementBehavior = default;

    public override void Initialize(int level)
    {
        base.Initialize(level);
        base.FeedData(_staticData);

        _stateManagerBehavior = GetComponent<StateManagerBehavior>();
        _stateManagerBehavior.FeedData(typeof(LizardState), LizardState.IDLING,
            CheckState, StopOldState, StartNewState);

        _meleeWeaponBehavior.Initialize(_level, IsTargetEnemy);
        _meleeAttackerBehavior = GetComponent<MeleeAttackerBehavior>();
        _meleeAttackerBehavior.FeedData(_staticData.MeleeAttackerStaticData,
            _meleeWeaponBehavior, IsTargetEnemy);

        _aiAttackerBehavior = GetComponent<AIAttackerBehavior>();
        _aiAttackerBehavior.FeedData(_staticData.AIAttackerStaticData);

        _movementBehavior = GetComponent<MovementBehavior>();
        _movementBehavior.FeedData(_staticData.MovementStaticData);
        _movementBehavior.MoveWithDirection(Vector2.left);
    }

    private void CheckState()
    {
        var currentState = _stateManagerBehavior.State.Value.ToEnum<LizardState>();
        switch (currentState)
        {
            case LizardState.IDLING:
                CheckIdlingState();
                break;
            case LizardState.MOVING:
                CheckMovingState();
                break;
            case LizardState.ATTACKING:
                CheckAttackingState();
                break;
            case LizardState.PRE_STUNNING:
                CheckPreStunningState();
                break;
            case LizardState.STUNNING:
                CheckStunningState();
                break;
            case LizardState.POST_STUNNING:
                CheckPostStunningState();
                break;
        }
    }

    private void StopOldState()
    {
        var oldState = _stateManagerBehavior.State.LastValue.ToEnum<LizardState>();
        switch (oldState)
        {
            case LizardState.IDLING:
                break;
            case LizardState.MOVING:
                _movementBehavior.StopMoving();
                break;
            case LizardState.ATTACKING:
                _meleeAttackerBehavior.StopAttacking();
                break;
            case LizardState.PRE_STUNNING:
                _statusOwnerBehavior.StunStatusBehavior.StopPreStunning();
                break;
            case LizardState.STUNNING:
                _statusOwnerBehavior.StunStatusBehavior.StopStunning();
                break;
            case LizardState.POST_STUNNING:
                _statusOwnerBehavior.StunStatusBehavior.StopPostStunning();
                break;
        }
    }

    private void StartNewState()
    {
        var newState = _stateManagerBehavior.State.Value.ToEnum<LizardState>();
        switch (newState)
        {
            case LizardState.IDLING:
                break;
            case LizardState.MOVING:
                _movementBehavior.MoveWithDirection(Vector2.left);
                break;
            case LizardState.ATTACKING:
                _meleeAttackerBehavior.StartAttacking();
                break;
            case LizardState.PRE_STUNNING:
                _statusOwnerBehavior.StunStatusBehavior.StartPreStunning();
                break;
            case LizardState.STUNNING:
                _statusOwnerBehavior.StunStatusBehavior.StartStunning();
                break;
            case LizardState.POST_STUNNING:
                _statusOwnerBehavior.StunStatusBehavior.StartPostStunning();
                break;
        }
    }

    private void CheckIdlingState()
    {
        if (_statusOwnerBehavior.StunStatusBehavior.IsAffected)
            _stateManagerBehavior.GoToTheNextState(LizardState.PRE_STUNNING);
        else if (_aiAttackerBehavior.EnemiesAreInVision)
            _stateManagerBehavior.GoToTheNextState(LizardState.ATTACKING);
        else
            _stateManagerBehavior.GoToTheNextState(LizardState.MOVING);
    }

    private void CheckMovingState()
    {
        if (_statusOwnerBehavior.StunStatusBehavior.IsAffected)
            _stateManagerBehavior.GoToTheNextState(LizardState.PRE_STUNNING);
        else if (_aiAttackerBehavior.EnemiesAreInVision)
            _stateManagerBehavior.GoToTheNextState(LizardState.ATTACKING);
    }

    private void CheckAttackingState()
    {
        if (_statusOwnerBehavior.StunStatusBehavior.IsAffected)
            _stateManagerBehavior.GoToTheNextState(LizardState.PRE_STUNNING);
        else if (_aiAttackerBehavior.EnemiesAreInVision == false)
            _stateManagerBehavior.GoToTheNextState(LizardState.MOVING);
    }

    private void CheckPreStunningState()
    {
        _stateManagerBehavior.GoToTheNextState(LizardState.STUNNING);
    }

    private void CheckStunningState()
    {
        if (_statusOwnerBehavior.StunStatusBehavior.IsAffected == false)
            _stateManagerBehavior.GoToTheNextState(LizardState.POST_STUNNING);
    }

    private void CheckPostStunningState()
    {
        if (_statusOwnerBehavior.StunStatusBehavior.IsAffected)
            _stateManagerBehavior.GoToTheNextState(LizardState.PRE_STUNNING);
        else if (_aiAttackerBehavior.EnemiesAreInVision)
            _stateManagerBehavior.GoToTheNextState(LizardState.ATTACKING);
        else
            _stateManagerBehavior.GoToTheNextState(LizardState.MOVING);
    }
}
