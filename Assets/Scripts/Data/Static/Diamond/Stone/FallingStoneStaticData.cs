using UnityEngine;

[CreateAssetMenu(fileName = "FallingStoneStaticData",
    menuName = "StaticData/Diamonds/Stone/FallingStoneStaticData", order = 1)]
public class FallingStoneStaticData : ScriptableObject
{
    [Space(Constants.DataSectionSpace)]
    public float ImpactDamage = default;
    public float ImpactDamageLevelPercentage = default;

    [Space(Constants.DataSectionSpace)]
    public float StunDuration = default;
    public float StunDurationLevelPercentage = default;

    [Space(Constants.DataSectionSpace)]
    public float CriticalChance = default;
    public float CriticalChanceLevelPercentage = default;

    [Space(Constants.DataSectionSpace)]
    public float CriticalDamage = default;
    public float CriticalDamageLevelPercentage = default;

    [Space(Constants.DataSectionSpace)]
    public float SpawnXOffset = default;
    public float SpawnYPosition = default;
    public AudioClip HitSound = default;
    public MovementStaticData MovementStaticData = default;
}
