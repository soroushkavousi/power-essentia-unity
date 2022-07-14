using UnityEngine;

[CreateAssetMenu(fileName = "FireDiamondStaticData",
    menuName = "StaticData/Diamonds/Fire/FireDiamondStaticData", order = 1)]
public class FireDiamondStaticData : PeriodicDiamondStaticData
{
    [Space(Constants.DataSectionSpace)]
    [Header("Fire Diamon Data")]
    public GroundFireBehavior GroundFireBehavior = default;
    public LevelInfo ChanceLevelInfo = default;
}
