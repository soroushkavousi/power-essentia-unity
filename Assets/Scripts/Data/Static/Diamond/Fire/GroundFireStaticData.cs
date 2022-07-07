using UnityEngine;

[CreateAssetMenu(fileName = "GroundFireStaticData",
    menuName = "StaticData/Diamonds/Fire/GroundFireStaticData", order = 1)]
public class GroundFireStaticData : ScriptableObject
{
    [Space(Constants.DataSectionSpace)]
    public float Duration = default;
    public float DurationLevelPercentage = default;

    [Space(Constants.DataSectionSpace)]
    public float Damage = default;
    public float DamageLevePercentage = default;

    [Space(Constants.DataSectionSpace)]
    public float CriticalChance = default;
    public float CriticalChanceLevelPercentage = default;

    [Space(Constants.DataSectionSpace)]
    public float CriticalDamage = default;
    public float CriticalDamageLevelPercentage = default;

    [Space(Constants.DataSectionSpace)]
    public float Slow = default;
    public float SlowLevelPercentage = default;

    [Space(Constants.DataSectionSpace)]
    public Vector2 SpawnOffset = default;
}
