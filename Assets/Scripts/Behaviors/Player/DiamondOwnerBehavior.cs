using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DiamondOwnerBehavior : MonoBehaviour
{
    [SerializeField] private Transform _toolsRing = default;
    [SerializeField] private Transform _leftRing = default;
    [SerializeField] private Transform _rightRing = default;
    [SerializeField] private Transform _box = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]
    private Dictionary<RingName, Transform> _ringMap = default;
    private Dictionary<DiamondName, DiamondDynamicData> _diamondDynamicDataMap = default;
    private Dictionary<RingName, List<AdvancedString>> _ringDiamondNamesMap = default;

    public Dictionary<DiamondName, DiamondBehavior> AllDiamondBehaviors { get; set; } = new Dictionary<DiamondName, DiamondBehavior>();
    public Dictionary<RingName, List<DiamondBehavior>> RingDiamondBehaviorsMap { get; set; } = new Dictionary<RingName, List<DiamondBehavior>>()
    {
        [RingName.TOOLS] = new List<DiamondBehavior>(),
        [RingName.LEFT] = new List<DiamondBehavior>(),
        [RingName.RIGHT] = new List<DiamondBehavior>(),
        [RingName.NONE] = new List<DiamondBehavior>(),
    };

    public void FeedData(Dictionary<DiamondName, DiamondDynamicData> diamondDynamicDataList,
        Dictionary<RingName, List<AdvancedString>> ringDiamondNamesMap)
    {
        _diamondDynamicDataMap = diamondDynamicDataList;
        _ringDiamondNamesMap = ringDiamondNamesMap;

        _ringMap = new Dictionary<RingName, Transform>
        {
            [RingName.TOOLS] = _toolsRing,
            [RingName.LEFT] = _leftRing,
            [RingName.RIGHT] = _rightRing,
            [RingName.NONE] = _box,
        };
        InitializeDiamondBehaviors();
    }

    private void InitializeDiamondBehaviors()
    {
        RingName ringName;
        DiamondName diamondName;
        foreach (var ringDiamondNames in _ringDiamondNamesMap)
        {
            ringName = ringDiamondNames.Key;
            foreach (var diamondNameString in ringDiamondNames.Value)
            {
                diamondName = diamondNameString.EnumValue.To<DiamondName>();
                CreateDiamond(ringName, diamondName);
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
        if (!PrefabContainerBehavior.Instance.DiamondPrefabs.ContainsKey(diamondName))
            return;
        DiamondBehavior diamondBehaviorPrefab = PrefabContainerBehavior.Instance.DiamondPrefabs[diamondName];
        var diamondDynamicData = _diamondDynamicDataMap[diamondName];
        DiamondBehavior diamondBehavior = Instantiate(diamondBehaviorPrefab, _ringMap[ringName]);
        diamondBehavior.Initialize(diamondDynamicData.IsDiscovered,
            diamondDynamicData.IsOwned, diamondDynamicData.Level, this);
        RingDiamondBehaviorsMap[ringName].Add(diamondBehavior);
        AllDiamondBehaviors.Add(diamondName, diamondBehavior);
        if (ringName == RingName.TOOLS)
            diamondBehavior.Activate();
    }
}
