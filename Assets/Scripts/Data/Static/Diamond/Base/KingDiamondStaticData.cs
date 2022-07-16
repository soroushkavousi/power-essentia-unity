using UnityEngine;

[CreateAssetMenu(fileName = "KingDiamondStaticData",
    menuName = "StaticData/Diamonds/Base/KingDiamondStaticData", order = 1)]
public class KingDiamondStaticData : PermanentDiamondStaticData
{
    [Space(Constants.DataSectionSpace)]
    public NumberLevelInfo GoldBoost = default;
}
