using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelResourceSystemBehavior : MonoBehaviour
{
    private static LevelResourceSystemBehavior _instance = default;

    [Space(Constants.DebugSectionSpace, order = -1001)]
    [Header(Constants.DebugSectionHeader, order = -1000)]

    [SerializeField] private ResourceBox _resourceBox = default;
    private ResourceBox _gameResourceBox = default;
    private Action<NumberChangeCommand> _temp = default;

    public static LevelResourceSystemBehavior Instance => Utils.GetInstance(ref _instance);
    public ResourceBox ResourceBox => _resourceBox;

    public void FeedData()
    {
        _resourceBox = new ResourceBox
        (
            new List<ResourceBunch>
            {
                new ResourceBunch(ResourceType.COIN, 0),
                new ResourceBunch(ResourceType.DEMON_BLOOD, 0),
                new ResourceBunch(ResourceType.DARK_DEMON_BLOOD, 0),
            }
        );

        _gameResourceBox = PlayerBehavior.Main.DynamicData.ResourceBox;
        _temp = (cc) => AddNewResourcesToResourceBox(cc, ResourceType.COIN);
        _gameResourceBox.ResourceBunches[ResourceType.COIN].OnNewValueActions.Add(_temp);
    }

    private void AddNewResourcesToResourceBox(NumberChangeCommand numberChange, ResourceType resourceType)
    {
        _resourceBox.ResourceBunches[resourceType].Change(numberChange.Amount.IntValue, name);
    }
}
