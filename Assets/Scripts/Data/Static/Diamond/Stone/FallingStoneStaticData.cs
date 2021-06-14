using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "FallingStoneStaticData",
    menuName = "StaticData/Diamonds/Stone/FallingStoneStaticData", order = 1)]
public class FallingStoneStaticData : ScriptableObject
{
    [SerializeField] private float _startImpactDamage = default;
    [SerializeField] private float _startStunDuration = default;
    [SerializeField] private float _startCriticalChance = default;
    [SerializeField] private float _startCriticalDamage = default;
    [SerializeField] private float _spawnXOffset = default;
    [SerializeField] private float _spawnYPosition = default;
    [SerializeField] private AudioClip _hitSound = default;
    [SerializeField] private MovementStaticData _movementStaticData = default;

    public float StartImpactDamage => _startImpactDamage;
    public float StartStunDuration => _startStunDuration;
    public float StartCriticalChance => _startCriticalChance;
    public float StartCriticalDamage => _startCriticalDamage;
    public float SpawnXOffset => _spawnXOffset;
    public float SpawnYPosition => _spawnYPosition;
    public AudioClip HitSound => _hitSound;
    public MovementStaticData MovementStaticData => _movementStaticData;
}
