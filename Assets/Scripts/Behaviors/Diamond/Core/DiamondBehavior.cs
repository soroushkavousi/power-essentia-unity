using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DiamondBehavior : MonoBehaviour
{
    [Header(Constants.HeaderStart + nameof(DiamondBehavior) + Constants.HeaderEnd)]
    [SerializeField] protected DiamondName _name = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] protected Observable<DiamondKnowledgeState> _knowledgeState = default;
    [SerializeField] protected Observable<int> _level = default;
    [SerializeField] protected Number _activeTime;
    [SerializeField] protected Number _cooldownTime;
    [SerializeField] protected List<ResourceBunch> _buyResourceBunches = default;
    [SerializeField] protected List<ResourceBunchWithLevel> _upgradeResourceBunches = default;
    [SerializeField] protected bool _isReady = default;
    [SerializeField] protected bool _onUsing = default;
    [SerializeField] protected bool _onCooldown = default;
    [SerializeField] protected float _ramainingTime = default;
    [SerializeField] protected float _ramainingPercentage = default;
    [SerializeField] protected bool _isPermanent = default;
    private DiamondStaticData _diamondStaticData = default;
    protected DiamondOwnerBehavior _diamondOwnerBehavior = default;
    protected AttackerBehavior _ownerAttackerBehavior = default;
    protected Transform _diamondEffectsParent = default;

    public DiamondName Name => _name;
    public string ShowName => _diamondStaticData.ShowName;
    public Sprite Icon => _diamondStaticData.Icon;
    public List<ResourceBunch> BuyResourceBunches => _buyResourceBunches;
    public List<ResourceBunchWithLevel> UpgradeResourceBunches => _upgradeResourceBunches;
    public Observable<DiamondKnowledgeState> KnowledgeState => _knowledgeState;
    public Observable<int> Level => _level;
    public Number ActiveTime => _activeTime;
    public Number CooldownTime => _cooldownTime;
    public bool IsReady => _isReady;
    public bool OnUsing => _onUsing;
    public bool OnCooldown => _onCooldown;
    public float RamainingTime => _ramainingTime;
    public float RamainingPercentage => _ramainingPercentage;
    public DiamondOwnerBehavior DiamondOwnerBehavior => _diamondOwnerBehavior;
    public AttackerBehavior OwnerAttackerBehavior => _ownerAttackerBehavior;
    public Func<GameObject, GameObject> IsTargetEnemyFunction { get; private set; }
    public Transform DiamondEffectsParent => _diamondEffectsParent;
    public string Description => GetDescription();
    public string StatsDescription => GetStatsDescription();

    public void FeedData(DiamondStaticData diamondStaticData)
    {
        _diamondStaticData = diamondStaticData;
    }

    public virtual void Initialize(Observable<DiamondKnowledgeState> knowledgeState,
        Observable<int> level, DiamondOwnerBehavior diamondOwnerBehavior)
    {
        _isReady = true;
        _knowledgeState = knowledgeState;
        _level = level;
        _diamondOwnerBehavior = diamondOwnerBehavior;
        if (_diamondOwnerBehavior != null)
            _ownerAttackerBehavior = diamondOwnerBehavior.GetComponent<AttackerBehavior>();

        _activeTime = new(_diamondStaticData.ActiveTime, level, _diamondStaticData.ActiveTimeLevelPercentage, min: 0f);
        _cooldownTime = new(_diamondStaticData.CooldownTime, level, _diamondStaticData.CooldownTimeLevelPercentage, min: 0f);

        IsTargetEnemyFunction = diamondOwnerBehavior.IsTargetEnemy;
        InitializeBuyAndUpgradeResourceBunches();
        if (SceneManagerBehavior.Instance.CurrentSceneName == SceneName.MISSION)
            _diamondEffectsParent = MissionManagerBehavior.Instance.DiamondEffectsParent;
        else
            _diamondEffectsParent = OutBoxBehavior.Instance.Location1;
    }

    private void InitializeBuyAndUpgradeResourceBunches()
    {
        _buyResourceBunches = new List<ResourceBunch>();
        foreach (var resourceBunchData in GameManagerBehavior.Instance.StaticData.Settings.BuyDiamondResourceBunches)
        {
            _buyResourceBunches.Add(new ResourceBunch(resourceBunchData.Type, resourceBunchData.Amount));
        }

        _upgradeResourceBunches = new List<ResourceBunchWithLevel>();
        foreach (var resourceBunchWithLevelData in GameManagerBehavior.Instance.StaticData.Settings.DiamondUpgradeResourceBunches)
        {
            var amount = new Number(resourceBunchWithLevelData.Amount, _level, resourceBunchWithLevelData.LevelPercentage);
            _upgradeResourceBunches.Add(new ResourceBunchWithLevel(resourceBunchWithLevelData.Type, amount));
        }
    }

    private void Update()
    {
        if (_isPermanent)
            return;

        if (_onUsing)
            UpdateRemainingTime(_activeTime.Value, Deactivate);
        else if (_onCooldown)
            UpdateRemainingTime(_cooldownTime.Value, GetReady);
    }

    public void Activate()
    {
        _isReady = false;
        DoActivationWork();
        _ramainingTime = _activeTime.Value;
        _onUsing = true;
    }
    protected abstract void DoActivationWork();

    public void Deactivate()
    {
        _onUsing = false;
        DoDeactivationWork();
        _ramainingTime = _cooldownTime.Value;
        _onCooldown = true;
    }
    protected abstract void DoDeactivationWork();

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

    protected abstract string GetDescription();
    protected abstract string GetStatsDescription();
}
