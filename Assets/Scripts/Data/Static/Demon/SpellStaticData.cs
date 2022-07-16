using System;
using UnityEngine;

[Serializable]
public class SpellStaticData : ScriptableObject
{
    public string Name = default;
    public NumberLevelInfo CooldownLevelInfo = default;
}