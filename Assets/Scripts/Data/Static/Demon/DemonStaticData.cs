using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DemonStaticData
{
    [SerializeField] private string _name = default;
    [SerializeField] private HealthStaticData _healthStaticData = default;
    [SerializeField] private float _healthPerLevel = default;

    public string Name => _name;
    public HealthStaticData HealthData => _healthStaticData;
    public float HealthPerLevel => _healthPerLevel;
}