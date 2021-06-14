using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class TestPlayer
{
    public static void ApplyData(PlayerDynamicDataTO data)
    {
        //data.Diamonds
        //    .Single(d => d.Name == DiamondName.FIRE).Level = 10;
        //data.Stats.AttackDamageLevel = 10;
        //data.Stats.AttackSpeedLevel = 10;

        //if (data.Stats.Level == 1)
        //    data.Stats.Level = 10;

        //data.Mission = 1;
        //if (data.Mission > 2)
        //    data.Mission = 1;

        //var fireDiamond = data.Diamonds.Single(d => d.Name == DiamondName.FIRE.ToString());
        //if (fireDiamond.Level == 1)
        //    fireDiamond.Level = 12;

        //var stoneDiamond = data.Diamonds.Single(d => d.Name == DiamondName.STONE.ToString());
        //if (stoneDiamond.Level == 1)
        //    stoneDiamond.Level = 12;

        var coinData = data.ResourceBox.ResourceBunches.Single(r => r.Type == ResourceType.COIN.ToString());
        if (coinData.Amount == 0)
            coinData.Amount = 100000;

        var demonBloodData = data.ResourceBox.ResourceBunches.Single(r => r.Type == ResourceType.DEMON_BLOOD.ToString());
        if (demonBloodData.Amount == 0)
            demonBloodData.Amount = 1000;

        data.SelectedItems.DemonLevel = 1;
        //data.SelectedItems.DeckItemDiamondNames[1] = DiamondName.BOW.ToString();
    }
}
