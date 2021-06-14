using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "ArcherDiamondStaticData",
    menuName = "StaticData/Diamonds/Tools/ArcherDiamondStaticData", order = 1)]
public class ArcherDiamondStaticData : ScriptableObject
{
    [SerializeField] private DiamondStaticData _diamondStaticData = default;
    [SerializeField] private float _startInterval = default;
    [SerializeField] private float _intervalNegativeBasePerLevel = default;
    [SerializeField] private int _startDiamondCount = default;
    [SerializeField] private float _diamondCountBasePerlevel = default;
    [SerializeField] private float _startCooldownReduction = default;
    [SerializeField] private float _cooldownReductionBasePerLevel = default;

    public DiamondStaticData DiamondStaticData => _diamondStaticData;
    public float StartInterval => _startInterval;
    public float IntervalNegativeBasePerLevel => _intervalNegativeBasePerLevel;
    public int StartDiamondCount => _startDiamondCount;
    public float DiamondCountBasePerlevel => _diamondCountBasePerlevel;
    public float StartCooldownReduction => _startCooldownReduction;
    public float CooldownReductionBasePerLevel => _cooldownReductionBasePerLevel;
}
