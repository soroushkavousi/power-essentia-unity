using UnityEngine;

[CreateAssetMenu(fileName = "StoneDiamondStaticData",
    menuName = "StaticData/Diamonds/Stone/StoneDiamondStaticData", order = 1)]
public class StoneDiamondStaticData : PeriodicDiamondStaticData
{
    [Space(Constants.DataSectionSpace)]
    [Header("Stone Diamon Data")]
    public FallingStoneBehavior FallingStoneBehavior = default;
    public LevelInfo ChanceLevelInfo = default;
}
