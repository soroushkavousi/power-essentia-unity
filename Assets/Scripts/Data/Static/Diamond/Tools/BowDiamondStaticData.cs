using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "BowDiamondStaticData",
    menuName = "StaticData/Diamonds/Tools/BowDiamondStaticData", order = 1)]
public class BowDiamondStaticData : ScriptableObject
{
    [SerializeField] private DiamondStaticData _diamondStaticData = default;
    [SerializeField] private float _damageBasePerLevel = default;
    [SerializeField] private float _fireRateBasePerLevel = default;
    [SerializeField] private float _criticalChanceBasePerLevel = default;
    [SerializeField] private float _criticalDamageBasePerLevel = default;

    public DiamondStaticData DiamondStaticData => _diamondStaticData;
    public float DamageBasePerLevel => _damageBasePerLevel;
    public float FireRateBasePerLevel => _fireRateBasePerLevel;
    public float CriticalChanceBasePerLevel => _criticalChanceBasePerLevel;
    public float CriticalDamageBasePerLevel => _criticalDamageBasePerLevel;
}
