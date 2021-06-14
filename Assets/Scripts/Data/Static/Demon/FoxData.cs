using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoxData",
    menuName = "StaticData/Demons/FoxData", order = 1)]
public class FoxData : ScriptableObject
{
    [Header("Base")]
    [SerializeField] private string _name = "Fox";

    [Header("Health")]
    [SerializeField] private int _health = 100;
    [SerializeField] private GameObject _deathVfx = default;

    [Header("Movement")]
    [SerializeField] private int _movementSpeed = 200;
    [SerializeField] private Vector2 _movementDirection = default;
    [SerializeField] private float _movementStartDelay = 1f;

    [Header("Attack")]
    [SerializeField] private int _attackDamage = 50;
    [SerializeField] private float _attackSpeed = 2f;

    [Header("Vision")]
    [SerializeField] private Vector2 _visionCenterPoint = default;
    [SerializeField] private Vector2 _visionSize = default;

    //[Header("Melee Attack")]

    public string Name { get => _name; }

    public int Health { get => _health; }
    public GameObject DeathVfx { get => _deathVfx; }

    public int MovementSpeed { get => _movementSpeed; }
    public Vector2 MovementDirection { get => _movementDirection; }
    public float MovementStartDelay { get => _movementStartDelay; }

    public int AttackDamage { get => _attackDamage; }
    public float AttackSpeed { get => _attackSpeed; }

    public Vector2 VisionCenterPoint { get => _visionCenterPoint; }
    public Vector2 VisionSize { get => _visionSize; }
}
