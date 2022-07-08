using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class SelectedItemsDynamicData : IObserver
    {
        public Observable<int> DemonLevel = new();

        public Dictionary<RingName, List<Observable<DiamondName>>> RingDiamondNamesMap { get; set; } = new Dictionary<RingName, List<Observable<DiamondName>>>
        {
            [RingName.TOOLS] = new List<Observable<DiamondName>>
            {
                new(),
                new(),
                new(),
            },
            [RingName.LEFT] = new List<Observable<DiamondName>>
            {
                new(),
                new(),
                new(),
                new(),
            },
            [RingName.RIGHT] = new List<Observable<DiamondName>>
            {
                new(),
                new(),
                new(),
                new(),
            },
        };
        public Dictionary<RingName, Observable<DiamondName>> MenuDiamondName { get; set; } = new Dictionary<RingName, Observable<DiamondName>>()
        {
            [RingName.DECK] = new Observable<DiamondName>(),
            [RingName.TOOLS] = new Observable<DiamondName>(),
        };

        private SelectedItemsDynamicData() { }

        public SelectedItemsDynamicData(int demonLevel, List<DiamondName> toolsRingDiamondNames,
            List<DiamondName> leftRingDiamondNames, List<DiamondName> righRingDiamondNames,
            DiamondName menuDeckDiamondName, DiamondName menuToolsDiamondName)
        {
            DemonLevel.Value = demonLevel;
            MenuDiamondName[RingName.LEFT] = MenuDiamondName[RingName.RIGHT] = MenuDiamondName[RingName.DECK];

            for (int i = 0; i < RingDiamondNamesMap[RingName.TOOLS].Count; i++)
            {
                RingDiamondNamesMap[RingName.TOOLS][i].Value = toolsRingDiamondNames[i];
            }

            for (int i = 0; i < RingDiamondNamesMap[RingName.LEFT].Count; i++)
            {
                RingDiamondNamesMap[RingName.LEFT][i].Value = leftRingDiamondNames[i];
                RingDiamondNamesMap[RingName.LEFT][i].Attach(this);
            }

            for (int i = 0; i < RingDiamondNamesMap[RingName.RIGHT].Count; i++)
            {
                RingDiamondNamesMap[RingName.RIGHT][i].Value = righRingDiamondNames[i];
                RingDiamondNamesMap[RingName.RIGHT][i].Attach(this);
            }

            MenuDiamondName[RingName.DECK].Value = menuDeckDiamondName;
            MenuDiamondName[RingName.TOOLS].Value = menuToolsDiamondName;
        }

        private void RemoveSameDiamondFromDeck(RingName ringName, int index)
        {
            var targetName = RingDiamondNamesMap[ringName][index].Value;
            foreach (var currentRingName in new List<RingName> { RingName.LEFT, RingName.RIGHT })
            {
                for (int i = 0; i < RingDiamondNamesMap[currentRingName].Count; i++)
                {
                    if (ringName == currentRingName && i == index)
                        continue;
                    if (RingDiamondNamesMap[currentRingName][i].Value == targetName)
                        RingDiamondNamesMap[currentRingName][i].Value = DiamondName.NONE;
                }
            }
        }

        public void OnNotify(ISubject subject)
        {
            if (subject is Observable<DiamondName> observable)
            {
                if (observable.Value == DiamondName.NONE)
                    return;
                foreach (var currentRingName in new List<RingName> { RingName.LEFT, RingName.RIGHT })
                {
                    for (int i = 0; i < RingDiamondNamesMap[currentRingName].Count; i++)
                    {
                        if (subject == RingDiamondNamesMap[currentRingName][i])
                        {
                            RemoveSameDiamondFromDeck(currentRingName, i);
                            return;
                        }
                    }
                }
            }
        }
    }
}
