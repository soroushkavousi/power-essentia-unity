using System;
using UnityEngine;

[Serializable]
public class WeaponStaticData : ScriptableObject
{
    [Space(Constants.DataSectionSpace)]
    [Header("Weapon Data")]
    public LevelInfo DamageLevelInfo = default;
    public LevelInfo SpeedLevelInfo = default;
    public LevelInfo CriticalChanceLevelInfo = default;
    public LevelInfo CriticalDamageLevelInfo = default;
}
