using System;
using UnityEngine;

[Serializable]
public class WeaponStaticData : ScriptableObject
{
    [Space(Constants.DataSectionSpace)]
    [Header("Weapon Data")]
    public float Damage = default;
    public float DamageLevelPercentage = default;

    public float Speed = default;
    public float SpeedLevelPercentage = default;

    public float CriticalChance = default;
    public float CriticalChanceLevelPercentage = default;

    public float CriticalDamage = default;
    public float CriticalDamageLevelPercentage = default;
}
