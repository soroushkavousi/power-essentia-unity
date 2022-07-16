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
    [SerializeField] private KingDiamondBehavior _kingDiamondBehavior = default;
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

        var allDiamondBehaviors = DiamondOwnerBehavior.MainDiamondOwner.AllDiamondBehaviors;
        _kingDiamondBehavior = (KingDiamondBehavior)allDiamondBehaviors[DiamondName.KING];
        _bloodDiamondBehavior = (BloodDiamondBehavior)allDiamondBehaviors[DiamondName.BLOOD];

        LevelManagerBehavior.Instance.Finished.Attach(this);
    }

    private void AddResourceForDeadDemon(DemonBehavior demonBehavior)
    {
        foreach (var deathReward in demonBehavior.HealthBehavior.DeathRewards)
        {
            var resourceType = deathReward.Type;
            var resourceAmount = deathReward.Amount.Value;
            switch (resourceType)
            {
                case ResourceType.COIN:
                    resourceAmount = resourceAmount.AddPercentage(_kingDiamondBehavior.GoldBoost.Value);
                    break;
                case ResourceType.DEMON_BLOOD:
                    resourceAmount = resourceAmount.MeasurePercentage(_bloodDiamondBehavior.BloodRatio.Value);
                    break;
            }
            var resourceAmountAsLong = resourceAmount.ToLong();
            _gameResourceBunches.Find(rb => rb.Type == resourceType).Amount.Value += resourceAmountAsLong;
            _resourceBunches.Find(rb => rb.Type == resourceType).Amount.Value += resourceAmountAsLong;
        }
    }

    private void AddDarkDemonBloodForWinning()
    {
        _gameResourceBunches.Find(rb => rb.Type == ResourceType.DARK_DEMON_BLOOD).Amount.Value += 1;
        _resourceBunches.Find(rb => rb.Type == ResourceType.DARK_DEMON_BLOOD).Amount.Value += 1;
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
        else if (subject == LevelManagerBehavior.Instance.Finished)
        {
            if (LevelManagerBehavior.Instance.Finished.Value == true)
            {
                AddDarkDemonBloodForWinning();
            }
        }
    }
}
