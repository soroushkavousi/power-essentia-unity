using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System.Linq;

public static class TestPlayer
{
    public static void ApplyData(PlayerDynamicData data)
    {
        data.Achievements.DemonLevel.Value = 6;
        data.SelectedItems.DemonLevel.Value = 7;

        var fireDiamond = data.Diamonds[DiamondName.FIRE];
        fireDiamond.KnowledgeState.Value = DiamondKnowledgeState.OWNED;
        data.SelectedItems.RingDiamondNamesMap[RingName.RIGHT][1].Value = DiamondName.FIRE;
        fireDiamond.Level.Value = 7;

        var stoneDiamond = data.Diamonds[DiamondName.STONE];
        stoneDiamond.Level.Value = 7;

        var bowDiamond = data.Diamonds[DiamondName.BOW];
        bowDiamond.Level.Value = 7;

        var coinResourceData = data.ResourceBunches.Single(r => r.Type == ResourceType.COIN);
        coinResourceData.Amount.Value = 10000000;

        var demonBloodResourceData = data.ResourceBunches.Single(r => r.Type == ResourceType.DEMON_BLOOD);
        demonBloodResourceData.Amount.Value = 1000000;
    }
}
