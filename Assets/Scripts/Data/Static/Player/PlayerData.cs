using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData",
    menuName = "StaticData/Defends/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [SerializeField] private RangeAttackerStaticData _rangeAttackerData = default;
    [SerializeField] private int _maxLevel = default;
    [SerializeField] private int _initialUpgradeCoinCost = default;
    [SerializeField] private int _upgradeCoinCostPerLevel = default;

    public RangeAttackerStaticData RangeAttackerData => _rangeAttackerData;
    public int MaxLevel => _maxLevel;

    public int GetUpgradeCoinCostForLevel(int level)
    {
        return _initialUpgradeCoinCost
            + level * _upgradeCoinCostPerLevel;
    }
}
