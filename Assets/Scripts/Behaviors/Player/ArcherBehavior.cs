using Assets.Scripts.Enums;
using UnityEngine;

[RequireComponent(typeof(RangeAttackerBehavior))]
public class ArcherBehavior : DiamondOwnerBehavior
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.HeaderStart + nameof(ArcherBehavior) + Constants.HeaderEnd)]
    [SerializeField] private ArcherStaticData _staticData2 = default;
    [SerializeField] private Transform _arrowLocation = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private BowDiamondBehavior _bowDiamondBehavior = default;
    private RangeAttackerBehavior _rangeAttackerBehavior = default;

    private void Awake()
    {
        Initialize(_staticData2);
        _stateManagerBehavior.FeedData(typeof(ArcherState), ArcherState.IDLING,
            CheckState, StopOldState, StartNewState);
        _bowDiamondBehavior = (BowDiamondBehavior)AllDiamondBehaviors[DiamondName.BOW];
        InitializeRangeAttackerBehavior();
    }

    private void InitializeRangeAttackerBehavior()
    {
        _rangeAttackerBehavior = GetComponent<RangeAttackerBehavior>();
        _rangeAttackerBehavior.FeedData(_staticData2.RangeAttackerData,
            _bowDiamondBehavior.BowWeaponBehavior, IsTargetEnemy);
    }

    private void CheckState()
    {
        var currentState = _stateManagerBehavior.State.Value.ToEnum<ArcherState>();
        switch (currentState)
        {
            case ArcherState.IDLING:
                CheckIdlingState();
                break;
            case ArcherState.AIMING:
                CheckAimingState();
                break;
            case ArcherState.SHOOTING:
                CheckShootingState();
                break;
        }
    }

    private void StopOldState()
    {
        var oldState = _stateManagerBehavior.State.LastValue.ToEnum<ArcherState>();
        var newState = _stateManagerBehavior.State.Value.ToEnum<ArcherState>();
        switch (oldState)
        {
            case ArcherState.IDLING:
                break;
            case ArcherState.AIMING:
                break;
            case ArcherState.SHOOTING:
                if (newState != ArcherState.AIMING)
                    _rangeAttackerBehavior.StopAttacking();
                break;
        }
    }

    private void StartNewState()
    {
        var newState = _stateManagerBehavior.State.Value.ToEnum<ArcherState>();
        switch (newState)
        {
            case ArcherState.IDLING:
                break;
            case ArcherState.AIMING:
                _rangeAttackerBehavior.Aim();
                break;
            case ArcherState.SHOOTING:
                _rangeAttackerBehavior.RangeWeaponBehavior.CreateProjectile(_arrowLocation);
                break;
        }
    }

    private void CheckIdlingState()
    {
        if (_mouseIsDown)
        {
            _rangeAttackerBehavior.AttackTargetPosition = GetAttackPosition();
            _stateManagerBehavior.GoToTheNextState(ArcherState.AIMING);
        }
    }

    private void CheckAimingState()
    {
        if (_rangeAttackerBehavior.AimingIsFinished)
            _stateManagerBehavior.GoToTheNextState(ArcherState.SHOOTING);
    }

    private void CheckShootingState()
    {
        if (_mouseIsDown)
        {
            _rangeAttackerBehavior.AttackTargetPosition = GetAttackPosition();
            _stateManagerBehavior.GoToTheNextState(ArcherState.AIMING);
        }
        else
            _stateManagerBehavior.GoToTheNextState(ArcherState.IDLING);
    }

    private Vector3 GetAttackPosition()
    {
        Vector3 attackPosition;
        if (Input.touchCount == 0)
            attackPosition = Input.mousePosition;
        else
            attackPosition = Input.GetTouch(0).position;

        attackPosition = Camera.main.ScreenToWorldPoint(attackPosition);
        return attackPosition;
    }
}
