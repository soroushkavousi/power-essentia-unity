using UnityEngine;

[CreateAssetMenu(fileName = "BowDiamondStaticData",
    menuName = "StaticData/Diamonds/Base/BowDiamondStaticData", order = 1)]
public class BowDiamondStaticData : PermanentDiamondStaticData
{
    //[Space(Constants.DataSectionSpace)]
    public RangeWeaponBehavior BowWeaponPrefab;
}
