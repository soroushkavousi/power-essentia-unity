using System;
using UnityEngine;

[Serializable]
public class SpellStaticData : ScriptableObject
{
    public string Name = default;
    public float Cooldown = default;
    public float CooldownLevelPercentage = default;
}