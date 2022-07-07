using UnityEngine;

[CreateAssetMenu(fileName = "ArcherDiamondStaticData",
    menuName = "StaticData/Diamonds/Tools/ArcherDiamondStaticData", order = 1)]
public class ArcherDiamondStaticData : DiamondStaticData
{
    [Space(Constants.DataSectionSpace)]
    [Header("Archer Diamon Data")]
    public float StartInterval = default;
    public float IntervalNegativeBasePerLevel = default;

    [Space(Constants.DataSectionSpace)]
    public int StartDiamondCount = default;
    public float DiamondCountBasePerlevel = default;

    [Space(Constants.DataSectionSpace)]
    public float StartCooldownReduction = default;
    public float CooldownReductionBasePerLevel = default;
}
