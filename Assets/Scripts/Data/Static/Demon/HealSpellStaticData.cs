using UnityEngine;

[CreateAssetMenu(fileName = "HealSpellData",
    menuName = "StaticData/Demons/HealSpellData", order = 1)]
public class HealSpellStaticData : SpellStaticData
{
    public LevelInfo HealAmountLevelInfo = default;
    public float MaxHealAreaRadius = default;
    public float HealAreaGrowTime = default;
}