using UnityEngine;

[CreateAssetMenu(fileName = "BloodDiamondStaticData",
    menuName = "StaticData/Diamonds/Base/BloodDiamondStaticData", order = 1)]
public class BloodDiamondStaticData : PermanentDiamondStaticData
{
    [Space(Constants.DataSectionSpace)]
    public LevelInfo BloodRatioLevelInfo = default;
}
