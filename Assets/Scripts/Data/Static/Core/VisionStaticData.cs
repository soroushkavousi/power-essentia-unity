using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VisionStaticData
{
    [SerializeField] private Vector2 _centerPoint = default;
    [SerializeField] private Vector2 _size = default;

    public Vector2 CenterPoint => _centerPoint;
    public Vector2 Size => _size;
}