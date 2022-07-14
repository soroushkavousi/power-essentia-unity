using Assets.Scripts.Enums;
using System;
using UnityEngine;

[Serializable]
public abstract class DiamondStaticData : ScriptableObject
{
    [Header("Diamon Data")]
    public DiamondName Name = default;
    public Sprite Icon = default;

    public string ShowName => Name.ToString().ToLower().CapitalizeFirstCharacter();
}
