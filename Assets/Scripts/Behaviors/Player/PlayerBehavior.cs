using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(StateManagerBehavior))]
[RequireComponent(typeof(BodyBehavior))]
[RequireComponent(typeof(AttackerBehavior))]
[RequireComponent(typeof(RangeAttackerBehavior))]
[RequireComponent(typeof(RotationBehavior))]
[RequireComponent(typeof(DiamondOwnerBehavior))]
public class PlayerBehavior : MonoBehaviour
{
    private static PlayerBehavior _main = default;
    [SerializeField] private PlayerData _staticData = default;
    [SerializeField] private bool _isMainPlayer = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    [SerializeField] private bool _mouseIsDown = default;

    [SerializeField] private PlayerDynamicData _dynamicData = default;
    private StateManagerBehavior _stateManagerBehavior = default;
    private BodyBehavior _bodyBehavior = default;
    private AttackerBehavior _attackerBehavior = default;
    private RangeAttackerBehavior _rangeAttackerBehavior = default;
    private RotationBehavior _rotationBehavior = default;
    private DiamondOwnerBehavior _diamondOwnerBehavior = default;

    public PlayerDynamicData DynamicData => _dynamicData;
    public bool IsMainPlayer => _isMainPlayer;
    public bool MouseIsDown { get => _mouseIsDown; set => _mouseIsDown = value; }
    public static PlayerBehavior Main => _main;
    public bool IsInitialized { get; }

    private void Awake()
    {
        if (_isMainPlayer)
            _main = this;

        GetDynamicData();
        _stateManagerBehavior = GetComponent<StateManagerBehavior>();
        _bodyBehavior = GetComponent<BodyBehavior>();
        _attackerBehavior = GetComponent<AttackerBehavior>();
        _rangeAttackerBehavior = GetComponent<RangeAttackerBehavior>();
        _rotationBehavior = GetComponent<RotationBehavior>();
        _diamondOwnerBehavior = GetComponent<DiamondOwnerBehavior>();

        _stateManagerBehavior.FeedData(typeof(PlayerState), PlayerState.IDLING,
            CheckState, StopOldState, StartNewPlayerState);
        _bodyBehavior.FeedData();
        _rangeAttackerBehavior.FeedData(_staticData.RangeAttackerData, IsTargetEnemy);
        _diamondOwnerBehavior.FeedData(_dynamicData.Diamonds,
            _dynamicData.SelectedItems.RingDiamondNamesMap);
    }

    private void Start()
    {
        if (SceneManagerBehavior.Instance.CurrentSceneName != SceneName.MISSION)
            gameObject.SetActive(false);
    }

    private void GetDynamicData()
    {
        if(IsMainPlayer)
            _dynamicData = PlayerDynamicDataTO.Instance.PlayerDynamicData;
    }

    private GameObject IsTargetEnemy(GameObject target)
    {
        var bodyAreaBehavior = target.GetComponent<BodyAreaBehavior>();
        if (bodyAreaBehavior != null)
        {
            target = bodyAreaBehavior.BodyBehavior.gameObject;
        }

        var invaderBehavior = target.GetComponent<DemonBehavior>();
        if (invaderBehavior != null)
            return target;

        return null;
    }

    private void CheckState()
    {
        var currentState = _stateManagerBehavior.State.EnumValue.To<PlayerState>();
        switch (currentState)
        {
            case PlayerState.IDLING:
                CheckIdlingState();
                break;
            case PlayerState.AIMING:
                CheckAimingState();
                break;
            case PlayerState.SHOOTING:
                CheckShootingState();
                break;
        }
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

    private void CheckIdlingState()
    {
        if (_mouseIsDown)
        {
            _rangeAttackerBehavior.AttackTargetPosition = GetAttackPosition();
            _stateManagerBehavior.GoToTheNextState(PlayerState.AIMING);
        }
    }

    private void CheckAimingState()
    {
        if (_rangeAttackerBehavior.AimingIsFinished)
            _stateManagerBehavior.GoToTheNextState(PlayerState.SHOOTING);
    }

    private void CheckShootingState()
    {
        if (_mouseIsDown)
        {
            _rangeAttackerBehavior.AttackTargetPosition = GetAttackPosition();
            _stateManagerBehavior.GoToTheNextState(PlayerState.AIMING);
        }
        else
            _stateManagerBehavior.GoToTheNextState(PlayerState.IDLING);
    }

    private void StopOldState()
    {
        var oldState = _stateManagerBehavior.State.LastEnumValue.To<PlayerState>();
        var newState = _stateManagerBehavior.State.EnumValue.To<PlayerState>();
        switch (oldState)
        {
            case PlayerState.IDLING:
                break;
            case PlayerState.AIMING:
                break;
            case PlayerState.SHOOTING:
                if(newState != PlayerState.AIMING)
                    _attackerBehavior.StopAttacking();
                break;
        }
    }

    private void StartNewPlayerState()
    {
        var newState = _stateManagerBehavior.State.EnumValue.To<PlayerState>();
        switch (newState)
        {
            case PlayerState.IDLING:
                break;
            case PlayerState.AIMING:
                _rangeAttackerBehavior.Aim();
                break;
            case PlayerState.SHOOTING:
                break;
        }
    }
}
