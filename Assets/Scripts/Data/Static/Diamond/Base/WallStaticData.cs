using UnityEngine;

[CreateAssetMenu(fileName = "WallStaticData",
    menuName = "StaticData/Diamonds/Base/WallStaticData", order = 1)]
public class WallStaticData : ScriptableObject
{
    public HealthStaticData HealthData = default;
}
