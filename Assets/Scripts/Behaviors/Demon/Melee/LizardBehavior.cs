using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateManagerBehavior))]
[RequireComponent(typeof(DemonBehavior))]
[RequireComponent(typeof(MeleeAttackerBehavior))]
[RequireComponent(typeof(AIAttackerBehavior))]
public class LizardBehavior : MonoBehaviour
{
    [SerializeField] private LizardStaticData _staticData = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    private StateManagerBehavior _stateManagerBehavior = default;
    private DemonBehavior _demonBehavior = default;
    private MeleeAttackerBehavior _meleeAttackerBehavior = default;
    private AttackerBehavior _attackerBehavior = default;
    private AIAttackerBehavior _aiAttackerBehavior = default;
    private MovementBehavior _movementBehavior = default;
    private StatusOwnerBehavior _statusOwnerBehavior = default;

    public void Initialize()
    {
        _demonBehavior = GetComponent<DemonBehavior>();
        _demonBehavior.FeedData(_staticData.DemonStaticData);

        _stateManagerBehavior = GetComponent<StateManagerBehavior>();
        _stateManagerBehavior.FeedData(typeof(LizardState), LizardState.IDLING,
            CheckState, StopOldState, StartNewState);

        _meleeAttackerBehavior = GetComponent<MeleeAttackerBehavior>();
        _meleeAttackerBehavior.FeedData(_staticData.MeleeAttackerStaticData, IsTargetEnemy);

        _aiAttackerBehavior = GetComponent<AIAttackerBehavior>();
        _aiAttackerBehavior.FeedData(_staticData.AIAttackerStaticData);

        _movementBehavior = GetComponent<MovementBehavior>();
        _movementBehavior.FeedData(_staticData.MovementStaticData);
        _movementBehavior.StartMoving();

        _statusOwnerBehavior = GetComponent<StatusOwnerBehavior>();
        _attackerBehavior = GetComponent<AttackerBehavior>();
    }

    private void CheckState()
    {
        var currentState = _stateManagerBehavior.State.EnumValue.To<LizardState>();
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
        var oldState = _stateManagerBehavior.State.LastEnumValue.To<LizardState>();
        switch (oldState)
        {
            case LizardState.IDLING:
                break;
            case LizardState.MOVING:
                _movementBehavior.StopMoving();
                break;
            case LizardState.ATTACKING:
                _attackerBehavior.StopAttacking();
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
        var newState = _stateManagerBehavior.State.EnumValue.To<LizardState>();
        switch (newState)
        {
            case LizardState.IDLING:
                break;
            case LizardState.MOVING:
                _movementBehavior.StartMoving();
                break;
            case LizardState.ATTACKING:
                _attackerBehavior.StartAttacking();
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

    private GameObject IsTargetEnemy(GameObject target)
    {
        var bodyAreaBehavior = target.GetComponent<BodyAreaBehavior>();
        if (bodyAreaBehavior != null)
        {
            target = bodyAreaBehavior.BodyBehavior.gameObject;
        }

        var castleBehavior = target.GetComponent<WallBehavior>();
        if (castleBehavior != null)
        {
            return target;
        }

        return null;
    }
}
