using Assets.Scripts.Models;
using System.Linq;

public static class TestPlayer
{
    public static void ApplyData(PlayerDynamicData data)
    {
        data.Achievements.DemonLevel.Value = 9;
        data.SelectedItems.DemonLevel.Value = 10;

        var coinResourceData = data.ResourceBunches.Single(r => r.Type == ResourceType.COIN);
        coinResourceData.Amount.Value = 10000000;

        var demonBloodResourceData = data.ResourceBunches.Single(r => r.Type == ResourceType.DEMON_BLOOD);
        demonBloodResourceData.Amount.Value = 1000000;
    }
}
