using System;
using UnityEngine;

[Serializable]
public abstract class PeriodicDiamondStaticData : DiamondStaticData
{
    [Space(Constants.DataSectionSpace)]
    [Header("Periodic Diamon Data")]

    public LevelInfo ActiveTimeLevelInfo = default;
    public LevelInfo CooldownLevelInfo = default;
}
