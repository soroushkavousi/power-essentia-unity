using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "GroundFireStaticData",
    menuName = "StaticData/Diamonds/Fire/GroundFireStaticData", order = 1)]
public class GroundFireStaticData : ScriptableObject
{
    [SerializeField] private float _startDuration = default;
    [SerializeField] private float _startDamage = default;
    [SerializeField] private float _startCriticalChance = default;
    [SerializeField] private float _startCriticalDamage = default;
    [SerializeField] private float _startSlow = default;
    [SerializeField] private Vector2 _spawnOffset = default;

    public float StartDuration => _startDuration;
    public float StartDamage => _startDamage;
    public float StartCriticalChance => _startCriticalChance;
    public float StartCriticalDamage => _startCriticalDamage;
    public float StartSlow => _startSlow;
    public Vector2 SpawnOffset => _spawnOffset;
}
