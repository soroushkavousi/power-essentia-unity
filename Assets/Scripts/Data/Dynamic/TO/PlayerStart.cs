using Assets.Scripts.Enums;
using System.Collections.Generic;

public static class PlayerStart
{
    public static PlayerDynamicDataTO Data { get; set; } = new PlayerDynamicDataTO
    {
        PatchNumber = 31,
        PlayerSetName = PlayerSetName.ARCHER.ToString(),
        DemonLevel = 1,
        SelectedItems = new SelectedItemsDynamicDataTO
        (
            demonLevel: 1,
            leftRingDiamondNames: new List<string>
            {
                DiamondName.NONE.ToString(),
                DiamondName.NONE.ToString(),
                DiamondName.NONE.ToString(),
                DiamondName.NONE.ToString(),
            },
            rightRingDiamondNames: new List<string>
            {
                DiamondName.FIRE.ToString(),
                DiamondName.NONE.ToString(),
                DiamondName.NONE.ToString(),
                DiamondName.NONE.ToString(),
            },
            toolsRingDiamondNames: new List<string>
            {
                DiamondName.ARCHER.ToString(),
                DiamondName.BOW.ToString(),
                DiamondName.WALL.ToString(),
                DiamondName.BLOOD.ToString(),
            },
            menuDeckDiamondName: DiamondName.FIRE.ToString(),
            menuToolsDiamondName: DiamondName.ARCHER.ToString()
        ),
        Diamonds = new List<DiamondDynamicDataTO>
        {
            new DiamondDynamicDataTO(DiamondName.ARCHER.ToString(), DiamondKnowledgeState.OWNED.ToString(), 1),
            new DiamondDynamicDataTO(DiamondName.BOW.ToString(), DiamondKnowledgeState.OWNED.ToString(),  1),
            new DiamondDynamicDataTO(DiamondName.WALL.ToString(), DiamondKnowledgeState.OWNED.ToString(),  1),
            new DiamondDynamicDataTO(DiamondName.BLOOD.ToString(), DiamondKnowledgeState.OWNED.ToString(),  1),
            new DiamondDynamicDataTO(DiamondName.FIRE.ToString(), DiamondKnowledgeState.OWNED.ToString(),  1),
            new DiamondDynamicDataTO(DiamondName.STONE.ToString(), DiamondKnowledgeState.DISCOVERED.ToString(),  1),
            //new DiamondDynamicDataTO(DiamondName.POISON.ToString(), true, false,  1),
            //new DiamondDynamicDataTO(DiamondName.ELECTRICITY.ToString(), true, false,  1),
            //new DiamondDynamicDataTO(DiamondName.ICE.ToString(), true, false,  1),
            //new DiamondDynamicDataTO(DiamondName.HERO.ToString(), true, false,  1),
            //new DiamondDynamicDataTO(DiamondName.WEAPON.ToString(), true, false,  1),
        },
        ResourceBunches = new List<ResourceBunchDynamicDataTO>
        {
            new ResourceBunchDynamicDataTO(ResourceType.COIN.ToString(), 8000),
            new ResourceBunchDynamicDataTO(ResourceType.DEMON_BLOOD.ToString(), 80),
            new ResourceBunchDynamicDataTO(ResourceType.DARK_DEMON_BLOOD.ToString(), 400),
        },
    };
}
