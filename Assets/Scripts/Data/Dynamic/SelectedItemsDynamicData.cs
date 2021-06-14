using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class SelectedItemsDynamicData
    {
        [SerializeField] private OnePartAdvancedNumber _demonLevel = new OnePartAdvancedNumber();

        public OnePartAdvancedNumber DemonLevel => _demonLevel;
        public Dictionary<RingName, List<AdvancedString>> RingDiamondNamesMap { get; set; } = new Dictionary<RingName, List<AdvancedString>>
        {
            [RingName.TOOLS] = new List<AdvancedString>
            {
                new AdvancedString(),
                new AdvancedString(),
                new AdvancedString(),
                new AdvancedString(),
            },
            [RingName.LEFT] = new List<AdvancedString>
            {
                new AdvancedString(),
                new AdvancedString(),
                new AdvancedString(),
                new AdvancedString(),
            },
            [RingName.RIGHT] = new List<AdvancedString>
            {
                new AdvancedString(),
                new AdvancedString(),
                new AdvancedString(),
                new AdvancedString(),
            },
        };
        public Dictionary<RingName, AdvancedString> MenuDiamondName { get; set; } = new Dictionary<RingName, AdvancedString>()
        {
            [RingName.DECK] = new AdvancedString(),
            [RingName.TOOLS] = new AdvancedString(),
        };

        private SelectedItemsDynamicData() { }

        public SelectedItemsDynamicData(int demonLevel, List<DiamondName> toolsRingDiamondNames, 
            List<DiamondName> leftRingDiamondNames, List<DiamondName> righRingDiamondNames, 
            DiamondName menuDeckDiamondName, DiamondName menuToolsDiamondName)
        {
            _demonLevel.FeedData(demonLevel);
            MenuDiamondName[RingName.LEFT] = MenuDiamondName[RingName.RIGHT] = MenuDiamondName[RingName.DECK];

            for (int i = 0; i < RingDiamondNamesMap[RingName.TOOLS].Count; i++)
            {
                RingDiamondNamesMap[RingName.TOOLS][i].FeedData(toolsRingDiamondNames[i]);
            }

            for (int i = 0; i < RingDiamondNamesMap[RingName.LEFT].Count; i++)
            {
                RingDiamondNamesMap[RingName.LEFT][i].FeedData(leftRingDiamondNames[i]);
            }
            RingDiamondNamesMap[RingName.LEFT][0].OnNewValueActions.Add(cc => RemoveSameDiamondFromDeck(cc, RingName.LEFT, 0));
            RingDiamondNamesMap[RingName.LEFT][1].OnNewValueActions.Add(cc => RemoveSameDiamondFromDeck(cc, RingName.LEFT, 1));
            RingDiamondNamesMap[RingName.LEFT][2].OnNewValueActions.Add(cc => RemoveSameDiamondFromDeck(cc, RingName.LEFT, 2));
            RingDiamondNamesMap[RingName.LEFT][3].OnNewValueActions.Add(cc => RemoveSameDiamondFromDeck(cc, RingName.LEFT, 3));

            for (int i = 0; i < RingDiamondNamesMap[RingName.RIGHT].Count; i++)
            {
                RingDiamondNamesMap[RingName.RIGHT][i].FeedData(righRingDiamondNames[i]);
            }
            RingDiamondNamesMap[RingName.RIGHT][0].OnNewValueActions.Add(cc => RemoveSameDiamondFromDeck(cc, RingName.RIGHT, 0));
            RingDiamondNamesMap[RingName.RIGHT][1].OnNewValueActions.Add(cc => RemoveSameDiamondFromDeck(cc, RingName.RIGHT, 1));
            RingDiamondNamesMap[RingName.RIGHT][2].OnNewValueActions.Add(cc => RemoveSameDiamondFromDeck(cc, RingName.RIGHT, 2));
            RingDiamondNamesMap[RingName.RIGHT][3].OnNewValueActions.Add(cc => RemoveSameDiamondFromDeck(cc, RingName.RIGHT, 3));

            MenuDiamondName[RingName.DECK].FeedData(menuDeckDiamondName);
            MenuDiamondName[RingName.TOOLS].FeedData(menuToolsDiamondName);
        }

        private void RemoveSameDiamondFromDeck(StringChangeCommand cc, RingName ringName,
            int index)
        {
            var targetName = RingDiamondNamesMap[ringName][index].Value;
            if (targetName == DiamondName.NONE.ToString())
                return;
            foreach (var currentRingName in new List<RingName> { RingName.LEFT, RingName.RIGHT })
            {
                for (int i = 0; i < RingDiamondNamesMap[currentRingName].Count; i++)
                {
                    if (ringName == currentRingName && i == index)
                        continue;
                    if (RingDiamondNamesMap[currentRingName][i].Value == targetName)
                        RingDiamondNamesMap[currentRingName][i].Change(DiamondName.NONE, nameof(SelectedItemsDynamicData));
                }
            }
        }
    }
}
