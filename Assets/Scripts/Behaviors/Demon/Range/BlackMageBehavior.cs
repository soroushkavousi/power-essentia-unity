using Assets.Scripts.Enums;
using UnityEngine;

[RequireComponent(typeof(StateManagerBehavior))]
[RequireComponent(typeof(MovementBehavior))]
[RequireComponent(typeof(RangeAttackerBehavior))]
[RequireComponent(typeof(AIAttackerBehavior))]
public class BlackMageBehavior : DemonBehavior
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.HeaderStart + nameof(BlackMageBehavior) + Constants.HeaderEnd)]
    [SerializeField] private RangeDemonStaticData _staticData = default;
    [SerializeField] private RangeWeaponBehavior _rangeWeaponBehavior = default;
    [SerializeField] private HealSpellBehavior _healSpellBehavior = default;
    [SerializeField] private Transform _fireLocation = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    protected StateManagerBehavior _stateManagerBehavior = default;
    protected AIAttackerBehavior _aiAttackerBehavior = default;
    protected RangeAttackerBehavior _rangeAttackerBehavior = default;
    protected MovementBehavior _movementBehavior = default;

    public override void Initialize(int level)
    {
        base.Initialize(level);
        base.FeedData(_staticData);

        _stateManagerBehavior = GetComponent<StateManagerBehavior>();
        _stateManagerBehavior.FeedData(typeof(BlackMageState), BlackMageState.IDLING,
            CheckState, StopOldState, StartNewState);

        _rangeWeaponBehavior.Initialize(_level, IsTargetEnemy);
        _rangeAttackerBehavior = GetComponent<RangeAttackerBehavior>();
        _rangeAttackerBehavior.FeedData(_staticData.RangeAttackerStaticData,
            _rangeWeaponBehavior, IsTargetEnemy);

        _aiAttackerBehavior = GetComponent<AIAttackerBehavior>();
        _aiAttackerBehavior.FeedData(_staticData.AIAttackerStaticData);

        _movementBehavior = GetComponent<MovementBehavior>();
        _movementBehavior.FeedData(_staticData.MovementStaticData, _level);
        _movementBehavior.MoveWithDirection(Vector2.left);

        _healSpellBehavior.Initialize(_level);
    }

    private void CheckState()
    {
        var currentState = _stateManagerBehavior.State.Value.ToEnum<BlackMageState>();
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
        var oldState = _stateManagerBehavior.State.LastValue.ToEnum<BlackMageState>();
        switch (oldState)
        {
            case BlackMageState.IDLING:
                break;
            case BlackMageState.MOVING:
                _movementBehavior.StopMoving();
                break;
            case BlackMageState.SHOOTING:
                _rangeAttackerBehavior.StopAttacking();
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
        var newState = _stateManagerBehavior.State.Value.ToEnum<BlackMageState>();
        switch (newState)
        {
            case BlackMageState.IDLING:
                break;
            case BlackMageState.MOVING:
                _movementBehavior.MoveWithDirection(Vector2.left);
                break;
            case BlackMageState.SHOOTING:
                _rangeAttackerBehavior.AttackTargetPosition = new Vector2(
                    WallBehavior.Instance.transform.position.x,
                    _rangeAttackerBehavior.ProjectileShotLocation.position.y);
                _rangeAttackerBehavior.Aim();
                _rangeAttackerBehavior.RangeWeaponBehavior.CreateProjectile(_fireLocation);
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
        if (IsInAttackArea == false)
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
        if (IsInAttackArea == false)
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
}

