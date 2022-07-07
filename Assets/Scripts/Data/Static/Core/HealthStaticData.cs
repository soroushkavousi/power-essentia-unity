using System;
using UnityEngine;

[Serializable]
public class HealthStaticData
{
    public float Health = default;
    public float HealthLevelPercentage = default;

    public float PhysicalResistance = default;
    public float PhysicalResistanceLevelPercentage = default;

    public float MagicResistance = default;
    public float MagicResistanceLevelPercentage = default;

    public GameObject DeathVfxPrefab = default;
}