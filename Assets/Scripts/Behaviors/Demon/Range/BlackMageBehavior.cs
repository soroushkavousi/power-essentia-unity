using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateManagerBehavior))]
[RequireComponent(typeof(DemonBehavior))]
[RequireComponent(typeof(MovementBehavior))]
[RequireComponent(typeof(RangeAttackerBehavior))]
[RequireComponent(typeof(AIAttackerBehavior))]
public class BlackMageBehavior : MonoBehaviour
{
    [SerializeField] private BlackMageStaticData _staticData = default;
    [SerializeField] private SpellBehavior _healSpellBehavior = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    private StateManagerBehavior _stateManagerBehavior = default;
    private DemonBehavior _demonBehavior = default;
    private RangeAttackerBehavior _rangeAttackerBehavior = default;
    private AttackerBehavior _attackerBehavior = default;
    private AIAttackerBehavior _aiAttackerBehavior = default;
    private MovementBehavior _movementBehavior = default;
    private StatusOwnerBehavior _statusOwnerBehavior = default;

    public void Initialize()
    {
        _demonBehavior = GetComponent<DemonBehavior>();
        _demonBehavior.FeedData(_staticData.DemonStaticData);

        _stateManagerBehavior = GetComponent<StateManagerBehavior>();
        _stateManagerBehavior.FeedData(typeof(BlackMageState), BlackMageState.IDLING,
            CheckState, StopOldState, StartNewState);

        _rangeAttackerBehavior = GetComponent<RangeAttackerBehavior>();
        _rangeAttackerBehavior.FeedData(_staticData.RangeAttackerStaticData, IsTargetEnemy);

        _aiAttackerBehavior = GetComponent<AIAttackerBehavior>();
        _aiAttackerBehavior.FeedData(_staticData.AIAttackerStaticData);

        _movementBehavior = GetComponent<MovementBehavior>();
        _movementBehavior.FeedData(_staticData.MovementStaticData);
        _movementBehavior.StartMoving();

        _statusOwnerBehavior = GetComponent<StatusOwnerBehavior>();
        _attackerBehavior = GetComponent<AttackerBehavior>();

        _healSpellBehavior.GetComponent<HealSpellBehavior>().FeedData();
    }

    private void Start()
    {

    }

    private void CheckState()
    {
        var currentState = _stateManagerBehavior.State.EnumValue.To<BlackMageState>();
        switch (currentState)
        {
            case BlackMageState.IDLING:
                CheckIdlingState();
                break;
            case BlackMageState.MOVING:
                CheckMovingState();
                break;
            case BlackMageState.SHOOTING:
                CheckShootingState();
                break;
            case BlackMageState.PRE_STUNNING:
                CheckPreStunningState();
                break;
            case BlackMageState.STUNNING:
                CheckStunningState();
                break;
            case BlackMageState.POST_STUNNING:
                CheckPostStunningState();
                break;
            case BlackMageState.HEALING:
                CheckHealingState();
                break;
        }
    }


    private void StopOldState()
    {
        var oldState = _stateManagerBehavior.State.LastEnumValue.To<BlackMageState>();
        switch (oldState)
        {
            case BlackMageState.IDLING:
                break;
            case BlackMageState.MOVING:
                _movementBehavior.StopMoving();
                break;
            case BlackMageState.SHOOTING:
                _attackerBehavior.StopAttacking();
                break;
            case BlackMageState.PRE_STUNNING:
                _statusOwnerBehavior.StunStatusBehavior.StopPreStunning();
                break;
            case BlackMageState.STUNNING:
                _statusOwnerBehavior.StunStatusBehavior.StopStunning();
                break;
            case BlackMageState.POST_STUNNING:
                _statusOwnerBehavior.StunStatusBehavior.StopPostStunning();
                break;
            case BlackMageState.HEALING:
                break;
        }
    }

    private void StartNewState()
    {
        var newState = _stateManagerBehavior.State.EnumValue.To<BlackMageState>();
        switch (newState)
        {
            case BlackMageState.IDLING:
                break;
            case BlackMageState.MOVING:
                _movementBehavior.StartMoving();
                break;
            case BlackMageState.SHOOTING:
                _rangeAttackerBehavior.AttackTargetPosition = new Vector2(WallBehavior.Instance.transform.position.x,
                    _rangeAttackerBehavior.ProjectileSpawnLocation.position.y);
                _rangeAttackerBehavior.Aim();
                //_attackerBehavior.StartAttacking();
                break;
            case BlackMageState.PRE_STUNNING:
                _statusOwnerBehavior.StunStatusBehavior.StartPreStunning();
                break;
            case BlackMageState.STUNNING:
                _statusOwnerBehavior.StunStatusBehavior.StartStunning();
                break;
            case BlackMageState.POST_STUNNING:
                _statusOwnerBehavior.StunStatusBehavior.StartPostStunning();
                break;
            case BlackMageState.HEALING:
                //Will cast in animation.
                break;
        }
    }

