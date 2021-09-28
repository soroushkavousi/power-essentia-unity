using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackerStaticData
{
    [SerializeField] private float _startDamage = default;
    [SerializeField] private float _startSpeed = default;
    [SerializeField] private float _startCriticalChance = default;
    [SerializeField] private float _startCriticalDamage = default;
    [SerializeField] private AudioClip _attackSound = default;

    public float StartDamage => _startDamage;
    public float StartSpeed => _startSpeed;
    public float StartCriticalChance => _startCriticalChance;
    public float StartCriticalDamage => _startCriticalDamage;
    public AudioClip AttackSound => _attackSound;
}