using Assets.Scripts.Data;
using System;
using System.Collections.Generic;

[Serializable]
public class DemonHealthStaticData : HealthStaticData
{
    public List<ResourceBunchWithLevelStaticData> DeathRewardsData = default;
}