using UnityEngine;

[CreateAssetMenu(fileName = "GroundFireStaticData",
    menuName = "StaticData/Diamonds/Fire/GroundFireStaticData", order = 1)]
public class GroundFireStaticData : ScriptableObject
{
    public NumberLevelInfo DurationLevelInfo = default;
    public NumberLevelInfo DamageLevelInfo = default;
    public NumberLevelInfo CriticalChanceLevelInfo = default;
    public NumberLevelInfo CriticalDamageLevelInfo = default;
    public NumberLevelInfo SlowLevelInfo = default;
    public Vector2 SpawnOffset = default;
}
