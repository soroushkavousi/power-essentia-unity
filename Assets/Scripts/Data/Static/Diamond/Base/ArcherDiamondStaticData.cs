using UnityEngine;

[CreateAssetMenu(fileName = "ArcherDiamondStaticData",
    menuName = "StaticData/Diamonds/Base/ArcherDiamondStaticData", order = 1)]
public class ArcherDiamondStaticData : PermanentDiamondStaticData
{
    [Space(Constants.DataSectionSpace)]
    [Header("Archer Diamon Data")]
    public NumberLevelInfo IntervalLevelInfo = default;
    public NumberLevelInfo DiamondCountLevelInfo = default;
    public NumberLevelInfo CooldownLevelInfo = default;
}
