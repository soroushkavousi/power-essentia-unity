using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HealthStaticData
{
    [SerializeField] private float _startHealth = default;
    [SerializeField] private GameObject _deathVfxPrefab = default;
    [SerializeField] private ResistanceStaticData _resistanceStaticData = default;

    public float StartHealth => _startHealth;
    public GameObject DeathVfxPrefab => _deathVfxPrefab;
    public ResistanceStaticData ResistanceStaticData => _resistanceStaticData;
}