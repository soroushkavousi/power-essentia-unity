using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelManagerStaticData", 
    menuName = "StaticData/Mission/LevelManagerStaticData", order = 1)]
public class LevelManagerStaticData : ScriptableObject
{
    [SerializeField] private float _waveTime = default;

    public float WaveTime => _waveTime;
}
