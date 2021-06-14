using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "StoneDiamondStaticData",
    menuName = "StaticData/Diamonds/Stone/StoneDiamondStaticData", order = 1)]
public class StoneDiamondStaticData : ScriptableObject
{
    [SerializeField] private DiamondStaticData _diamondStaticData = default;
    [SerializeField] private FallingStoneBehavior _fallingStoneBehavior = default;
    [SerializeField] private float _startFallingStoneChance = default;
    [SerializeField] private float _fallingStoneChancePerLevel = default;
    [SerializeField] private float _fallingStoneImpactDamagePerLevel = default;
    [SerializeField] private float _fallingStoneStunDurationPerLevel = default;
    [SerializeField] private float _fallingStoneCriticalChancePerLevel = default;
    [SerializeField] private float _fallingStoneCriticalDamagePerLevel = default;

    public DiamondStaticData DiamondStaticData => _diamondStaticData;
    public FallingStoneBehavior FallingStoneBehavior => _fallingStoneBehavior;
    public float StartFallingStoneChance => _startFallingStoneChance;
    public float FallingStoneChancePerLevel => _fallingStoneChancePerLevel;
    public float FallingStoneImpactDamagePerLevel => _fallingStoneImpactDamagePerLevel;
    public float FallingStoneStunDurationPerLevel => _fallingStoneStunDurationPerLevel;
    public float FallingStoneCriticalChancePerLevel => _fallingStoneCriticalChancePerLevel;
    public float FallingStoneCriticalDamagePerLevel => _fallingStoneCriticalDamagePerLevel;
}
