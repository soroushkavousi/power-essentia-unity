using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "WallStaticData", 
    menuName = "StaticData/Diamonds/Tools/WallStaticData", order = 1)]
public class WallStaticData : ScriptableObject
{
    [SerializeField] private HealthStaticData _healthData = default;

    public HealthStaticData HealthData => _healthData;
}
