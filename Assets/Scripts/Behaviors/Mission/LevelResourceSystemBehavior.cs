using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System.Collections.Generic;
using UnityEngine;

public class LevelResourceSystemBehavior : MonoBehaviour, IObserver<DemonBehavior>, IObserver
{
    private static LevelResourceSystemBehavior _instance = default;

    [Space(Constants.SpaceSection)]
    [Header(Constants.DebugSectionHeader)]

    [SerializeField] private List<ResourceBunch> _resourceBunches = default;
    [SerializeField] private BloodDiamondBehavior _bloodDiamondBehavior = default;
    private List<ResourceBunch> _gameResourceBunches = default;

    public static LevelResourceSystemBehavior Instance => Utils.GetInstance(ref _instance);
    public List<ResourceBunch> ResourceBox => _resourceBunches;

    public void FeedData()
    {
        _resourceBunches = new List<ResourceBunch>()
            {
                new ResourceBunch(ResourceType.COIN, 0),
                new ResourceBunch(ResourceType.DEMON_BLOOD, 0),
                new ResourceBunch(ResourceType.DARK_DEMON_BLOOD, 0),
            };

        _gameResourceBunches = PlayerBehavior.MainPlayer.DynamicData.ResourceBunches;
        WaveManagerBehavior.Instance.Attach(this);
        _bloodDiamondBehavior = (BloodDiamondBehavior)DiamondOwnerBehavior.MainDiamondOwner.AllDiamondBehaviors[DiamondName.BLOOD];
    }

    private void AddResourceForDeadDemon(DemonBehavior demonBehavior)
    {
        foreach (var deathReward in demonBehavior.HealthBehavior.DeathRewards)
        {
            var resourceType = deathReward.Type;
            var resourceAmount = deathReward.Amount.Value;
            if (deathReward.Type == ResourceType.DEMON_BLOOD)
                resourceAmount = resourceAmount.MeasurePercentage(_bloodDiamondBehavior.BloodRatio.Value);
            var resourceAmountAsLong = resourceAmount.ToLong();
            _gameResourceBunches.Find(rb => rb.Type == resourceType).Amount.Value += resourceAmountAsLong;
            _resourceBunches.Find(rb => rb.Type == resourceType).Amount.Value += resourceAmountAsLong;
        }
    }

    public void OnNotify(ISubject<DemonBehavior> subject, DemonBehavior demonBehavior)
    {
        if (ReferenceEquals(subject, WaveManagerBehavior.Instance))
        {
            demonBehavior.Attach(this);
        }
    }

    public void OnNotify(ISubject subject)
    {
        if (subject is DemonBehavior demonBehavior)
        {
            if (demonBehavior.State == DemonState.DEAD)
            {
                AddResourceForDeadDemon(demonBehavior);
            }
        }
    }
}
