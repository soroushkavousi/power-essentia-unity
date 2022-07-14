using UnityEngine;

[CreateAssetMenu(fileName = "WallDiamondStaticData",
    menuName = "StaticData/Diamonds/Base/WallDiamondStaticData", order = 1)]
public class WallDiamondStaticData : PermanentDiamondStaticData
{
    [Space(Constants.DataSectionSpace)]
    [Header("Wall Diamon Data")]
    public WallBehavior WallBehaviorPrefab = default;
}
