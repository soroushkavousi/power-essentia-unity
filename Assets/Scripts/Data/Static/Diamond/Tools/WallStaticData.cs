using UnityEngine;

[CreateAssetMenu(fileName = "WallStaticData",
    menuName = "StaticData/Diamonds/Tools/WallStaticData", order = 1)]
public class WallStaticData : ScriptableObject
{
    public HealthStaticData HealthData = default;
}
