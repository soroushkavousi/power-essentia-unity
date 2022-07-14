using UnityEngine;

[CreateAssetMenu(fileName = "GroundFireStaticData",
    menuName = "StaticData/Diamonds/Fire/GroundFireStaticData", order = 1)]
public class GroundFireStaticData : ScriptableObject
{
    public LevelInfo DurationLevelInfo = default;
    public LevelInfo DamageLevelInfo = default;
    public LevelInfo CriticalChanceLevelInfo = default;
    public LevelInfo CriticalDamageLevelInfo = default;
    public LevelInfo SlowLevelInfo = default;
    public Vector2 SpawnOffset = default;
}
