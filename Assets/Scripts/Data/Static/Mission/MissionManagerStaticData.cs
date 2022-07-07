using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionManagerStaticData",
    menuName = "StaticData/Mission/MissionManagerStaticData", order = 1)]
public class MissionManagerStaticData : ScriptableObject
{
    public List<LevelDescriptionStaticData> LevelDescriptions = default;
}
