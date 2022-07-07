using Assets.Scripts.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDescriptionStaticData",
    menuName = "StaticData/Mission/LevelDescriptionStaticData", order = 1)]
public class LevelDescriptionStaticData : ScriptableObject
{
    public List<WaveDescription> WaveDescriptions = default;

    public int DemonCount => WaveDescriptions.Sum(ib => ib.DemonCount);
}
