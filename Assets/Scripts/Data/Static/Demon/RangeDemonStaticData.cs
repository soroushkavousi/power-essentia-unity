using UnityEngine;

[CreateAssetMenu(fileName = "RangeDemonStaticData",
    menuName = "StaticData/Demons/RangeDemonStaticData", order = 1)]
public class RangeDemonStaticData : DemonStaticData
{
    public RangeAttackerStaticData RangeAttackerStaticData = default;
    public AIAttackerStaticData AIAttackerStaticData = default;
    public MovementStaticData MovementStaticData = default;
}