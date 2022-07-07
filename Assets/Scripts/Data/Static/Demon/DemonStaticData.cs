using System;
using UnityEngine;

[Serializable]
public class DemonStaticData : ScriptableObject
{
    public string Name = default;
    public DemonHealthStaticData HealthData = default;
}