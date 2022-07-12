using Assets.Scripts.Data;
using System;
using System.Collections.Generic;

[Serializable]
public class SettingsStaticData
{
    public int DiamondMaxLevel = 10;
    public float AcceptedCollisionDistance = 30f;
    public float MaxAttackSpeed = 30f;
    public List<ResourceBunchStaticData> BuyDiamondResourceBunches = default;
    public List<ResourceBunchWithLevelStaticData> DiamondUpgradeResourceBunches = default;
}