    private void CheckIdlingState()
    {
        if(_demonBehavior.IsInAttackArea == false)
            _stateManagerBehavior.GoToTheNextState(BlackMageState.MOVING);

        if (_statusOwnerBehavior.StunStatusBehavior.IsAffected)
            _stateManagerBehavior.GoToTheNextState(BlackMageState.PRE_STUNNING);
        else if (_healSpellBehavior.State == SpellState.READY)
            _stateManagerBehavior.GoToTheNextState(BlackMageState.HEALING);
        else if (_aiAttackerBehavior.EnemiesAreInVision)
            _stateManagerBehavior.GoToTheNextState(BlackMageState.SHOOTING);
        else
            _stateManagerBehavior.GoToTheNextState(BlackMageState.MOVING);
    }

    private void CheckMovingState()
    {
        if (_demonBehavior.IsInAttackArea == false)
            return;

        if (_statusOwnerBehavior.StunStatusBehavior.IsAffected)
            _stateManagerBehavior.GoToTheNextState(BlackMageState.PRE_STUNNING);
        else if (_healSpellBehavior.State == SpellState.READY)
            _stateManagerBehavior.GoToTheNextState(BlackMageState.HEALING);
        else if (_aiAttackerBehavior.EnemiesAreInVision)
            _stateManagerBehavior.GoToTheNextState(BlackMageState.SHOOTING);
    }

    private void CheckShootingState()
    {
        if (_statusOwnerBehavior.StunStatusBehavior.IsAffected)
            _stateManagerBehavior.GoToTheNextState(BlackMageState.PRE_STUNNING);
        else if (_healSpellBehavior.State == SpellState.READY)
            _stateManagerBehavior.GoToTheNextState(BlackMageState.HEALING);
        else if (_aiAttackerBehavior.EnemiesAreInVision == false)
            _stateManagerBehavior.GoToTheNextState(BlackMageState.MOVING);
    }

    private void CheckPreStunningState()
    {
        _stateManagerBehavior.GoToTheNextState(BlackMageState.STUNNING);
    }

    private void CheckStunningState()
    {
        if (_statusOwnerBehavior.StunStatusBehavior.IsAffected == false)
            _stateManagerBehavior.GoToTheNextState(BlackMageState.POST_STUNNING);
    }

    private void CheckPostStunningState()
    {
        if (_statusOwnerBehavior.StunStatusBehavior.IsAffected)
            _stateManagerBehavior.GoToTheNextState(BlackMageState.PRE_STUNNING);
        else if (_healSpellBehavior.State == SpellState.READY)
            _stateManagerBehavior.GoToTheNextState(BlackMageState.HEALING);
        else if (_aiAttackerBehavior.EnemiesAreInVision)
            _stateManagerBehavior.GoToTheNextState(BlackMageState.SHOOTING);
        else
            _stateManagerBehavior.GoToTheNextState(BlackMageState.MOVING);
    }

    private void CheckHealingState()
    {
        if (_statusOwnerBehavior.StunStatusBehavior.IsAffected)
            _stateManagerBehavior.GoToTheNextState(BlackMageState.PRE_STUNNING);
        else if (_aiAttackerBehavior.EnemiesAreInVision)
            _stateManagerBehavior.GoToTheNextState(BlackMageState.SHOOTING);
        else
            _stateManagerBehavior.GoToTheNextState(BlackMageState.MOVING);
    }

    private void CastHealSpell() => _healSpellBehavior.Cast();

    private GameObject IsTargetEnemy(GameObject target)
    {
        var bodyAreaBehavior = target.GetComponent<BodyAreaBehavior>();
        if (bodyAreaBehavior != null)
        {
            target = bodyAreaBehavior.BodyBehavior.gameObject;
        }

        var castleBehavior = target.GetComponent<WallBehavior>();
        if (castleBehavior != null)
            return target;

        return null;
    }
}

