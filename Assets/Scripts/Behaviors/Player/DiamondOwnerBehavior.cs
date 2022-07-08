using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;

public abstract class DiamondOwnerBehavior : PlayerBehavior
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.HeaderStart + nameof(DiamondOwnerBehavior) + Constants.HeaderEnd)]
    [SerializeField] private Transform _toolsRingSlot = default;
    [SerializeField] private Transform _leftRingSlot = default;
    [SerializeField] private Transform _rightRingSlot = default;
    [SerializeField] private Transform _boxSlot = default;

    //[Space(Constants.DebugSectionSpace)]
    //[Header(Constants.DebugSectionHeader)]
    private DiamondOwnerStaticData _staticData = default;
    private Dictionary<RingName, Transform> _ringTransformMap = default;
    private Dictionary<DiamondName, DiamondDynamicData> _diamondDynamicDataMap = default;
    private Dictionary<RingName, List<Observable<DiamondName>>> _chosenRings = default;

    public static DiamondOwnerBehavior MainDiamondOwner => MainPlayer.To<DiamondOwnerBehavior>();
    public Dictionary<DiamondName, DiamondBehavior> AllDiamondBehaviors { get; set; } = new Dictionary<DiamondName, DiamondBehavior>();
    public Dictionary<RingName, List<DiamondBehavior>> RingDiamondBehaviorsMap { get; set; } = new Dictionary<RingName, List<DiamondBehavior>>()
    {
        [RingName.TOOLS] = new List<DiamondBehavior>(),
        [RingName.LEFT] = new List<DiamondBehavior>(),
        [RingName.RIGHT] = new List<DiamondBehavior>(),
        [RingName.NONE] = new List<DiamondBehavior>(),
    };

    protected void FeedData(DiamondOwnerStaticData staticData)
    {
        _staticData = staticData;
        base.FeedData(_staticData);
        _diamondDynamicDataMap = _dynamicData.Diamonds;
        _chosenRings = _dynamicData.SelectedItems.RingDiamondNamesMap;

        _ringTransformMap = new Dictionary<RingName, Transform>
        {
            [RingName.TOOLS] = _toolsRingSlot,
            [RingName.LEFT] = _leftRingSlot,
            [RingName.RIGHT] = _rightRingSlot,
            [RingName.NONE] = _boxSlot,
        };
        InitializeDiamondBehaviors();
    }

    private void InitializeDiamondBehaviors()
    {
        RingName ringName;
        foreach (var ringDiamondNames in _chosenRings)
        {
            ringName = ringDiamondNames.Key;
            foreach (var diamondName in ringDiamondNames.Value)
            {
                CreateDiamond(ringName, diamondName.Value);
            }
        }

        if (SceneManagerBehavior.Instance.CurrentSceneName == SceneName.MISSION)
            return;

        foreach (var otherDiamondName in _diamondDynamicDataMap.Keys)
        {
            if (AllDiamondBehaviors.ContainsKey(otherDiamondName))
                continue;
            CreateDiamond(RingName.NONE, otherDiamondName);
        }
    }

    private void CreateDiamond(RingName ringName, DiamondName diamondName)
    {
        if (diamondName == DiamondName.NONE)
        {
            RingDiamondBehaviorsMap[ringName].Add(default);
            return;
        }

        if (!GameManagerBehavior.Instance.StaticData.Prefabs.DiamondPrefabs
                .TryGetValue(diamondName, out DiamondBehavior diamondBehaviorPrefab))
            return;
        DiamondBehavior diamondBehavior = Instantiate(diamondBehaviorPrefab, _ringTransformMap[ringName]);

        var diamondDynamicData = _diamondDynamicDataMap[diamondName];
        diamondBehavior.Initialize(diamondDynamicData.KnowledgeState, diamondDynamicData.Level, this);
        RingDiamondBehaviorsMap[ringName].Add(diamondBehavior);
        AllDiamondBehaviors.Add(diamondName, diamondBehavior);
        if (ringName == RingName.TOOLS)
            diamondBehavior.Activate();
    }
}
