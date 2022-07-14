using Assets.Scripts.Enums;
using Assets.Scripts.Models;
using System.Linq;

public static class TestPlayer
{
    public static void ApplyData(PlayerDynamicData data)
    {
        SetResources(data);
        //SetDemonLevels(data);
        //SetDiamondLevels(data);
    }

    private static void SetResources(PlayerDynamicData data)
    {
        var coinResourceData = data.ResourceBunches.Single(r => r.Type == ResourceType.COIN);
        coinResourceData.Amount.Value = 999999;

        var demonBloodResourceData = data.ResourceBunches.Single(r => r.Type == ResourceType.DEMON_BLOOD);
        demonBloodResourceData.Amount.Value = 99999;
    }

    private static void SetDemonLevels(PlayerDynamicData data)
    {
        var demonLevel = 40;
        data.Achievements.DemonLevel.Value = demonLevel - 1;
        data.SelectedItems.DemonLevel.Value = demonLevel;
    }

    private static void SetDiamondLevels(PlayerDynamicData data)
    {
        var diamondLevel = 10;

        var fireDiamond = data.Diamonds[DiamondName.FIRE];
        fireDiamond.KnowledgeState.Value = DiamondKnowledgeState.OWNED;
        data.SelectedItems.RingDiamondNamesMap[RingName.RIGHT][1].Value = DiamondName.FIRE;
        fireDiamond.Level.Value = diamondLevel;

        var stoneDiamond = data.Diamonds[DiamondName.STONE];
        stoneDiamond.Level.Value = diamondLevel;

        var bowDiamond = data.Diamonds[DiamondName.BOW];
        bowDiamond.Level.Value = diamondLevel;

        var wallDiamond = data.Diamonds[DiamondName.WALL];
        wallDiamond.Level.Value = diamondLevel;
    }
}
