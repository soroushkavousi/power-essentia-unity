using UnityEngine;

[CreateAssetMenu(fileName = "HealSpellData",
    menuName = "StaticData/Demons/HealSpellData", order = 1)]
public class HealSpellStaticData : SpellStaticData
{
    public NumberLevelInfo HealAmountLevelInfo = default;
    public float MaxHealAreaRadius = default;
    public float HealAreaGrowTime = default;
}