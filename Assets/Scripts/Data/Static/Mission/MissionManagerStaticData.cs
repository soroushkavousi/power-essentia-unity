using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionManagerStaticData", 
    menuName = "StaticData/Mission/MissionManagerStaticData", order = 1)]
public class MissionManagerStaticData : ScriptableObject
{
    [SerializeField] private Vector2 _missionAreaMaxPosition = default;
    [SerializeField] private List<LevelDescriptionStaticData> _levelDescriptions = default;

    public Vector2 MissionAreaMaxPosition => _missionAreaMaxPosition;
    public List<LevelDescriptionStaticData> Levels => _levelDescriptions;
}
