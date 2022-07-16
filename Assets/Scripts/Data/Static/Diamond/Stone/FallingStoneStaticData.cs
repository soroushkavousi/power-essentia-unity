using UnityEngine;

[CreateAssetMenu(fileName = "FallingStoneStaticData",
    menuName = "StaticData/Diamonds/Stone/FallingStoneStaticData", order = 1)]
public class FallingStoneStaticData : ScriptableObject
{
    public NumberLevelInfo ImpactDamageLevelInfo = default;
    public NumberLevelInfo StunDurationLevelInfo = default;
    public NumberLevelInfo CriticalChanceLevelInfo = default;
    public NumberLevelInfo CriticalDamageLevelInfo = default;
    public float SpawnXOffset = default;
    public float SpawnYPosition = default;
    public AudioClip HitSound = default;
    public MovementStaticData MovementStaticData = default;
}
