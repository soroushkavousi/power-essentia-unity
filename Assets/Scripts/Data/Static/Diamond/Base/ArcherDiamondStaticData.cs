using UnityEngine;

[CreateAssetMenu(fileName = "ArcherDiamondStaticData",
    menuName = "StaticData/Diamonds/Base/ArcherDiamondStaticData", order = 1)]
public class ArcherDiamondStaticData : PermanentDiamondStaticData
{
    [Space(Constants.DataSectionSpace)]
    [Header("Archer Diamon Data")]
    public LevelInfo IntervalLevelInfo = default;
    public LevelInfo DiamondCountLevelInfo = default;
    public LevelInfo CooldownLevelInfo = default;
}
