using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "WallDiamondStaticData",
    menuName = "StaticData/Diamonds/Tools/WallDiamondStaticData", order = 1)]
public class WallDiamondStaticData : ScriptableObject
{
    [SerializeField] private DiamondStaticData _diamondStaticData = default;
    [SerializeField] private WallBehavior _wallBehaviorPrefab = default;
    [SerializeField] private Vector2 _locationInBattleField= default;
    [SerializeField] private float _healthBasePerLevel = default;
    [SerializeField] private float _physicalResistanceBasePerLevel = default;

    public DiamondStaticData DiamondStaticData => _diamondStaticData;
    public WallBehavior WallBehaviorPrefab => _wallBehaviorPrefab;
    public Vector2 LocationInBattleField => _locationInBattleField;
    public float HealthBasePerLevel => _healthBasePerLevel;
    public float PhysicalResistanceBasePerLevel => _physicalResistanceBasePerLevel;
}
