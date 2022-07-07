using Assets.Scripts.Data;
using System;
using System.Collections.Generic;

[Serializable]
public class SettingsStaticData
{
    public List<ResourceBunchStaticData> BuyDiamondResourceBunches = default;
    public List<ResourceBunchWithLevelStaticData> DiamondUpgradeResourceBunches = default;
}
