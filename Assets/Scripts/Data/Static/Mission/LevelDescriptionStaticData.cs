using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDescriptionStaticData", 
    menuName = "StaticData/Mission/LevelDescriptionStaticData", order = 1)]
public class LevelDescriptionStaticData : ScriptableObject
{
    [SerializeField] private int _number = default;
    [SerializeField] private List<WaveDescription> _waveDescriptions = default;

    public int Number => _number;
    public List<WaveDescription> WaveDescriptions => _waveDescriptions;
    public int DemonCount => _waveDescriptions.Sum(ib => ib.DemonCount);
}
