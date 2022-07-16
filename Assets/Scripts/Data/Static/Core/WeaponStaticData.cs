using System;
using UnityEngine;

[Serializable]
public class WeaponStaticData : ScriptableObject
{
    [Space(Constants.DataSectionSpace)]
    [Header("Weapon Data")]
    public NumberLevelInfo DamageLevelInfo = default;
    public NumberLevelInfo SpeedLevelInfo = default;
    public NumberLevelInfo CriticalChanceLevelInfo = default;
    public NumberLevelInfo CriticalDamageLevelInfo = default;
}
