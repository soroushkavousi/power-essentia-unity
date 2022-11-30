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
    [SerializeField] protected DiamondType _type = default;
    [SerializeField] protected Observable<DiamondState> _state = new(DiamondState.READY);
    [SerializeField] protected Observable<DiamondKnowledgeState> _knowledgeState = default;
    [SerializeField] protected Level _level = default;
    [SerializeField] protected List<ResourceBunch> _buyResourceBunches = default;
    [SerializeField] protected List<ResourceBunchWithLevel> _upgradeResourceBunches = default;
    private DiamondStaticData _diamondStaticData = default;
    protected DiamondOwnerBehavior _diamondOwnerBehavior = default;
    protected AttackerBehavior _ownerAttackerBehavior = default;
    protected Transform _diamondEffectsParent = default;

    public DiamondName Name => _name;
    public DiamondType Type => _type;
    public string ShowName => _diamondStaticData.ShowName;
    public Sprite Icon => _diamondStaticData.Icon;
    public List<ResourceBunch> BuyResourceBunches => _buyResourceBunches;
    public List<ResourceBunchWithLevel> UpgradeResourceBunches => _upgradeResourceBunches;
    public Observable<DiamondState> State => _state;
    public Observable<DiamondKnowledgeState> KnowledgeState => _knowledgeState;
    public Level Level => _level;
    public DiamondOwnerBehavior DiamondOwnerBehavior => _diamondOwnerBehavior;
    public AttackerBehavior OwnerAttackerBehavior => _ownerAttackerBehavior;
    public Func<GameObject, GameObject> IsTargetEnemyFunction { get; private set; }
    public Transform DiamondEffectsParent => _diamondEffectsParent;
    public string Description => GetDescription();
    public string StatsDescription => GetStatsDescription();

    public void FeedData(DiamondStaticData diamondStaticData, DiamondType type)
    {
        _diamondStaticData = diamondStaticData;
        _type = type;
    }

    public virtual void Initialize(Observable<DiamondKnowledgeState> knowledgeState,
        Level level, DiamondOwnerBehavior diamondOwnerBehavior)
    {
        _knowledgeState = knowledgeState;
        _level = level;
        _diamondOwnerBehavior = diamondOwnerBehavior;
        if (_diamondOwnerBehavior != null)
            _ownerAttackerBehavior = diamondOwnerBehavior.GetComponent<AttackerBehavior>();

        IsTargetEnemyFunction = diamondOwnerBehavior.IsTargetEnemy;
        InitializeBuyAndUpgradeResourceBunches();
        if (SceneManagerBehavior.Instance.CurrentSceneName.Value == SceneName.MISSION)
            _diamondEffectsParent = MissionManagerBehavior.Instance.DiamondEffectsParent;
        else
            _diamondEffectsParent = OutBoxBehavior.Instance.Location1;
    }

    private void InitializeBuyAndUpgradeResourceBunches()
    {
        _buyResourceBunches = new List<ResourceBunch>();
        foreach (var resourceBunchData in GameManagerBehavior.Instance.Settings.BuyDiamondResourceBunches)
        {
            _buyResourceBunches.Add(new ResourceBunch(resourceBunchData.Type, resourceBunchData.Amount));
        }

        _upgradeResourceBunches = new List<ResourceBunchWithLevel>();
        foreach (var resourceBunchWithLevelData in GameManagerBehavior.Instance.Settings.DiamondUpgradeResourceBunches)
        {
            var amount = new Number(_level, resourceBunchWithLevelData.AmountLevelInfo);
            _upgradeResourceBunches.Add(new ResourceBunchWithLevel(resourceBunchWithLevelData.Type, amount));
        }
    }

    public abstract void Activate();
    protected abstract void DoActivationWork();
    public abstract void Deactivate();
    protected abstract void DoDeactivationWork();
    protected abstract string GetDescription();
    protected abstract string GetStatsDescription();
}
