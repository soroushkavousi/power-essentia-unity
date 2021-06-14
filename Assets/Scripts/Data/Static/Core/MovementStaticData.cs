using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MovementStaticData
{
    [SerializeField] private float _speed = default;
    [SerializeField] private Vector2 _startDirection = default;
    [SerializeField] private float _startDelay = default;
    [SerializeField] private float _animationSpeed = default;
    [SerializeField] private bool _dontStopWhenReach = default;

    public float Speed => _speed;
    public Vector2 StartDirection => _startDirection;
    public float StartDelay => _startDelay;
    public float AnimationSpeed => _animationSpeed;
    public bool DontStopWhenReach => _dontStopWhenReach;

    public MovementStaticData(float speed, Vector2 startDirection, 
        float startDelay, float animationSpeed, bool dontStopWhenReach)
    {
        _speed = speed;
        _startDirection = startDirection;
        _startDelay = startDelay;
        _animationSpeed = animationSpeed;
        _dontStopWhenReach = dontStopWhenReach;
    }
}