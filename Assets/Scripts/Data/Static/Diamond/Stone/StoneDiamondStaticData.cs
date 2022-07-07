using UnityEngine;

[CreateAssetMenu(fileName = "StoneDiamondStaticData",
    menuName = "StaticData/Diamonds/Stone/StoneDiamondStaticData", order = 1)]
public class StoneDiamondStaticData : DiamondStaticData
{
    [Space(Constants.DataSectionSpace)]
    [Header("Stone Diamon Data")]
    public FallingStoneBehavior FallingStoneBehavior = default;

    [Space(Constants.DataSectionSpace)]
    public float Chance = default;
    public float ChanceLevelPercentage = default;
}
