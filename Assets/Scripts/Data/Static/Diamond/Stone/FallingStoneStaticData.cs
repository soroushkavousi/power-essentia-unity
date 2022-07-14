using UnityEngine;

[CreateAssetMenu(fileName = "FallingStoneStaticData",
    menuName = "StaticData/Diamonds/Stone/FallingStoneStaticData", order = 1)]
public class FallingStoneStaticData : ScriptableObject
{
    public LevelInfo ImpactDamageLevelInfo = default;
    public LevelInfo StunDurationLevelInfo = default;
    public LevelInfo CriticalChanceLevelInfo = default;
    public LevelInfo CriticalDamageLevelInfo = default;
    public float SpawnXOffset = default;
    public float SpawnYPosition = default;
    public AudioClip HitSound = default;
    public MovementStaticData MovementStaticData = default;
}
