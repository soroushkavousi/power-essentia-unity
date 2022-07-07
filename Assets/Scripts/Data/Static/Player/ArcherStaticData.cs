using UnityEngine;

[CreateAssetMenu(fileName = "ArcherData",
    menuName = "StaticData/Player/ArcherData", order = 1)]
public class ArcherStaticData : DiamondOwnerStaticData
{
    public RangeAttackerStaticData RangeAttackerData;
}
