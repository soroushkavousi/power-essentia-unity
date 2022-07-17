using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DemonHealthBehavior : HealthBehavior
{
    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]
    [SerializeField] private List<ResourceBunchWithLevel> _deathRewards = default;
    private DemonHealthStaticData _staticData = default;
    [NonSerialized] private Level _level = default;

    public List<ResourceBunchWithLevel> DeathRewards => _deathRewards;

    public void FeedData(DemonHealthStaticData staticData, Level level)
    {
        _staticData = staticData;
        _level = level;
        base.FeedData(_staticData, level);

        _deathRewards = new List<ResourceBunchWithLevel>();
        foreach (var deathRewardData in _staticData.DeathRewardsData)
        {
            var resourceAmount = new Number(_level, deathRewardData.AmountLevelInfo);
            _deathRewards.Add(new ResourceBunchWithLevel(deathRewardData.Type, resourceAmount));
        }
    }
}
