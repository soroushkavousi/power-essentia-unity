using Assets.Scripts.Enums;
using System;
using UnityEngine;

public abstract class PeriodicDiamondBehavior : DiamondBehavior
{
    [SerializeField] protected Number _activeTime;
    [SerializeField] protected Number _cooldownTime;
    [SerializeField] protected bool _onCooldown = default;
    [SerializeField] protected float _ramainingTime = default;
    [SerializeField] protected float _ramainingPercentage = default;
    private PeriodicDiamondStaticData _periodicDiamondStaticData = default;

    public Number ActiveTime => _activeTime;
    public Number CooldownTime => _cooldownTime;
    public float RamainingTime => _ramainingTime;
    public float RamainingPercentage => _ramainingPercentage;

    public bool OnCooldown => _onCooldown;

    public void FeedData(PeriodicDiamondStaticData periodicDiamondStaticData)
    {
        _periodicDiamondStaticData = periodicDiamondStaticData;
        FeedData(_periodicDiamondStaticData, DiamondType.PERIODIC);
    }

    public override void Initialize(Observable<DiamondKnowledgeState> knowledgeState,
        Level level, DiamondOwnerBehavior diamondOwnerBehavior)
    {
        base.Initialize(knowledgeState, level, diamondOwnerBehavior);
        _activeTime = new(_level, _periodicDiamondStaticData.ActiveTimeLevelInfo, min: 0f);
        _cooldownTime = new(_level, _periodicDiamondStaticData.CooldownLevelInfo, min: 0f);
    }

    private void Update()
    {
        if (_onUsing)
            UpdateRemainingTime(_activeTime.Value, Deactivate);
        else if (_onCooldown)
            UpdateRemainingTime(_cooldownTime.Value, GetReady);
    }

    public override void Activate()
    {
        _isReady = false;
        DoActivationWork();
        _ramainingTime = _activeTime.Value;
        _onUsing = true;
    }

    public override void Deactivate()
    {
        _onUsing = false;
        DoDeactivationWork();
        _ramainingTime = _cooldownTime.Value;
        _onCooldown = true;
    }

    public void GetReady()
    {
        _onCooldown = false;
        _isReady = true;
    }

    private void UpdateRemainingTime(float maxTime, Action finishingAction)
    {
        _ramainingTime -= Time.deltaTime;
        if (_ramainingTime < 0)
            _ramainingTime = 0;
        _ramainingPercentage = _ramainingTime / maxTime * 100;
        if (_ramainingPercentage == 0)
        {
            finishingAction();
        }
    }
}
