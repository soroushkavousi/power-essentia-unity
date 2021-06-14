using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "FireDiamondStaticData",
    menuName = "StaticData/Diamonds/Fire/FireDiamondStaticData", order = 1)]
public class FireDiamondStaticData : ScriptableObject
{
    [SerializeField] private DiamondStaticData _diamondStaticData = default;
    [SerializeField] private GroundFireBehavior _groundFireBehavior = default;
    [SerializeField] private float _startGroundFireChance = default;
    [SerializeField] private float _groundFireChancePerLevel = default;
    [SerializeField] private float _groundFireDurationPerLevel = default;
    [SerializeField] private float _groundFireDamagePerLevel = default;
    [SerializeField] private float _groundFireSlowPerLevel = default;
    [SerializeField] private float _groundFireCriticalChancePerLevel = default;
    [SerializeField] private float _groundFireCriticalDamagePerLevel = default;

    public DiamondStaticData DiamondStaticData => _diamondStaticData;
    public GroundFireBehavior GroundFireBehavior => _groundFireBehavior;
    public float StartGroundFireChance => _startGroundFireChance;
    public float GroundFireChancePerLevel => _groundFireChancePerLevel;
    public float GroundFireDurationPerLevel => _groundFireDurationPerLevel;
    public float GroundFireDamagePerLevel => _groundFireDamagePerLevel;
    public float GroundFireSlowPerLevel => _groundFireSlowPerLevel;
    public float GroundFireCriticalChancePerLevel => _groundFireCriticalChancePerLevel;
    public float GroundFireCriticalDamagePerLevel => _groundFireCriticalDamagePerLevel;
}
