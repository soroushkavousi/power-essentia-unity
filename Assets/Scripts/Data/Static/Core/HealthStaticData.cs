using System;
using UnityEngine;

[Serializable]
public class HealthStaticData
{
    public LevelInfo HealthLevelInfo = default;
    public LevelInfo PhysicalResistanceLevelInfo = default;
    public LevelInfo MagicResistanceLevelInfo = default;
    public GameObject DeathVfxPrefab = default;
}