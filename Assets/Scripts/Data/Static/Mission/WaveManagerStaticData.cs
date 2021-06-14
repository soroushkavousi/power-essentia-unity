using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveManagerStaticData",
    menuName = "StaticData/Mission/WaveManagerStaticData", order = 1)]
public class WaveManagerStaticData : ScriptableObject
{
    [SerializeField] private int _columnDistance = default;

    public int ColumnDistance => _columnDistance;
}
