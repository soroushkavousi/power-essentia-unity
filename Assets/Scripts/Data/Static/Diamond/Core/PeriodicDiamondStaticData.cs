using System;
using UnityEngine;

[Serializable]
public abstract class PeriodicDiamondStaticData : DiamondStaticData
{
    [Space(Constants.DataSectionSpace)]
    [Header("Periodic Diamon Data")]

    public NumberLevelInfo ActiveTimeLevelInfo = default;
    public NumberLevelInfo CooldownLevelInfo = default;
}
