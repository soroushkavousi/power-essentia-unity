using System;
using UnityEngine;

[Serializable]
public class HealthStaticData
{
    public NumberLevelInfo HealthLevelInfo = default;
    public NumberLevelInfo PhysicalResistanceLevelInfo = default;
    public NumberLevelInfo MagicResistanceLevelInfo = default;
    public GameObject DeathVfxPrefab = default;
}