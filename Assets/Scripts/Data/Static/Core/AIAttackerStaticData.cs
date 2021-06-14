using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AIAttackerStaticData
{
    [SerializeField] private VisionStaticData _visionStaticData = default;

    public VisionStaticData VisionStaticData => _visionStaticData;
}