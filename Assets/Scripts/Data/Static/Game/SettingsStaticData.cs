using Assets.Scripts.Data;
using System;
using System.Collections.Generic;

[Serializable]
public class SettingsStaticData
{
    public int DiamondMaxLevel = default;
    public List<ResourceBunchStaticData> BuyDiamondResourceBunches = default;
    public List<ResourceBunchWithLevelStaticData> DiamondUpgradeResourceBunches = default;
}
