using Assets.Scripts.Enums;
using System;
using UnityEngine;

[Serializable]
public abstract class DiamondStaticData : ScriptableObject
{
    [Header("Diamon Data")]
    public DiamondName Name = default;
    public bool IsPermanent = default;
    public float ActiveTime = default;
    public float ActiveTimeLevelPercentage = default;
    public float CooldownTime = default;
    public float CooldownTimeLevelPercentage = default;
    public Sprite Icon = default;

    public string ShowName => Name.ToString().ToLower().CapitalizeFirstCharacter();
}
