using UnityEngine;

[CreateAssetMenu(fileName = "BloodDiamondStaticData",
    menuName = "StaticData/Diamonds/Base/BloodDiamondStaticData", order = 1)]
public class BloodDiamondStaticData : DiamondStaticData
{
    [Space(Constants.DataSectionSpace)]
    public float BloodRatio = default;
    public float BloodRatioLevelPercentage = default;
}
