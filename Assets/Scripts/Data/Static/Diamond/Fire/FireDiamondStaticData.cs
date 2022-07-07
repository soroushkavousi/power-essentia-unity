using UnityEngine;

[CreateAssetMenu(fileName = "FireDiamondStaticData",
    menuName = "StaticData/Diamonds/Fire/FireDiamondStaticData", order = 1)]
public class FireDiamondStaticData : DiamondStaticData
{
    [Space(Constants.DataSectionSpace)]
    [Header("Fire Diamon Data")]
    public GroundFireBehavior GroundFireBehavior = default;

    [Space(Constants.DataSectionSpace)]
    public float Chance = default;
    public float ChanceLevelPercentage = default;
}
