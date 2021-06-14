using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResistanceStaticData
{
    [SerializeField] private float _startPhysicalResistance = default;
    [SerializeField] private float _startMagicResistance = default;
    [SerializeField] private float _startStatusResistance = default;

    public float StartPhysicalResistance => _startPhysicalResistance;
    public float StartMagicResistance => _startMagicResistance;
    public float StartStatusResistance => _startStatusResistance;
}